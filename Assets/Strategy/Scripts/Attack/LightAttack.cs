using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class LightAttack : FireAttack
	{
		public GameObject missilePrefab;
		public float range = 50;
		public Transform spawnPoint;
		
		protected override void Attack(Unit target) {
			var missile = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
			var d = target.Body.position - transform.position;
			d.y = 0;
			Vector3 targetPos = d.normalized * range + transform.position;
			var proj = missile.GetComponent<ProjectileTrace>();
			proj.targetPos = targetPos;
			proj.fixedTarget = true;
		}
	}
}
