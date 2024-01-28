using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Animancer;
using System.Linq;
using MEC;

namespace Strategy {
	
	public class AttackState : UnitState
	{
		private Unit target;
		private BaseAttack attackBehavior;
		private AnimancerState currState;
		
		public Unit Target => target;
		
		private CoroutineHandle handle;
		
		protected void Awake() {
			base.Awake();
			attackBehavior = GetComponent<BaseAttack>();
		}
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			Assert.IsTrue(Util.GetLiveUnits(unit.GetEnemiesInAttackRange()).Count() > 0, "No enemies in attack state");
			unit.StopAgent();
			handle = Timing.RunCoroutine(StartAttack().CancelWith(gameObject));
		}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			if (target != null) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(target.transform.position, 0.5f);
				/*
				var body = unit.animancer.transform;
				var lookv = target.transform.position - body.position;
				
				Gizmos.color = Color.yellow;
				
				Gizmos.DrawLine(body.position, body.position + lookv * 100);
				Gizmos.DrawLine(body.position, body.position + body.forward * 100);
				*/
			}
		}
		
		private void Attack() {
			attackBehavior.ApplyAttack(target);
		}
		
		private IEnumerator<float> StartAttack() {
			InAttack = false;
			while (true) {
				target = GetTarget(target);
				if (target == null) {
					yield return Timing.WaitForOneFrame;
					continue;
				}
				yield return Timing.WaitUntilDone(LookAtTarget());
				if (unit.config.attackClip != null) {
					InAttack = true;
					unit.animancer.Stop();
					currState = unit.animancer.Play(unit.config.attackClip);
					currState.Speed = unit.attackSpeed;
					yield return Timing.WaitUntilTrue(CurrentStateEnd);
					InAttack = false;
					currState = null;
					yield return Timing.WaitForOneFrame;
				}
			}
		}
		
		private bool CurrentStateEnd() {
			return currState != null && currState.NormalizedTime >= 1;
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			if (unit.config.attackClip != null) {
				unit.animancer.Stop();
			}
			Timing.KillCoroutines(handle);
		}
		
		private Unit GetTarget(Unit currTarget) {
			if (currTarget != null && !currTarget.IsDead) {
				float d = Util.XZDistance(currTarget.transform.position, transform.position);
				if (d >= unit.config.attackMinRadius && d < unit.config.attackMaxRadius) {
					return currTarget;
				}
			}
			var targets = Util.GetLiveUnits(unit.GetEnemiesInAttackRange());
			if (targets.Count() == 0) {
				return null;
			}
			currTarget = Util.GetNearest(transform.position, targets.Where(e => !e.IsDead));
			return currTarget;
		}
		
		public bool InAttack { get; set; }
		
		private IEnumerator<float> LookAtTarget() {
			Transform body = unit.animancer.transform;
			while (true) {
				var lookv = target.transform.position - body.position;
				lookv.y = 0;
				var forward = body.forward;
				forward.y = 0;
				lookv.Normalize();
				forward.Normalize();
				float angle = Vector3.Angle(forward, lookv);
				if (angle < 0.2f)  {
					break;
				}
				lookv = Vector3.Lerp(forward, lookv, 0.1f);
				body.LookAt(body.position + lookv, Vector3.up);
				yield return Timing.WaitForOneFrame;
			}
		}
	}

}
