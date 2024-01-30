using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class MissileAttack : FireAttack
	{
		public GameObject missilePrefab;
		public Transform spawnPoint;
		
		protected override void Attack(Unit target) {
			var missile = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
			missile.GetComponent<ProjectileTrace>().target = target.Body.transform;
		}
	}
}
