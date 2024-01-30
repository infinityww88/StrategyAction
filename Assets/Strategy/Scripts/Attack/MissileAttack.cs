using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class MissileAttack : BaseAttack
	{
		public GameObject missilePrefab;
		public Transform spawnPoint;
		
		private CoroutineHandle attackCoroHandle;
		
		public float attackInterval = 1f;
		private float lastLaunchTime = 0;
		
		private Unit target;
		
		public override bool HasTarget() {
			return TargetIsValid(target);
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
				float cd = Mathf.Max(0, attackInterval - (Time.time - lastLaunchTime));
				if (cd > 0) {
					yield return Timing.WaitForSeconds(cd);
				}
				
				if (!HasTarget()) {
					ScanTarget();
					yield return Timing.WaitForOneFrame;
					continue;
				}
				yield return Timing.WaitUntilDone(Util.LookAtTarget(transform, target.Body, 5f, 0.1f));
				var missile = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
				missile.GetComponent<ProjectileTrace>().target = target.Body.transform;
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
