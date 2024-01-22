using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ProjectDawn.Navigation.Hybrid;
using Animancer;

public class TestUnit : MonoBehaviour
{
	public AnimationClip clip;
	
	private AnimancerComponent animancer;
	
	private AnimancerState state;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		animancer = GetComponent<AnimancerComponent>();
		state = animancer.Play(clip);
		state.Events.Add(1f, () => {
			Debug.Log("-- end --");
		});
	}
	
	[Button]
	void Test() {
		state.NormalizedTime = 0;
	}
	
	void Attack() {
		Debug.Log("Attack");
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	 void Update()
	{
		if (state.NormalizedEndTime >= 1) {
			Debug.Log("animation end");
		}
	}
}
