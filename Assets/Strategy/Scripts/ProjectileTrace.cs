using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class ProjectileTrace : MonoBehaviour
	{
		public Transform target;
		public Vector3 targetPos;
		public bool fixedTarget = false;
		public float radius;
		public float speed;
		public float lerpFactor = 0.2f;

		// Update is called once per frame
		void Update()
		{
			Vector3 destPos = Vector3.zero;
			if (fixedTarget) {
				destPos = targetPos;
			}
			else {
				destPos = target.position;
			}
			Vector3 d = destPos - transform.position;
			
			if (d.magnitude < radius) {
				//Debug.Log("Hit Target");
				Destroy(gameObject);
				return;
			}

			transform.position = Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * speed);
			transform.LookAt(destPos, Vector3.up);
		}
	}

}
