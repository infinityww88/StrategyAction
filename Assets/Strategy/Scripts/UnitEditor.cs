using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ProjectDawn.Navigation.Hybrid;
using ProjectDawn.Navigation;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Strategy {

	public class UnitEditor : MonoBehaviour
	{
		#if UNITY_EDITOR
		
		#region Agent
		[BoxGroup("Agent"), OnInspectorInit("@agentSpeed = ReadSpeed()")]
		public float agentSpeed;
		[BoxGroup("Agent"), OnInspectorInit("@layers = ReadLayer()")]
		public UnitLayer layers;
		#endregion
		
		#region Cylinder Shape
		[BoxGroup("Agent Shape"), OnInspectorInit("@agentRadius = ReadRadius()")]
		public float agentRadius;
		[BoxGroup("Agent Shape"), OnInspectorInit("@agentHeight = ReadHeight()")]
		public float agentHeight; 
		#endregion
		
		[InlineEditor, OnInspectorInit("InitAttack")]
		public BaseAttack[] attack;
		
		void InitAttack() {
			attack = GetComponentsInChildren<BaseAttack>();
		}
		
		T ReadProperty<T> (SerializedObject so, Func<SerializedProperty, T> reader, string propertyName) {
			SerializedProperty sp = so.FindProperty(propertyName);
			return reader(sp);
		}
		
		float ReadFloatProperty(SerializedObject so, string propertyName) {
			return ReadProperty<float>(so, sp => sp.floatValue, propertyName);
		}
		
		int ReadIntProperty(SerializedObject so, string propertyName) {
			return ReadProperty<int>(so, sp => sp.intValue, propertyName);
		}
		
		float ReadSpeed() {
			float v = ReadFloatProperty(GetSO<AgentAuthoring>(), "Speed");
			Debug.Log($"read speed {v}");
			return v;
		}
		
		float ReadRadius() {
			return ReadFloatProperty(GetSO<AgentCylinderShapeAuthoring>(), "Radius");
		}
		
		float ReadHeight() {
			return ReadFloatProperty(GetSO<AgentCylinderShapeAuthoring>(), "Height");
		}
		
		UnitLayer ReadLayer() {
			NavigationLayers layer = (NavigationLayers)ReadIntProperty(GetSO<AgentAuthoring>(), "m_Layers");
			UnitLayer ret = UnitLayer.None;
			switch (layer) {
			case NavigationLayers.None:
				ret = UnitLayer.None;
				break;
			case NavigationLayers.Everything:
				ret = UnitLayer.ALL;
				break;
			case NavigationLayers.Default:
				ret = UnitLayer.Ground;
				break;
			case NavigationLayers.Layer1:
				ret = UnitLayer.Sky;
				break;
			}
			return ret;
		}
		
		void UpdateProperty<T>(UnityEngine.Object obj, Action<SerializedProperty, T> updateAction,
			params ValueTuple<string, T>[] properties) {
			SerializedObject so = new SerializedObject(obj);
			foreach (var p in properties) {
				SerializedProperty sp = so.FindProperty(p.Item1);
				//sp.floatValue = p.Item2;
				updateAction(sp, p.Item2);
			}
			so.ApplyModifiedProperties();
		}
		
		void UpdateFloat(UnityEngine.Object obj, params ValueTuple<string, float>[] properties) {
			UpdateProperty<float>(obj, (sp, v) => sp.floatValue = v, properties);
		}
		
		void UpdateInt(UnityEngine.Object obj, params ValueTuple<string, int>[] properties) {
			UpdateProperty<int>(obj, (sp, v) => sp.intValue = v, properties);
		}
		
		int GetNavLayers(UnitLayer layers) {
			NavigationLayers nl = NavigationLayers.Default;
			switch (layers) {
			case	UnitLayer.Ground:
				nl = NavigationLayers.Default;
				break;
			case UnitLayer.Sky:
				nl = NavigationLayers.Layer1;
				break;
			case UnitLayer.ALL:
				nl = NavigationLayers.Everything;
				break;
			default:
				nl =	NavigationLayers.None;
				break;
			}
			return (int)nl;
		}
		
		SerializedObject GetSO<T>() where T : Component {
			var agent = GetComponentInChildren<T>();
			return new SerializedObject(agent);
		}
		
		[Button]
		void ApplyAgentNavigation() {
			var agent = GetComponentInChildren<AgentAuthoring>();
			UpdateInt(agent, ValueTuple.Create("m_Layers", GetNavLayers(layers)));
			UpdateFloat(agent, ValueTuple.Create("Speed", agentSpeed));
			
			var agentClinder = GetComponentInChildren<AgentCylinderShapeAuthoring>();
			UpdateFloat(agentClinder,
				ValueTuple.Create("Radius", agentRadius),
				ValueTuple.Create("Height", agentHeight));
				
			var agentCollider = GetComponentInChildren<AgentColliderAuthoring>();
			UpdateInt(agentCollider, ValueTuple.Create("m_Layers", GetNavLayers(layers)));
			
			var agentSonarAvoid = GetComponentInChildren<AgentAvoidAuthoring>();
			
			UpdateInt(agentSonarAvoid, ValueTuple.Create("m_Layers", GetNavLayers(layers)));
			
			var agentSeparation = GetComponentInChildren<AgentSeparationAuthoring>();
			UpdateInt(agentSeparation, ValueTuple.Create("m_Layers", GetNavLayers(layers)));
		}
		
		// Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected.
		protected void OnDrawGizmosSelected()
		{
			DebugExtension.DrawCylinder(transform.position,
				transform.position + Vector3.up * agentHeight,
				Color.green, agentRadius);
		}
		
		#endif
	}
}

