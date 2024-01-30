using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
using Pathfinding;
using System.Linq;

namespace Strategy {
	
	public class MoveState : UnitState
	{
		public float stuckPosDelta = 1f;
		public float reachRadius = 0.2f;
		public float targetUpdateInterval = 0.5f;
		public float stuckMonitorInterval = 5f;
		
		private CoroutineHandle targetUpadteHandle;
		private CoroutineHandle stuckMonitorHandle;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.config.moveClip != null) {
				animancer.Play(unit.config.moveClip);
			}
			targetUpadteHandle = Timing.RunCoroutine(MoveToTarget().CancelWith(gameObject));
			stuckMonitorHandle = Timing.RunCoroutine(MonitorStuck().CancelWith(gameObject));
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			animancer.Stop();
			Timing.KillCoroutines(targetUpadteHandle);
			Timing.KillCoroutines(stuckMonitorHandle);
		}
		
		private Vector3 dest;
		
		private IEnumerator<float> MoveToTarget() {
			while (true) {
				dest = unit.TargetPos;
				unit.SetDestination(dest);
				
				yield return Timing.WaitForSeconds(targetUpdateInterval);
			}
		}
		
		private IEnumerator<float> MonitorStuck() {
			while (true) {
				yield return Timing.WaitForSeconds(stuckMonitorInterval);
				if (Util.XZDistance(unit.NavBody.transform.position, dest) < stuckPosDelta) {
					Debug.Log($"clear move: stuck {unit.NavBody.transform.position} {dest}");
					unit.ClearMoveTarget();
					break;
				}
			}
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			if (Util.XZDistance(unit.NavBody.transform.position, dest) < 0.2f) {
				Debug.Log($"clear move: reach target {dest}");
				unit.ClearMoveTarget();
			}
		}
	}
}

