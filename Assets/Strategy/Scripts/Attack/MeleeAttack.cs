using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class MeleeAttack : BaseAttack
	{
		public override void ApplyAttack(Unit target) {
			if (target == null || target.IsDead) {
				return;
			}
			target.hp -= unit.config.attackPoint;
		}
	}
}

