using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class GunAttack : FireAttack
	{
		private GunController gunController;
		
		// Awake is called when the script instance is being loaded.
		protected new void Awake()
		{
			base.Awake();
			gunController = GetComponent<GunController>();
		}
		
		protected override void Attack(Unit target) {
			gunController.Fire();
		}
	}

}
