using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class FreezeState : UnitState
	{
		public float FreezeTime { get; set; }
	
		private float startTime;
	
		// Start is called before the first frame update
		void OnEnable()
		{
			startTime = Time.time;
			if (unit.config.idleClip) {
				unit.animancer.Play(unit.config.idleClip);
			}
		}

		// Update is called once per frame
		void OnDisable()
		{
			unit.animancer.Stop();
		}
    
		public bool IsFreezing {
			get {
				return Time.time - startTime < FreezeTime;
			}
		}
	}

}
