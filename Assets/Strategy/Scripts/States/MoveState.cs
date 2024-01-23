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
		public float monitorInterval = 1f;
		
		private CoroutineHandle handle;
		private Vector3 endOfPath = Vector3.zero;
		private bool pathPending = false;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.AgentWake();
			if (unit.config.moveClip != null) {
				unit.animancer.Play(unit.config.moveClip);
			}
			handle = Timing.RunCoroutine(MoveToTarget().CancelWith(gameObject));
		}
		
		private void OnPathComplete(Path path) {
			Debug.Log($"Move state on path complete: {path.error} {path.vectorPath.Last()}");
			endOfPath = path.vectorPath.Last();
			pathPending = false;
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
			Timing.KillCoroutines(handle);
		}
		
		private Vector3 dest;
		
		private IEnumerator<float> MoveToTarget() {
			while (true) {
				pathPending = true;
				dest = unit.TargetPos;
				unit.SetAgentDestination(dest, OnPathComplete);
				yield return Timing.WaitUntilFalse(() => pathPending);
				
				if ((endOfPath - dest).XZ().magnitude > 0.5f) {
					Debug.Log($"clear move: stuck {endOfPath} {dest}");
					unit.ClearMoveTarget();
					yield break;
				}
				
				yield return Timing.WaitForSeconds(monitorInterval);
			}
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			if ((transform.position - dest).XZ().magnitude < 0.2f) {
				Debug.Log($"clear move: reach target {dest}");
				unit.ClearMoveTarget();
			}
		}
	}
}

