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
		public GameObject unitPrefab;
		
		[Button]
		private void Move() {
			unit.SetMoveTarget(target.position);
		}
		
		[Button]
		private void Spawn() {
			var unit = Instantiate(unitPrefab, target.position, target.rotation);
			GameController.Instance.AddUnit(unit.GetComponent<Unit>());
		}
	}
}

