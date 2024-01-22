using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackArrow : MonoBehaviour
{
	public float pretime = 1.5f;
	public float postTime = 0.5f;
	public string uv0;
	public string uv1;
	
	private Material mat;
	
	public float Length {
		get {
			return transform.localScale.z;
		}
		set {
			var scale = transform.localScale;
			scale.z = value;
			transform.localScale = scale;
		}
	}
	
    // Start is called before the first frame update
    void Start()
	{
		mat = GetComponent<MeshRenderer>().material;
	    var preTween = mat.DOFloat(0f, uv0, pretime);
		var postTween = mat.DOFloat(1f, uv1, postTime);
	    DOTween.Sequence()
		    .Append(preTween)
		    .Append(postTween)
		    .SetTarget(gameObject)
		    .OnRewind(() => {
		    	mat.SetFloat(uv0, 1f);
		    	mat.SetFloat(uv1, 0f);
		    })
		    .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
