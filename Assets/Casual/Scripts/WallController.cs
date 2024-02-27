using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace ModelMatch {
	
	public class WallController : MonoBehaviour
	{
		public Transform leftWall;
		public Transform topWall;
		public Transform rightWall;
		public Transform bottomWall;
		
		public Transform leftAnchor;
		public Transform topAnchor;
		public Transform rightAnchor;
		public Transform bottomAnchor;
		
		private void AlignWall(Transform wall, Vector3 pos) {
			pos.y = wall.position.y;
			wall.position = pos;
		}
		
		[Button]
		public void AlignWalls() {
			var area = GetPlayArea();
			float l = area.x, r = area.y, t = area.z, b = area.w;
			AlignWall(leftWall, new Vector3(l, 0, (t + b) / 2));
			AlignWall(rightWall,new Vector3(r, 0, (t + b) / 2));
			
			AlignWall(topWall, new Vector3((l + r) / 2, 0, t));
			AlignWall(bottomWall, new Vector3((l + r) / 2, 0, b));
		}
		
		// return left, right, top, bottom
		public Vector4 GetPlayArea() {
			var doc = FindObjectOfType<UIDocument>();
			var root = doc.rootVisualElement;
			var vp = root.Q<VisualElement>("GamePlayViewPort");
			var rect = Utils.GetViewRectByUI(vp);
			return Utils.GetWorldAreaYByUI(Camera.main, vp, 0);
		}
	}

}
