using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Strategy {
	
	public class UnitState : MonoBehaviour
	{
		protected Unit unit;
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			unit = GetComponent<Unit>();
		}
	}
	
}

  