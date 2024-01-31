using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace Strategy {
	
	public class GunController : MonoBehaviour
	{
		private ParticleSystem[] muzzles;
		
		// Start is called before the first frame update
		void Awake()
		{
			muzzles = GetComponentsInChildren<ParticleSystem>();
		}

		[Button]
		public void Fire() {
			muzzles.Foreach(p => {
				p.Emit(1);	
			});
		}
	}
}
