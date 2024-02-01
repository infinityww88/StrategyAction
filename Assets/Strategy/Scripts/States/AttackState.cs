using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Animancer;
using System.Linq;
using MEC;

namespace Strategy {
	
	public class AttackState : UnitState
	{
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.StopAgent();
			foreach (var a in unit.AttackBehaviors) {
				a.StartAttack();
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			foreach (var a in unit.AttackBehaviors) {
				a.StopAttack();
			}
		}
	}

}
