using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Strategy {
	
	public class MoveState : UnitState
	{
		public float stuckPosDelta = 1f;
		public float monitorInterval = 1f;

		private float lastMonitorTime = 0f;
		private Vector3 lastMonitorPos = Vector3.zero;
		private Tween timer = null;
	
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.AgentWake();
			unit.SetAgentDestination(unit.TargetPos);
			lastMonitorTime = Time.time;
			lastMonitorPos = transform.position;
			if (unit.config.moveClip != null) {
				unit.animancer.Play(unit.config.moveClip);
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			if ((Time.time - lastMonitorTime) > monitorInterval) {
				//float posDelta = (transform.position - lastMonitorPos).magnitude;
				if (unit.IsStuck(unit.TargetPos)) {
					// Event Stuck
					unit.ClearMoveTarget();
				} else {
					lastMonitorTime = Time.time;
					//lastMonitorPos = transform.position;
				}
			}
		}
	}
}

