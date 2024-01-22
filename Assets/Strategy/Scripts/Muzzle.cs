using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Strategy {
	
	public class Muzzle : MonoBehaviour
	{
		private MeshRenderer renderer;
		private Material material;
		private Tween tween;
		
		public float lifeTime = 0.5f;
		
		// Start is called before the first frame update
		void Start()
		{
			renderer = GetComponent<MeshRenderer>();
			material = renderer.material;
		}
		
		[Button]
		public void MuzzleFire() {
			if (tween != null) {
				tween.Kill();
			}
			var c = material.color;
			c.a = 1;
			material.color = c;
			tween = material.DOFade(0, lifeTime).SetTarget(gameObject);
		}
	}
}

