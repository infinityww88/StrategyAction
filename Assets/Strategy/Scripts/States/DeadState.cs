using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectDawn.Navigation.Hybrid;

namespace Strategy {
	
	public class DeadState : UnitState
	{
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.StopAgent();
			unit.GetComponent<AgentAuthoring>().enabled = false;
			if (unit.config.deadClip != null) {
				unit.animancer.Play(unit.config.deadClip);
			}
		}
	
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			
		}
	}
	
}

