using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class LaserController : MonoBehaviour
	{
		private Transform target;
		
		private LineRenderer lineRenderer;
		
		private CoroutineHandle attackCoroHandler;
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			lineRenderer = GetComponent<LineRenderer>();
		}
		
		public void Light(Transform target) {
			lineRenderer.enabled = true;
			this.target = target;
		}
		
		public void Close() {
			target = null;
			lineRenderer.enabled = false;
		}
		
		void Update() {
			if (target == null) {
				return;
			}
			var p1 = transform.InverseTransformPoint(target.position);
			lineRenderer.SetPosition(1, p1);
		}
	}

}
