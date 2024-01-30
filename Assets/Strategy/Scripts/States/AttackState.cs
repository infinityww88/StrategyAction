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
		private BaseAttack[] attackBehaviors;
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Awake()
		{
			base.Awake();
			attackBehaviors = unit.Body.GetComponentsInChildren<BaseAttack>();
		}
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			unit.StopAgent();
			foreach (var a in attackBehaviors) {
				a.StartAttack();
			}
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			foreach (var a in attackBehaviors) {
				a.StopAttack();
			}
		}
	}

}
