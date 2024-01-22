using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Strategy {
	
	public class UnitStuckMonitor : MonoBehaviour
	{
		public float stuckPosDelta = 1f;
		public float monitorInterval = 1f;
	
		private float lastMonitorTime = 0f;
		private Vector3 lastMonitorPos = Vector3.zero;
		private Tween timer = null;
	
		private TweenCallback onStuck = null;
    
		public void StartMonitor(TweenCallback onStuck) {
			this.onStuck = onStuck;
			lastMonitorTime = Time.time;
			lastMonitorPos = transform.position;
			timer = DOTween.Sequence()
				.AppendInterval(monitorInterval)
				.AppendCallback(CheckStuck)
				.SetTarget(gameObject)
				.SetLoops(-1);
		}
	
		public void StopMonitor() {
			timer.Kill();
		}
	
		void CheckStuck() {
			float posDelta = (transform.position - lastMonitorPos).magnitude;
			if (posDelta < stuckPosDelta) {
				// Event Stuck
				onStuck?.Invoke();
				StopMonitor();
			} else {
				lastMonitorTime = Time.time;
				lastMonitorPos = transform.position;
			}
		}
	}
}
