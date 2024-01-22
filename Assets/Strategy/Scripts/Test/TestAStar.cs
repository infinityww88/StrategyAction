using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

namespace Strategy {
	
	public class TestAStar : MonoBehaviour
	{
		public Transform targetPos;
	
		private Seeker seeker;
		private RichAI richAI;
		private NavmeshCut navmeshCut;
	
		public float reachRadius = 0.1f;
	
		// Start is called before the first frame update
		void Start()
		{
			seeker = GetComponent<Seeker>();
			richAI = GetComponent<RichAI>();
			navmeshCut = GetComponent<NavmeshCut>();
		}

		[Button]
		void StartPath() {
			seeker.StartPath(transform.position, targetPos.position, OnPathComplete);
		}
	
		void OnPathComplete(Path path) {
		
		}
	
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			if ((transform.position - targetPos.position).XZ().magnitude < reachRadius) {
				richAI.isStopped = true;
				richAI.enabled = false;
				seeker.enabled = false;
				navmeshCut.enabled = true;
			}
		}
	}
}

