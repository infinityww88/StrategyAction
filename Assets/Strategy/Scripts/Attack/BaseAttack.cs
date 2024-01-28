using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class BaseAttack : MonoBehaviour
	{
		protected Unit unit;
		
		// Start is called before the first frame update
		void Start()
		{
			unit = GetComponent<Unit>();
		}
		
		public virtual void ApplyAttack(Unit target) {
			
		}
	}

}
