using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation;
using DG.Tweening;
using System;
using System.Linq;
using MEC;

namespace Strategy {
	
	[System.Flags]
	public enum UnitLayer {
		Ground = 1 << 0,
		Sky = 1 << 1,
		ALL = -1,
		None = 0,
	}
	
	public class Unit : MonoBehaviour
	{
		public UnitConfig config;
		
		[SerializeField]
		private float chaseRadius;
		
		[SerializeField]
		private float chaseMinRadius;
		
		private AgentAuthoring agent;
		
		[SerializeField]
		private Transform body;
		
		[SerializeField]
		private GameObject selectedCircle;
		
		[SerializeField]
		private int teamId = 0;
		
		public int TeamId => teamId;
		
		public Vector3 Destination { get; set; }
		
		public bool IsFreezing { get; set; }
		
		private UnitState currState = null;
		
		public float ChaseRadius => chaseRadius;
		
		public float ChaseMinRadius => chaseMinRadius;
		
		private IdleState idleState;
		private ChaseState chaseState;
		private DeadState deadState;
		private AttackState attackState;
		private MoveState moveState;
		private FreezeState freezeState;
		
		public float attackSpeed = 1;

		private List<Unit> enemiesInChaseRange = new List<Unit>();
		
		private bool hasMoveTarget = false;
		private Vector3 targetPos = Vector3.zero;
		
		public Vector3 TargetPos => targetPos;
		
		public float hp;
		
		public float stuckPosDelta = 0.5f;
		
		public UnitLayer attackLayers;
		public UnitLayer unitLayer;
		
		private CoroutineHandle alignVelocityHandle;
		private CoroutineHandle alignPositionHandle;
		
		private BaseAttack[] attackBehaviors;
		
		public IEnumerable<BaseAttack> AttackBehaviors => attackBehaviors;
	
		public enum EState {
			Idle,
			Chase,
			Freeze,
			Attack,
			Dead
		}
		
		void Awake() {
			//animancer = GetComponentInChildren<AnimancerComponent>();
			
			agent = GetComponentInChildren<AgentAuthoring>();
			
			idleState = GetComponent<IdleState>();
			chaseState = GetComponent<ChaseState>();
			deadState = GetComponent<DeadState>();
			attackState = GetComponent<AttackState>();
			moveState = GetComponent<MoveState>();
			freezeState = GetComponent<FreezeState>();
			
			attackBehaviors = body.GetComponentsInChildren<BaseAttack>();
		}
		
		public Transform NavBody => agent?.transform;
		public Transform Body => body;
	
		public EState State { get; set; }
		
		private List<Func<bool>> actions = new List<Func<bool>>();
		
		public bool IsDead {
			get {
				return hp <= 0;
			}
		}
		
		public Vector3 GetAgentVelocity() {
			if (!agent.HasEntityBody || agent.EntityBody.IsStopped) {
				return Vector3.zero;
			}
			return agent.EntityBody.Velocity;
		}
		
		public List<Unit> GetEnemiesInChaseRange() {
			return enemiesInChaseRange;
		}
		
		public void SetMoveTarget(Vector3 targetPos) {
			hasMoveTarget = true;
			this.targetPos = targetPos;
		}
		
		public void ClearMoveTarget() {
			hasMoveTarget = false;
		}
		
		void UpdateChaseEnemies() {
			enemiesInChaseRange.Clear();
			enemiesInChaseRange.AddRange(
				Util.GetUnits(
				NavBody.transform.position,
				ChaseMinRadius,
				ChaseRadius,
				Util.EnemyTeamId(TeamId),
				attackLayers));
		}
		
		void SwitchState(UnitState newState) {
			if (currState == newState) {
				return;
			}
						
			if (currState != null) {
				currState.enabled = false;
			}
			
			currState = newState;
			
			if (currState != null) {
				currState.enabled = true;
			}
		}
		
		private List<Vector3> remainPath = new	List<Vector3>();
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmosSelected()
		{
			var center = NavBody == null ? transform.position : NavBody.position;
			DebugExtension.DrawCircle(center + Vector3.one * 0.2f,
				Vector3.up,
				Color.HSVToRGB(0.5f, 0.5f, 1),
				ChaseMinRadius);
			DebugExtension.DrawCircle(center + Vector3.one * 0.2f,
				Vector3.up,
				Color.HSVToRGB(0.5f, 1, 1),
				ChaseRadius);
		}
		
		public void ShowSelectCircle(bool show) {
			selectedCircle.SetActive(show);
		}
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			hp = config.maxHp;
			
			alignVelocityHandle = Timing.RunCoroutine(Util.AlignAgentRotation(
				Body, GetAgentVelocity, 0.2f).CancelWith(gameObject));
			alignPositionHandle = Timing.RunCoroutine(Util.AlignAgentPosition(
				Body, () => NavBody.position, 0.2f).CancelWith(gameObject));
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDestroy()
		{
			Timing.KillCoroutines(alignVelocityHandle);
			Timing.KillCoroutines(alignPositionHandle);
		}
		
		public void SetDestination(Vector3 dest) {
			agent.SetDestination(dest);
		}
		
		public void StopAgent() {
			agent.Stop();
		}
		
		public void EnableAgent(bool enabled) {
			agent.enabled = enabled;
		}
		
		public bool InAttackAnimation { get; set; }

		void Update() {
			
			if (hp <= 0) {
				SwitchState(deadState);
			}
			else if (IsFreezing) {
				SwitchState(freezeState);
			}
			else if (GameController.Instance.state == GameController.EState.Prepare
				|| GameController.Instance.state == GameController.EState.GameOver) {
				SwitchState(idleState);
			}
			else if (hasMoveTarget) {
				SwitchState(moveState);
			}
			else {
				if (InAttackAnimation) {
					return;
				}

				if (!(currState is AttackState)) {
					attackBehaviors.Foreach(e => e.ScanTarget());
				}
				
				bool hasAttackTarget = attackBehaviors.Any(e => e.HasTarget());
				
				if (hasAttackTarget) {
					SwitchState(attackState);
				}
				else {
					UpdateChaseEnemies();
					
					if (Util.GetLiveUnits(enemiesInChaseRange).Count() > 0 || TeamId == 1) {
						SwitchState(chaseState);
					}
					else {
						SwitchState(idleState);
					}
				}
			}
		}
	}
}

