using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class DeadState : UnitState
	{
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.config.deadClip != null) {
				unit.animancer.Play(unit.config.deadClip);
			}
			unit.AgentStop();
			unit.RestoreNavMesh();
		}
	
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			
		}
	}
	
}

