using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRaycast : MonoBehaviour
{
	public LayerMask layerMask;
	private bool pressed = false;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if (Mouse.current.leftButton.isPressed) {
	    	if (pressed) {
	    		return;
	    	}
	    	pressed = true;
	    	Vector3 pos = Mouse.current.position.value;
	    	Ray ray = Camera.main.ScreenPointToRay(pos);
	    	RaycastHit[] hitInfos = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
	    	Debug.Log("hit start");
	    	foreach (var hit in hitInfos) {
	    		string name = hit.collider.gameObject.name;
	    		Debug.Log($"hit name {name}");
	    	}
	    }
	    else {
	    	pressed = false;
	    }
    }
}
