using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class GunAttack : FireAttack
	{
		private GunController gunController;
		public float attackSpeed;
		
		// Awake is called when the script instance is being loaded.
		protected new void Awake()
		{
			base.Awake();
			gunController = GetComponentInChildren<GunController>();
		}
		
		protected override void Attack(Unit target) {
			if (unit.Debug) {
				Debug.Log($"{gameObject.name} Fire");
			}
			gunController.Fire();
		}
	}

}
