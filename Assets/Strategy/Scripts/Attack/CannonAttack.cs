using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class CannonAttack : BaseAttack
	{
		public Transform bulletSpawnPoint;
		public GameObject bulletPrefab;
		
		public override void ApplyAttack(Unit target) {
			if (target == null || target.IsDead) {
				return;
			}
			
			var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
			bullet.GetComponent<Projectile>().Project(target.transform.position);
			Destroy(bullet, 8);
		}
	}

}
