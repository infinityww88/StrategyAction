using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ModelMatch {
	
	public class LuaVM
	{
		private static LuaVM m_instance = null;
		
		public static LuaVM Instance {
			get {
				if (m_instance == null) {
					m_instance = new LuaVM();
				}
				return m_instance;
			}
		}
		
		private LuaEnv luaEnv;
		
		public LuaEnv Env => luaEnv;
		
		public LuaVM() {
			luaEnv = new LuaEnv();
		}
	}

}
