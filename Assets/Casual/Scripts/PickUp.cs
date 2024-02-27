using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace ModelMatch {
	
	public class PickUp : MonoBehaviour
	{
		public LayerMask pickLayerMask;
		
		public InputAction touchAction;
		public InputAction touchUp;
		
		private Outline lastHighlight = null;
		
		// Start is called before the first frame update
		void Start()
		{
			touchAction.performed += OnTouch;
			touchUp.canceled += OnTouchCancel;
			touchUp.started += OnTouchStart;
		}
		
		public void OnTouch(InputAction.CallbackContext ctx) {
			PickUpModel(ctx.ReadValue<Vector2>());
		}
		
		public void OnTouchStart(InputAction.CallbackContext ctx) {
			PickUpModel(Touch.activeTouches[0].screenPosition);
		}
		
		public void OnTouchCancel(InputAction.CallbackContext ctx) {
			if (lastHighlight != null) {
				lastHighlight.enabled = false;
				GlobalManager.Instance.OnPickupComponent?.Invoke(lastHighlight.gameObject);
				lastHighlight = null;
			}
		}
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			touchAction.Enable();
			touchUp.Enable();
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			touchAction.Disable();
			touchUp.Disable();
		}
		
		void PickUpModel(Vector2 pos) {
			Ray ray = Camera.main.ScreenPointToRay(pos);
			if (!Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, pickLayerMask)) {
				return;
			}
			
			var go = hitInfo.collider.transform.gameObject;
			if (go == lastHighlight) {
				return;
			}
			Outline outline;
			if (!go.TryGetComponent<Outline>(out outline)) {
				outline = go.AddComponent<Outline>();
				outline.OutlineColor = Color.yellow;
				outline.OutlineWidth = 6;
				outline.enabled = false;
			}
			if (lastHighlight != null) {
				lastHighlight.enabled = false;
			}
			lastHighlight = outline;
			outline.enabled = true;
		}
	}

}
