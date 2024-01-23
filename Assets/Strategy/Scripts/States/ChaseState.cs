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
		public float stuckUnitWaitTime = 5f;
		private Vector3 endOfPath = Vector3.zero;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.config.moveClip != null) {
				unit.animancer.Play(unit.config.moveClip);
			}
			unit.AgentWake();
			handle = Timing.RunCoroutine(SetTarget().CancelWith(gameObject));
		}
		
		protected struct StuckUnit {
			public Unit unit;
			public float time;
		}
		
		void RemoveStuckUnits(List<StuckUnit> units) {
			while (units.Count > 0) {
				if (Time.time - units.First().time >= stuckUnitWaitTime) {
					units.RemoveAt(0);
				}
				else {
					return;
				}
			}
		}
		
		bool StuckUnitsContains(List<StuckUnit> units, Unit u) {
			foreach (var e in units) {
				if (e.unit == u) {
					return true;
				}
			}
			return false;
		}
		
		IEnumerator<float> SetTarget() {
			
			List<StuckUnit> stuckUnits = new List<StuckUnit>();
			Unit target = null;
			
			while (true) {
				RemoveStuckUnits(stuckUnits);
				
				target = GetTarget(target, stuckUnits);
			
				Vector3 dest = Vector3.zero;
				
				if (target != null) {
					dest = target.transform.position;
				}
				else {
					if (unit.TeamId == 1) {
						dest = GameController.Instance.BaseTower.position;
					}
					else {
						yield return Timing.WaitForSeconds(unit.config.refreshTargetInterval);
						continue;
					}
				}
			
				pathPending = true;
				unit.SetAgentDestination(dest, OnPathComplete);
			
				yield return Timing.WaitUntilFalse(() => pathPending);
			
				if ((endOfPath - dest).XZ().magnitude > 0.5f && target != null) {
					stuckUnits.Add(new StuckUnit(){unit = target, time = Time.time});
					target = null;
				} else {
					yield return Timing.WaitForSeconds(unit.config.refreshTargetInterval);
				}
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
			Timing.KillCoroutines(handle);
		}
		
		protected virtual Unit GetTarget(Unit currTarget, List<StuckUnit> excludeUnits) {
			if (currTarget != null && !currTarget.IsDead) {
				float d = Util.XZDistance(currTarget.transform.position, transform.position);
				if (d >= unit.config.attackMaxRadius && d < unit.config.chaseRadius) {
					return currTarget;
				}
			}
			
			var e = Util.GetNearest(transform.position,
				Util.GetLiveUnits(unit.GetEnemiesInChaseRange())
				.Where(u => !StuckUnitsContains(excludeUnits, u)));
			
			if (e != null) {
				return e;
			}
	
			return null;
		}
		
		private bool pathPending = false;
		
		private void OnPathComplete(Path path) {
			pathPending = false;
			endOfPath = path.vectorPath.Last();
		}
	}
}

