﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Assertions;
using Pathfinding;
using MEC;

namespace Strategy {
	
	public class ChaseState : UnitState
	{
		private CoroutineHandle handle;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.moveClip != null) {
				animancer.Play(unit.moveClip);
			}
			handle = Timing.RunCoroutine(SetTarget().CancelWith(gameObject));
		}
		
		private Vector3 targetPos;
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmosSelected()
		{
			if (unit != null && unit.NavBody != null) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine(unit.NavBody.transform.position + Vector3.up * 0.1f, targetPos + Vector3.up * 0.1f);
			}
		}
		
		IEnumerator<float> SetTarget() {
			Unit target = null;
			
			while (true) {
				
				target = GetTarget(target);
			
				Vector3 dest = unit.NavBody.position;
				
				if (target != null) {
					dest = target.NavBody.position;
				}
				else {
					Assert.IsTrue(unit.TeamId == 1, "self team cannot get target enemies");
					Vector3 bottomPos = unit.NavBody.position;
					bottomPos.z = GameController.Instance.bottomLine;
					if (unit.NavBody.transform.position.z < bottomPos.z
						&& Mathf.Abs(unit.NavBody.transform.position.z - bottomPos.z) > 6f) {
						dest = bottomPos;
					}
					else {
						dest = GameController.Instance.BaseTower.position;
					}
				}
				
				targetPos = dest;
			
				unit.SetDestination(dest);
	
				yield return Timing.WaitForSeconds(unit.refreshTargetInterval);
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			animancer.Stop();
			Timing.KillCoroutines(handle);
		}
		
		protected virtual Unit GetTarget(Unit currTarget) {
			if (currTarget != null && !currTarget.IsDead) {
				float d = Util.XZDistance(currTarget.transform.position, unit.NavBody.transform.position);
				if (d >= unit.ChaseMinRadius && d < unit.ChaseRadius) {
					return currTarget;
				}
			}
			
			var e = Util.GetNearestLiveEnemy(unit.TeamId,
				unit.NavBody.position,
				unit.ChaseMinRadius,
				unit.ChaseRadius,
				unit.attackLayers);
			
			if (e != null) {
				return e;
			}
	
			return null;
		}
	}
}

