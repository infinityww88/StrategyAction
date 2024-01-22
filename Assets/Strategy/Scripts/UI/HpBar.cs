using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceWar {
	
	public class HpBar : MonoBehaviour
	{
		public Transform target;
		public Vector3 offset;
		private Canvas canvas;
		private RectTransform rectTransform;
		
		// Start is called before the first frame update
		void Start()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
			rectTransform.position = screenPos + offset;
		}
	}

}
