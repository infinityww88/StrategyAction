using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MEC;

namespace Strategy {
	
	public class CannonAttack : FireAttack
	{
		public Transform bulletSpawnPoint;
		public GameObject bulletPrefab;
		
		protected override void Attack(Unit target) {
			var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
			bullet.GetComponent<Projectile>().Project(target.NavBody.position);
			Destroy(bullet, 8);
		}
	}

}
