using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Strategy {
	
	public class InputController : MonoBehaviour
	{
		public UIDocument uiDoc;
		private VisualElement root;
		private VisualElement selectRect;
		private bool mouseDown = false;
		private Vector2 mouseStartPos = Vector2.zero;

		// Start is called before the first frame update
		void Start()
		{
			root = uiDoc.rootVisualElement.Q("Root");
			selectRect = root.Q("SelectRect");
			Debug.Log(root.name);
		}
		
		void SelectUnit(Rect rect) {
			Unit[] units = FindObjectsOfType<Unit>();
			foreach (var u in units) {
				Vector2 p = Camera.main.WorldToScreenPoint(u.transform.position);
				if (rect.Contains(p)) {
					Debug.Log($"select {u.gameObject.name}");
				}
			}
		}
		
		void MouseUpdate() {
			Mouse mouse = Mouse.current;
			if (mouse.leftButton.isPressed) {
				if (!mouseDown) {
					mouseDown = true;
					mouseStartPos = mouse.position.value;
					selectRect.style.display = DisplayStyle.Flex;
				} else {
					Vector2 currPos = mouse.position.value;
					float left = Mathf.Min(currPos.x, mouseStartPos.x);
					float right = Mathf.Max(currPos.x, mouseStartPos.x);
					float top = Mathf.Max(currPos.y, mouseStartPos.y);
					float bottom = Mathf.Min(currPos.y, mouseStartPos.y);
					selectRect.style.left = left;
					selectRect.style.bottom = bottom;
					selectRect.style.width = right - left;
					selectRect.style.height = top - bottom;
					SelectUnit(new Rect(left, bottom, right - left, top - bottom));
				}
			} else {
				if (mouseDown) {
					// Cancel drag
					mouseDown = false;
					selectRect.style.display = DisplayStyle.None;
					selectRect.style.width = 0;
					selectRect.style.height = 0;
				}
			}
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			MouseUpdate();
		}
	}
	
}
