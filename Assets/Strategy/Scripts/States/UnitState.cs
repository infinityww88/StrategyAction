using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

namespace Strategy {
	
	public class UnitState : MonoBehaviour
	{
		protected Unit unit;
		
		protected AnimancerComponent animancer;
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			unit = GetComponent<Unit>();
			animancer = unit.Body.GetComponent<AnimancerComponent>();
		}
	}
	
}

  