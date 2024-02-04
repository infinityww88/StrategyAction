using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System.Threading;
using System;

public class TestCoro : MonoBehaviour
{
	private CoroutineHandle handle;
    // Start is called before the first frame update
    void Start()
    {
	   
    }
    
	//CancellationTokenSource token;
	
	[Button]
	void Test_Start() {
		handle = Timing.RunCoroutine(Coro1().CancelWith(gameObject));
		//token = new CancellationTokenSource();
		//var a = AsyncFunc1(token);
		//Debug.Log(a);
	}
	
	[Button]
	void Test_End() {
		//token.Cancel();
		Timing.KillCoroutines(handle);
		Debug.Log($"Kill coro1 {handle.IsValid} {handle.IsRunning}");
	}
	
	async Task AsyncFunc1(CancellationTokenSource token) {
		while (!token.IsCancellationRequested) {
			Debug.Log($"async func1 start");
			await AsyncFunc2(token);
			Debug.Log($"async func1 end");
		}
	}
	
	async Task AsyncFunc2(CancellationTokenSource token) {
		while (!token.IsCancellationRequested) {
			Debug.Log($"async func2 {Time.frameCount}");
			await Awaitable.WaitForSecondsAsync(1);
		}
	}
    
	IEnumerator<float> Coro1() {
		while (true) {
			Debug.Log("Coro1 start");
			yield return Timing.WaitUntilDone(Coro2(60 * 3).KillWith(Timing.CurrentCoroutine));
			Debug.Log("Coro1 end");
		}
	}
	
	IEnumerator<float> Coro2(int secs) {
		for (int i = 0; i < secs; i++) {
			Debug.Log($"Coro2 {i}");
			yield return Timing.WaitForSeconds(0.5f);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
