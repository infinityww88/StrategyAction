using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArrow : MonoBehaviour
{
	public float speed = 5;
	public float destroyDelay = 0.2f;
	private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody>();
    }
    
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		Debug.Log($"Collide {other.name}");
		Destroy(other.gameObject, destroyDelay);
	}

    // Update is called once per frame
	void FixedUpdate()
    {
	    rb.MovePosition(transform.forward * speed * Time.fixedDeltaTime + transform.position);
    }
}
