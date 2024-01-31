using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class LaserAttack : BaseAttack
	{
		private LaserController laser;
		
		// Awake is called when the script instance is being loaded.
		protected new void Awake()
		{
			base.Awake();
			laser = GetComponentInChildren<LaserController>();
		}
		
		protected override IEnumerator<float> AttackCoro() {
			while (true) {
				if (!HasTarget()) {
					laser.Close();
					ScanTarget();
					yield return Timing.WaitForOneFrame;
					continue;
				}
				laser.Light(target.Body);
				yield return Timing.WaitForOneFrame;
			}
		}
	}
}

