using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MEC;

namespace Strategy {
	
	public class CannonAttack : BaseAttack
	{
		public Transform bulletSpawnPoint;
		public GameObject bulletPrefab;
		public float attackInterval = 1f;
		private CoroutineHandle attackCoroHandle;
		
		private Unit target;
		
		public override bool HasTarget() {
			return TargetIsValid(target);
		}
		
		public override void ScanTarget() {
			target = Util.GetNearestLiveEnemy(unit.TeamId,
				transform.position,
				attackMinRadius,
				attackMaxRadius,
				unit.attackLayers);
		}
		
		public IEnumerator<float> AttackCoro() {
			while (true) {
				if (!HasTarget()) {
					ScanTarget();
					yield return Timing.WaitForOneFrame;
					continue;
				}
				
				yield return Timing.WaitUntilDone(Util.LookAtTarget(transform, target.NavBody, 0.2f, 0.1f));
			
				var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
				bullet.GetComponent<Projectile>().Project(target.NavBody.position);
				Destroy(bullet, 8);
				
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
