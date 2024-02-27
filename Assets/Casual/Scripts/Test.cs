using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace ModelMatch {
	public class Test : MonoBehaviour
	{
		private VisualElement root;
		private VisualElement element;
		public string elementName;
		public Camera camera;
		public float worldY;
		public Transform card;
		
		// Start is called before the first frame update
		void Start()
		{
			root = FindObjectOfType<UIDocument>().rootVisualElement;
			element = root.Q(elementName);
			Invoke("Init", 0.1f);
		}
		
		[Button]
		void Init() {
			root = FindObjectOfType<UIDocument>().rootVisualElement;
			element = root.Q(elementName);
			/*
			Vector4 area = Utils.GetWorldAreaYByUI(camera, element, card.position.y);
			float size = area.y - area.x;
			card.localScale = new Vector3(size/2, size/2, 1);
			var pos = new Vector3((area.x + area.y) / 2, card.position.y, (area.z + area.w) / 2);
			card.position = pos;
			*/
						
			Vector3 lb, lt, rb, rt;
			Utils.GetWorldAreaByUI(camera, element, card.position, out lb, out lt, out rt, out rb);
			Vector3 pos = (lb + rt) / 2;
			float size = rb.x - lb.x;
			card.position = pos;
			card.localScale = Vector3.one * size;
			var rot = Quaternion.FromToRotation(card.up, -camera.transform.forward);
			card.rotation = rot * card.rotation;
		}

		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			/*
			if (element == null || camera == null || worldPos == null) {
				return;
			}
			Vector3 lt, lb, rt, rb;
			Utils.GetWorldAreaByUI(camera, element, worldPos.position, out lt, out lb, out rt, out rb);
			Gizmos.DrawSphere(lt, 0.2f);
			Gizmos.DrawSphere(lb, 0.2f);
			Gizmos.DrawSphere(rt, 0.2f);
			Gizmos.DrawSphere(rb, 0.2f);
			*/
		}
	}
}

