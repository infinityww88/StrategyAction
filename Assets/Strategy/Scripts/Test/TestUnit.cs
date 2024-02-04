using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ProjectDawn.Navigation.Hybrid;
using Animancer;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Strategy  {
	
	public class TestUnit : MonoBehaviour
	{
		[OnValueChanged("OnAgentChange")]
		public float radius;
		
		public AgentCylinderShapeAuthoring agentShape;
		
		#if UNITY_EDITOR
		private void OnAgentChange() {
			SerializedObject so = new SerializedObject(agentShape);
			SerializedProperty r = so.FindProperty("Radius");
			r.floatValue = radius;
			so.ApplyModifiedProperties();
		}
		#endif
		
		void Attack() {
			
		}
	}
}

