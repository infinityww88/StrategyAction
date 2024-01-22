using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Strategy {
	
	public class Projectile : MonoBehaviour
	{	    
		/*
		Vx * t = Sx;
		Vy * t + 0.5 * g * t^2 = Sy;
		Vx = aVy;
		    
		a * Vy * t = Sx;
		Vy * t = Sx / a;
		Sx / a + 0.5 * g * t ^ 2 = Sy;
		t = Sqrt(2 * (Sy - Sx / a) / g)
		*/
		public void Project(Vector3 targetPos)
		{
			var s = targetPos - transform.position;
			var forward = transform.forward;
			var t = forward;
			t.y = 0;
			float vx = t.magnitude;
			float vy = forward.y;
			float a = vx / vy;
			t = s;
			t.y = 0;
			float sx = t.magnitude;
			float sy = s.y;
			float b = 2 * (sy - sx / a) / -Physics.gravity.magnitude;
			//Debug.Log($"s {s}, vx {vx}, vy {vy}, a {a}, sx {sx}, sy {sy}, b {b}");
			Assert.IsTrue(b > 0, "project impossible");
			float time = Mathf.Sqrt(b);
			vx = sx / time;
			vy = vx / a;
			Vector3 velocity = forward * Mathf.Sqrt(vx * vx + vy * vy);
			GetComponent<Rigidbody>().velocity = velocity;
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			
		}
	}
	
}