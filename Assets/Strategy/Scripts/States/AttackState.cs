using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Animancer;
using System.Linq;

namespace Strategy {
	
	public class AttackState : UnitState
	{
		private Unit target;
		private AnimancerState currState;
		
		public Unit Target => target;
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			Assert.IsTrue(Util.GetLiveUnits(unit.GetEnemiesInAttackRange()).Count() > 0, "No enemies in attack state");
			unit.AgentStop();
			unit.CutNavmesh();
			GetTarget();
			if (target != null) {
				LookAtTarget();
			}
			if (unit.config.attackClip != null) {
				currState = unit.animancer.Play(unit.config.attackClip);
			}
		}
		
		public void Attack() {
			
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			if (unit.config.attackClip != null) {
				unit.animancer.Stop();	
			}
		}
		
		private void GetTarget() {
			if (target == null
				|| target.IsDead 
				|| (target.transform.position - transform.position).magnitude > unit.config.attackMaxRadius) {
				if (unit.TeamId == 0) {
					
				}
				var targets = Util.GetLiveUnits(unit.GetEnemiesInAttackRange());
				if (targets.Count() == 0) {
					return;
				}
				target = Util.GetNearest(transform.position, targets.Where(e => !e.IsDead));
				if (target == null) {
					return;
				}
			}
		}
		
		public bool PeriodEnd { get; set; }
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			unit.animancer.States.Current.Speed = unit.attackSpeed;
			if (currState.NormalizedTime < 1) {
				PeriodEnd = false;
				return;
			}
			
			PeriodEnd = true;
			
			GetTarget();
			
			if (target != null) {
				LookAtTarget();
			}
			
			//if (Vector3.Angle(transform.forward, target.transform.forward) > 15) {
			currState.NormalizedTime = 0;
			//}
		}
		
		private void LookAtTarget() {
			var v = target.transform.position - transform.position;
			v.y = 0;
			transform.LookAt(transform.position + v, Vector3.up);
		}
	}

}
