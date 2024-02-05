using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation;

namespace Strategy {
	
	[CreateAssetMenu(menuName="Strategy/UnitConfig", fileName="UnitConfig", order=-1)]
	public class UnitConfig : ScriptableObject
	{
		public float maxHp = 100;
		public float attackPoint = 10;
		public float chaseMinRadius = 0;
		public float chaseMaxRadius = 20;
	
		public float stuckPosDelta = 1f;
		public float reachRadius = 0.2f;
		public float targetUpdateInterval = 0.5f;
		public float stuckMonitorInterval = 5f;
	
		public float refreshTargetInterval = 1;
	
		public float attackSpeed = 1;
		public UnitLayer attackLayers = UnitLayer.Ground;
		public UnitLayer unitLayer = UnitLayer.Ground;
	
		public AnimationClip idleClip;
		public AnimationClip moveClip;
		public AnimationClip attackClip;
		public AnimationClip deadClip;
	}
}

