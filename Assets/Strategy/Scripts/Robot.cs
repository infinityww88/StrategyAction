using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class Robot : MonoBehaviour
	{
		public Muzzle muzzle0;
		public Muzzle muzzle1;
		
		public void Attack() {
			muzzle0.MuzzleFire();
			muzzle1.MuzzleFire();
		}
	}
}

