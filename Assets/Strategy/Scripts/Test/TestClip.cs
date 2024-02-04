using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class TestClip : MonoBehaviour
{
	[SerializeField]
	[InlineEditor]
	private Strategy.UnitConfig config;
	
	[InlineEditor]
	public TestObj testObj;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
	protected void OnDrawGizmos()
	{
		DebugExtension.DrawCircle(transform.position, Vector3.up, Color.blue, config.attackPoint);
	}
}
