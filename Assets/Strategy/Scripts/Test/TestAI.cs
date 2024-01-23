using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using MEC;

namespace Strategy {
	
	public class NewBehaviourScript : MonoBehaviour
	{
		public Transform target;
		
		private NavMeshAgent agent;
		private NavMeshObstacle obstacle;
		public float radius = 5;
		
		private float lastTime = 0;
	
		// Start is called before the first frame update
		void Start()
		{
			agent = GetComponent<NavMeshAgent>();
			obstacle = GetComponent<NavMeshObstacle>();
		}
		
		[Button]
		void Test() {
			agent.destination = target.position;
			Debug.Log($"start hasPath {agent.hasPath} {Time.frameCount}");
			lastTime = Time.time;
		}
		
		//IEnumerator<float> Task() {
			
		//}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			if (agent != null && agent.hasPath) {
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(agent.pathEndPosition, 1f);
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (agent.enabled == false) {
				return;
			}
			var d = (transform.position.XZ() - target.position.XZ()).magnitude;
			if (d < radius) {
				agent.Stop();
				agent.enabled = false;
				obstacle.enabled = true;
			}
		}
	}
}

