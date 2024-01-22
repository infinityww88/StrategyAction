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
			if ((transform.position - target.position).magnitude < radius) {
				Debug.Log("Hit Target");
				Destroy(gameObject);
				return;
			}
		
			Vector3 v = (target.position - transform.position).normalized;
			v = Vector3.Lerp(transform.forward, v, lerpFactor);
			transform.LookAt(transform.position + v, Vector3.up);
			transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
		}
	}

}
