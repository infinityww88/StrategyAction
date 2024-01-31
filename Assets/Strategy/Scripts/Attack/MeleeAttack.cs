using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using MEC;

namespace Strategy {
	
	public class MeleeAttack : BaseAttack
	{
		public AnimationClip attackClip;
		private AnimancerComponent animancer;
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected new void Start()
		{
			animancer = GetComponent<AnimancerComponent>();
		}

		public override void StopAttack() {
			base.StopAttack();
			unit.InAttackAnimation = false;
		}
		
		void Attack() {
			
		}
		
		void GetTarget(Unit target){
			if (target != null) {
				if (target.IsDead) {
					target = null;
				}
				else {
					float d = Util.XZDistance(target.transform.position,
						unit.NavBody.position);
					if (d >= attackMaxRadius) {
						target = null;
					}
				}
			}
			if (target == null) {
				ScanTarget();
			}
		}
		
		protected override IEnumerator<float> AttackCoro() {
			bool idle = false;
			AnimancerState idleState = null;
			AnimancerState attackState = null;
			unit.InAttackAnimation = false;
			
			while (true) {
				GetTarget(target);
				if (target == null) {
					if (idle == false) {
						idle = true;
						if (idleState == null) {
							attackState = null;
							idleState = animancer.Play(unit.config.idleClip);
						}
					}
					yield return Timing.WaitForOneFrame;
					continue;
				}
				idle = false;
				idleState = null;
				
				yield return Timing.WaitUntilDone(Util.LookAtTarget(transform, target.NavBody, 0.2f, 0.8f));
				
				unit.InAttackAnimation = true;
				if (attackState == null){
					attackState = animancer.Play(attackClip);
				}
				else {
					attackState.NormalizedTime = 0;
				}
				yield return Timing.WaitUntilTrue(() => attackState.NormalizedTime >= 1);
				unit.InAttackAnimation = false;
				
				yield return Timing.WaitForOneFrame;
			}
		}
	}
}

