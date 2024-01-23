﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class IdleState : UnitState
	{
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			if (unit.config.idleClip != null) {
				unit.animancer.Play(unit.config.idleClip);
			}
			unit.AgentStop();
			unit.CutNavmesh();
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			unit.animancer.Stop();
		}
	}
}
