using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CanvasInfo : MonoBehaviour
{
	public Transform target;
	
	public Transform leftBottom;
	public Transform leftTop;
	public Transform rightTop;
	public Transform rightBottom;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected IEnumerator Start()
	{
		yield return 0;
		Info();
	}
    
	[Button]
	void Info() {
		var pos = (leftBottom.position + rightTop.position) / 2;
		target.position = pos;
		var width = (rightBottom.position - leftBottom.position).magnitude;
		var height = (leftTop.position - leftBottom.position).magnitude;
		target.localScale = new Vector3(width, height, 1);
		target.rotation = transform.rotation;
	}

    // Update is called once per frame
    void Update()
    {
	    Info();
    }
}
