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
		private AnimancerState currState;
		
		public Unit Target => target;
		
		private CoroutineHandle handle;
		
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
				Gizmos.DrawSphere(target.transform.position, 0.2f);
			}
		}
		
		private IEnumerator<float> StartAttack() {
			InAttack = false;
			while (true) {
				target = GetTarget(target);
				ConsoleProDebug.Watch($"{gameObject.name} attack target", $"{target}");
				if (target == null) {
					yield return Timing.WaitForOneFrame;
					continue;
				}
				yield return Timing.WaitUntilDone(LookAtTarget());
				if (unit.config.attackClip != null) {
					InAttack = true;
					Debug.Log($"{gameObject.name} {Time.frameCount} start Attack");
					unit.animancer.Stop();
					currState = unit.animancer.Play(unit.config.attackClip);
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
		
		public void Attack() {
			
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
				float d = (currTarget.transform.position - transform.position).XZ().magnitude;
				if (d >= unit.config.attackMinRadius && d < unit.config.attackMaxRadius) {
					return target;
				}
			}
			var targets = Util.GetLiveUnits(unit.GetEnemiesInAttackRange());
			if (targets.Count() == 0) {
				return null;
			}
			target = Util.GetNearest(transform.position, targets.Where(e => !e.IsDead));
			return target;
		}
		
		public bool InAttack { get; set; }
		
		private IEnumerator<float> LookAtTarget() {
			Transform body = unit.animancer.transform;
			while (true) {
				var lookv = target.transform.position - body.position;
				lookv.y = 0;
				float angle = Vector3.Angle(body.forward, lookv);
				if (angle < 1)  {
					break;
				}
				lookv = Vector3.Lerp(body.forward, lookv, 0.01f).normalized;
				body.LookAt(body.position + lookv, Vector3.up);
				yield return Timing.WaitForOneFrame;
			}
		}
	}

}
