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
			unit.StopAgent();
			if (unit.idleClip) {
				animancer.Play(unit.idleClip);
			}
		}

		// Update is called once per frame
		void OnDisable()
		{
			animancer.Stop();
		}
    
		public bool IsFreezing {
			get {
				return Time.time - startTime < FreezeTime;
			}
		}
	}

}
