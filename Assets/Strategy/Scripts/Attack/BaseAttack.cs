﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace Strategy {
	
	public class BaseAttack : MonoBehaviour
	{
		protected Unit unit;
		protected Unit target;
		public float attackMinRadius;
		public float attackMaxRadius;
		protected CoroutineHandle attackCoroHandle;
		
		public bool debug = false;
		
		public string DebugInfo() {
			return $"target {target}, corohandle {attackCoroHandle.IsRunning}";
		}
		
		public virtual bool HasTarget() {
			return TargetIsValid(target);
		}
		
		public virtual void ScanTarget() {
			target = Util.GetNearestLiveEnemy(
				unit.TeamId,
				transform.position,
				attackMinRadius,
				attackMaxRadius,
				unit.config.attackLayers);
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
			if (!debug) {
				return;
			}
			DebugExtension.DrawCircle(transform.position, Vector3.up, Color.HSVToRGB(0, 0.7f, 1), attackMinRadius);
			DebugExtension.DrawCircle(transform.position, Vector3.up, Color.HSVToRGB(0, 1, 1), attackMaxRadius);
			if (TargetIsValid(target)) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(target.NavBody.position, 0.5f);
			}
		}
		
		public virtual void StartAttack() {
			attackCoroHandle = Timing.RunCoroutine(
				AttackCoro().CancelWith(gameObject));
		}
		
		public virtual void StopAttack() {
			Timing.KillCoroutines(attackCoroHandle);
		}
		
		protected virtual IEnumerator<float> AttackCoro() {
			yield break;
		}
	}

}
