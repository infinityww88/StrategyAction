using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class FireAttack : BaseAttack
	{		
		private CoroutineHandle attackCoroHandle;

		public float attackInterval = 1f;
		public bool hasCD = true;
		public float lookAtAngleThreshold = 1f;
		public float lookAtLerpFactor = 0.2f;
		
		private float lastLaunchTime = 0;
		private Unit target;
		
		public override bool HasTarget() {
			return TargetIsValid(target);
		}
		
		protected virtual void Attack(Unit target) {
			
		}
		
		public override void ScanTarget() {
			target = Util.GetNearestLiveEnemy(unit.TeamId,
				transform.position,
				attackMinRadius,
				attackMaxRadius,
				unit.attackLayers
			);
		}
		
		public IEnumerator<float> AttackCoro() {
			while (true) {
				
				if (hasCD) {
					float cd = Mathf.Max(0, attackInterval - (Time.time - lastLaunchTime));
					if (cd > 0) {
						yield return Timing.WaitForSeconds(cd);
					}
				}
				
				if (!HasTarget()) {
					ScanTarget();
					yield return Timing.WaitForOneFrame;
					continue;
				}
				
				if (Util.GetLookAtAngle(transform, target.NavBody.transform) > lookAtAngleThreshold) {
					yield return Timing.WaitUntilDone(Util.LookAtTarget(transform,
						target.Body,
						lookAtAngleThreshold,
						lookAtLerpFactor));
				}
				
				Attack(target);
				
				lastLaunchTime = Time.time;
				
				yield return Timing.WaitForSeconds(attackInterval);
			}
		}
		
		public override void StartAttack() {
			attackCoroHandle = Timing.RunCoroutine(AttackCoro().CancelWith(gameObject));
		}
		
		public override void StopAttack() {
			Timing.KillCoroutines(attackCoroHandle);
		}
	}
}
