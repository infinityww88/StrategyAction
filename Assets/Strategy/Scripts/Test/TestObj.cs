using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour
{
	public Vector3 size;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
	// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
	protected void OnDrawGizmos()
	{
		DebugExtension.DrawBounds(new Bounds(transform.position, size), Color.green);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
