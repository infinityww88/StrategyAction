using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ProjectDawn.Navigation.Hybrid;
using Animancer;
using DG.Tweening;
using System;
using System.Linq;

namespace Strategy {
	
	public class Unit : MonoBehaviour
	{
		public UnitConfig config;
		
		private AgentAuthoring agent;
		
		[SerializeField]
		private int teamId = 0;
		
		public int TeamId => teamId;
		
		public AnimancerComponent animancer;
		
		public Vector3 Destination { get; set; }
		
		public bool IsFreezing { get; set; }
		
		private UnitState currState = null;
		
		private IdleState idleState;
		private ChaseState chaseState;
		private DeadState deadState;
		private AttackState attackState;
		private MoveState moveState;
		private FreezeState freezeState;
		
		public float attackSpeed;
		
		private List<Unit> enemiesInAttackRange = new	List<Unit>();
		private List<Unit> enemiesInChaseRange = new	List<Unit>();
		
		private bool hasMoveTarget = false;
		private Vector3 targetPos = Vector3.zero;
		
		public Vector3 TargetPos => targetPos;
		
		public float hp;
		
		public float stuckPosDelta = 0.5f;
	
		public enum EState {
			Idle,
			Chase,
			Freeze,
			Attack,
			Dead
		}
		
		void Awake() {
			animancer = GetComponentInChildren<AnimancerComponent>();
			
			agent = GetComponent<AgentAuthoring>();
			
			idleState = GetComponent<IdleState>();
			chaseState = GetComponent<ChaseState>();
			deadState = GetComponent<DeadState>();
			attackState = GetComponent<AttackState>();
			moveState = GetComponent<MoveState>();
			freezeState = GetComponent<FreezeState>();
		}
	
		public EState State { get; set; }
		
		private List<Func<bool>> actions = new List<Func<bool>>();
		
		public bool IsDead {
			get {
				return hp <= 0;
			}
		}
		
		public Vector3 GetAgentVelocity() {
			return agent.EntityBody.Velocity;
		}
		
		public List<Unit> GetEnemiesInAttackRange() {
			return enemiesInAttackRange;
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
		
		void UpdateAttackEnemies() {
			enemiesInAttackRange.Clear();
			enemiesInAttackRange.AddRange(
				Util.GetUnits(
					transform.position,
					config.attackMinRadius,
					config.attackMaxRadius,
					Util.EnemyTeamId(TeamId)));
		}
		
		void UpdateChaseEnemies() {
			enemiesInChaseRange.Clear();
			enemiesInChaseRange.AddRange(
				Util.GetUnits(
				transform.position,
				config.attackMaxRadius,
				config.chaseRadius,
				Util.EnemyTeamId(TeamId)));
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
		protected void OnDrawGizmos()
		{
			DebugExtension.DrawCylinder(transform.position,
				transform.position + Vector3.up * 2, 
				Color.red,
				config.attackMinRadius);
			DebugExtension.DrawCylinder(transform.position,
				transform.position + Vector3.up * 2, 
				Color.blue,
				config.attackMaxRadius);
			if (TeamId != 0) {
				DebugExtension.DrawCylinder(transform.position,
					transform.position + Vector3.up * 2, 
					Color.yellow,
					config.chaseRadius);
			}
		}
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			hp = config.maxHp;
		}
		
		
		public void SetDestination(Vector3 dest) {
			agent.SetDestination(dest);
		}
		
		public void StopAgent() {
			agent.Stop();
		}

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
				UpdateAttackEnemies();
				if (Util.GetLiveUnits(enemiesInAttackRange).Count() > 0) {
					SwitchState(attackState);
				}
				else {
					UpdateChaseEnemies();
					var s = currState as AttackState;
					if (s != null && s.InAttack) {
						return;
					}
					if (Util.GetLiveUnits(enemiesInChaseRange).Count() > 0 || TeamId == 1) {
						SwitchState(chaseState);
					}
				}
			}
		}
	}
}

