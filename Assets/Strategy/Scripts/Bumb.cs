using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class Bumb : MonoBehaviour
	{
		public GameObject effect;
		public LayerMask layerMask;
		public float scaleSize = 5f;
		
		// Start is called before the first frame update
		void Start()
		{
        
		}
		
		// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
		protected void OnCollisionEnter(Collision collisionInfo)
		{
			if ((layerMask.value | collisionInfo.gameObject.layer) != 0) {
				var e = Instantiate(effect, transform.position, Quaternion.identity);
				e.transform.localScale = Vector3.one * scaleSize;
				Destroy(gameObject);
			}
		}
	}

}
