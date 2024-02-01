using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System.Linq;

namespace Strategy {
	
	public class InputController : MonoBehaviour
	{
		public enum EMode {
			None,
			Select,
			MoveOrder,
			PutUnit
		}
		
		public UIDocument uiDoc;
		private VisualElement root;
		private VisualElement selectRect;
		private bool mouseDown = false;
		private Vector2 mouseStartPos = Vector2.zero;
		
		private HashSet<Unit> selectedUnit = new HashSet<Unit>();
		
		public List<GameObject> unitPrefabs;
		private InputAction click = new InputAction(binding: "<Mouse>/rightButton");
		
		// Start is called before the first frame update
		void Start()
		{
			root = uiDoc.rootVisualElement.Q("Root");
			selectRect = root.Q("SelectRect");
			click.performed += RightButtonClick;
		}
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			click.Enable();
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			click.Disable();
		}
		
		void RightButtonClick(InputAction.CallbackContext ctx) {
			int teamId = Keyboard.current.ctrlKey.isPressed ? 1 : 0;
			Vector2 pos = Mouse.current.position.value;
			PutUnit(pos, teamId);
		}
		
		void SelectUnit(Rect rect) {
			GameController.Instance.GetTeam(0).Foreach(e => {
				if (e.IsDead) {
					return;
				}
				Vector2 p = Camera.main.WorldToScreenPoint(e.Body.position);
				if (rect.Contains(p)) {
					e.ShowSelectCircle(true);
					selectedUnit.Add(e);
				}
			});
		}
		
		void MouseUpdate() {
			Mouse mouse = Mouse.current;
			if (mouse.leftButton.isPressed) {
				if (!mouseDown) {
					mouseDown = true;
					mouseStartPos = mouse.position.value;
					selectedUnit.Foreach(u => {
						u.ShowSelectCircle(false);	
					});
					selectedUnit.Clear();
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
					selectRect.style.display = DisplayStyle.Flex;
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
		
		private bool GetGroundPos(Vector3 screenPos, out Vector3 groundPos) {
			var ray = Camera.main.ScreenPointToRay(screenPos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
				groundPos = hit.point;
				groundPos.y = 0;
				return true;
			}
			groundPos = Vector3.zero;
			return false;
		}
		
		private void SelectUpdate() {
			if (selectedUnit.Count == 0) {
				return;
			}
			var mouse = Mouse.current;
			if (mouse.rightButton.isPressed) {
				Vector3 pos;
				if (GetGroundPos(mouse.position.value, out pos)) {
					Vector3 t = Vector3.zero;
					selectedUnit.Select(u => u.NavBody.position).Foreach(u => {
						t += u;
					});
					t /= selectedUnit.Count;
					var d = pos - t;
					selectedUnit.Foreach(u => {
						u.SetMoveTarget(u.NavBody.position + d);
					});
				}
			}
		}
		
		void PutUnit(Vector2 screenPos, int teamId) {
			Vector3 groundPos;
			if (GetGroundPos(screenPos, out groundPos)) {
				var p = Util.RandomElement<GameObject>(unitPrefabs);
				var o = Instantiate(p, groundPos, Quaternion.identity);
				Unit unit = o.GetComponent<Unit>();
				unit.SetTeamId(teamId);
				GameController.Instance.AddUnit(unit);
			}
		}
		
		// Update is called every frame, if the MonoBehaviour is enabled.
		protected void Update()
		{
			MouseUpdate();
			SelectUpdate();
			//PutUnitUpdate();
		}
	}
	
}
