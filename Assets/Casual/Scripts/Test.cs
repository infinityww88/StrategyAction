using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using XLua;

namespace ModelMatch {
	
	public class Test : MonoBehaviour
	{
		public TextAsset luaCode;
		public string chunkName;
		public string goName;
		
		[Button]
		public void RunCode() {
			var o = GameObject.Find(goName);
			if (o != null) {
				LuaMonoBehaviour.BehaviourScript = luaCode.text;
				LuaMonoBehaviour.BehaviourScriptName = chunkName;
				o.AddComponent<LuaMonoBehaviour>();
				LuaMonoBehaviour.BehaviourScript = "";
				LuaMonoBehaviour.BehaviourScriptName = "";
			}
		}
		
		[Button]
		public void Info() {
			Debug.Log(LuaMonoBehaviour.BehaviourScript);
		}
	}
}

