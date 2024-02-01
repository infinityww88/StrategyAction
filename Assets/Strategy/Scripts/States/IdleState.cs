using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Strategy {
	
	public class IdleState : UnitState
	{

		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.StopAgent();
			if (unit.idleClip != null) {
				animancer.Play(unit.idleClip);
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			animancer.Stop();
		}
	}
}
