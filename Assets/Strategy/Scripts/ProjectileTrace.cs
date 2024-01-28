using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class ProjectileTrace : MonoBehaviour
	{
		public Transform target;
		public float radius;
		public float speed;
		public float lerpFactor = 0.2f;
	
		// Start is called before the first frame update
		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 d = target.position - transform.position;
			
			if (d.magnitude < radius) {
				Debug.Log("Hit Target");
				Destroy(gameObject);
				return;
			}

			transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
			transform.LookAt(target, Vector3.up);
		}
	}

}
