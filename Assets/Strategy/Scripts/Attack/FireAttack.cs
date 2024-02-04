using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class FireAttack : BaseAttack
	{
		public float attackInterval = 1f;
		public bool hasCD = true;
		public float lookAtAngleThreshold = 1f;
		public float lookAtLerpFactor = 0.2f;
		
		private float lastLaunchTime = -1000;
		
		protected virtual void Attack(Unit target) {
			
		}
	
		protected override IEnumerator<float> AttackCoro() {
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
				
				if (Util.GetLookAtAngle(transform, target.Body) > lookAtAngleThreshold) {
					yield return Timing.WaitUntilDone(Util.LookAtTarget(transform,
						target.Body,
						lookAtAngleThreshold,
						lookAtLerpFactor).KillWith(Timing.CurrentCoroutine));
				}
				
				Attack(target);
				
				lastLaunchTime = Time.time;
				
				yield return Timing.WaitForSeconds(attackInterval);
			}
		}
	}
}
