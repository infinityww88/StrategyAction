using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using DG.Tweening;

namespace ModelMatch {
	
	public class Test : MonoBehaviour
	{
		public Transform target;
		public float duration = 10;
		private Tween tween = null;
		
		[Button]
		public void RunCode() {
			tween = transform.DOMove(target.position, duration).SetLoops(-1, LoopType.Yoyo);
		}
		
		[Button]
		public void Hold() {
			if (tween != null) {
				if (tween.timeScale == 0) {
					tween.timeScale = 1;
				} else {
					tween.timeScale = 0;
				}
			}
		}
	}
}

