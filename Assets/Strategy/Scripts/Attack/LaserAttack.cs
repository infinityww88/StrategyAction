using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class LaserAttack : BaseAttack
	{
		private LaserController laser;
		[SerializeField]
		private float readyCd = 1f;
		
		// Awake is called when the script instance is being loaded.
		protected new void Awake()
		{
			base.Awake();
			laser = GetComponentInChildren<LaserController>();
		}
		
		protected override IEnumerator<float> AttackCoro() {
			yield return Timing.WaitForSeconds(readyCd);
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
		
		public override void StopAttack()
		{
			base.StopAttack();
			laser.Close();
		}
	}
}

