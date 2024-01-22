using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Assertions;

namespace Strategy {
	
	public class ChaseState : UnitState
	{
		private float lastTime;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.config.moveClip != null) {
				unit.animancer.Play(unit.config.moveClip);
			}
			lastTime = Time.time;
			unit.AgentWake();
			unit.SetAgentDestination(GetDestination());
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
		}
		
		protected virtual Vector3 GetDestination() {
			var e = Util.GetNearest(transform.position,
				Util.GetLiveUnits(unit.GetEnemiesInChaseRange()));
			
			if (e != null) {
				return e.transform.position;
			}
			
			Assert.IsTrue(unit.TeamId == 1, "no enemies for own team to chase");
			
			return GameController.Instance.BaseTower.position;
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			if (Time.time - lastTime >= unit.config.refreshTargetInterval) {
				unit.SetAgentDestination(GetDestination());
				lastTime = Time.time;
			}
		}
	}
}

