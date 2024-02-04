using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

public class TestAnimation : MonoBehaviour
{
	private AnimancerComponent animancer;
	public AnimationClip clip;
	private AnimancerState state;
	
    // Start is called before the first frame update
    void Start()
    {
	    animancer = GetComponent<AnimancerComponent>();
    }
    
	[Button]
	void Play() {
		animancer.Playable.UnpauseGraph();
		state = animancer.States.GetOrCreate(clip);
		state.NormalizedTime = 0;
		animancer.Play(state);
	}
	
	[Button]
	void Stop() {
		animancer.Playable.PauseGraph();
	}
    
	void Attack() {
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
