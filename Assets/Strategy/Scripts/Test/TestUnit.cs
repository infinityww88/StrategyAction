using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ProjectDawn.Navigation.Hybrid;
using Animancer;

namespace Strategy  {
	
	public class TestUnit : MonoBehaviour
	{
		public Unit unit;
		public Transform target;
		
		[Button]
		private void Move() {
			unit.SetMoveTarget(target.position);
		}
	}
}

