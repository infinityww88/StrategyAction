using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class Cannon : MonoBehaviour
	{
		public Transform bulletSpawnPoint;
		public GameObject bulletPrefab;

		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			var target = GetComponent<AttackState>().Target;
			if (target != null) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(target.transform.position, 1f);
			}
		}
		
		public void Attack() {
			var target = GetComponent<AttackState>().Target;
			var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
			bullet.GetComponent<Projectile>().Project(target.transform.position);
			Destroy(bullet, 5);
		}
	}

}
