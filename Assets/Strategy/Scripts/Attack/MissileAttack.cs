using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class MissileAttack : BaseAttack
	{
		public GameObject missilePrefab;
		public Transform spawnPoint;
		
		public override void ApplyAttack(Unit target) {
			if (target == null || target.IsDead) {
				return;
			}
			var missile = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
			missile.GetComponent<ProjectileTrace>().target = target.transform;
		}
	}
}
