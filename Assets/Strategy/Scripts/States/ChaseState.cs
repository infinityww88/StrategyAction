using System.Collections;
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
			if (unit.config.moveClip != null) {
				unit.animancer.Play(unit.config.moveClip);
			}
			handle = Timing.RunCoroutine(SetTarget().CancelWith(gameObject));
		}
		
		private Vector3 targetPos;
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, targetPos + Vector3.up * 0.1f);
		}
		
		IEnumerator<float> SetTarget() {
			Unit target = null;
			
			while (true) {
				
				target = GetTarget(target);
			
				Vector3 dest = transform.position;
				
				if (target != null) {
					dest = target.transform.position;
				}
				else {
					Assert.IsTrue(unit.TeamId == 1, "self team cannot get target enemies");
					Vector3 bottomPos = unit.transform.position;
					bottomPos.z = GameController.Instance.bottomLine;
					if (unit.transform.position.z < bottomPos.z
						&& Mathf.Abs(unit.transform.position.z - bottomPos.z) > 6f) {
						dest = bottomPos;
					}
					else {
						dest = GameController.Instance.BaseTower.position;
					}
				}
				
				targetPos = dest;
			
				unit.SetDestination(dest);
	
				yield return Timing.WaitForSeconds(unit.config.refreshTargetInterval);
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
			Timing.KillCoroutines(handle);
		}
		
		protected virtual Unit GetTarget(Unit currTarget) {
			if (currTarget != null && !currTarget.IsDead) {
				float d = Util.XZDistance(currTarget.transform.position, transform.position);
				if (d >= unit.config.attackMaxRadius && d < unit.config.chaseRadius) {
					return currTarget;
				}
			}
			
			var e = Util.GetNearest(transform.position,
				Util.GetLiveUnits(unit.GetEnemiesInChaseRange()));
			
			if (e != null) {
				return e;
			}
	
			return null;
		}
	}
}

