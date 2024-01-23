using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

namespace Strategy {
	
	public class TestAStar : MonoBehaviour
	{
		public Transform pos0;
		public Transform pos1;
		private Path path;
		private Seeker seeker;
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			seeker = GetComponent<Seeker>();
		}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			if (path != null) {
				Util.DrawPathGizmos(path.vectorPath, Color.red);
			}
		}
		
		void OnPathComplete(Path path) {
			this.path = path;
		}
		
		[Button]
		void Test() {
			seeker.StartPath(transform.position, pos1.position, OnPathComplete);
		}
		
		[Button]
		void TestReverse() {
			seeker.StartPath(pos1.position, pos0.position, OnPathComplete);
		}
	}
}

