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
			unit.EnableAgent(false);
			if (unit.deadClip != null) {
				animancer.Play(unit.deadClip);
			}
		}
	
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			
		}
	}
	
}

