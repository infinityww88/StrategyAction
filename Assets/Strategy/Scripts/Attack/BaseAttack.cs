using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class BaseAttack : MonoBehaviour
	{
		protected Unit unit;
		public float attackMinRadius;
		public float attackMaxRadius;
		
		public virtual bool HasTarget() {
			return false;
		}
		
		public virtual void ScanTarget() {
			
		}
		
		protected bool TargetIsValid(Unit target) {
			return target != null 
				&& !target.IsDead 
				&& Util.InRadiusXZ(transform.position, target.NavBody.position, attackMinRadius, attackMaxRadius);
		}
		
		// Start is called before the first frame update
		protected void Awake()
		{
			unit = GetComponentInParent<Unit>();
		}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmosSelected()
		{
			DebugExtension.DrawCircle(transform.position, Vector3.up, Color.HSVToRGB(0, 0.7f, 1), attackMinRadius);
			DebugExtension.DrawCircle(transform.position, Vector3.up, Color.HSVToRGB(0, 1, 1), attackMaxRadius);
		}
		
		public virtual void StartAttack() {
			
		}
		
		public virtual void StopAttack() {
			
		}
	}

}
