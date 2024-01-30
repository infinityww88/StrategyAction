using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation;

[CreateAssetMenu(menuName="Strategy/UnitConfig", fileName="UnitConfig", order=-1)]
public class UnitConfig : ScriptableObject
{
	public float maxHp;
	public float skillCD;
	public float attackPoint;
	
	public float chaseBeginRadius;	// enemies 进入这个半径开始追击
	public float chaseEndRadius;	// enemies 进入这个半径停止追击，开始攻击
	
	public float refreshTargetInterval;
	
	public int findEnemiesPolicy;
	
	public AnimationClip idleClip;
	public AnimationClip moveClip;
	//public AnimationClip attackClip;
	//public AnimationClip skillClip;
	public AnimationClip deadClip;
}
