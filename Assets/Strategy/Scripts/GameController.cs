using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Strategy {
	
	public class GameController : MonoBehaviour
	{
		public static GameController Instance { get; set; }
		
		public float bottomLine = 100f;
		
		public Transform baseTower;
	
		public enum EState {
			Prepare,
			Run,
			GameOver
		}
	
		public EState state = default(EState);
	
		public bool GameOver {
			get {
				return state == EState.GameOver;
			}
		}
		
		public Transform BaseTower => baseTower;
	
		private HashSet<Unit> team0Units = new HashSet<Unit>();
		private HashSet<Unit> team1Units = new HashSet<Unit>();
		
		public IEnumerable<Unit> GetTeam(int teamId) {
			return teamId == 0 ? team0Units : team1Units;
			
		}
		
		public IEnumerable<Unit> GetEnemyTeam(int teamId) {
			return teamId == 1 ? team0Units : team1Units;
		}
		
		public void AddUnit(Unit unit) {
			if (unit.TeamId == 0) {
				team0Units.Add(unit);
			} else {
				team1Units.Add(unit);
			}
		}
		
		// Awake is called when the script instance is being loaded.
		protected void Awake()
		{
			if (Instance != null) {
				Destroy(gameObject);
			}
			else {
				Instance = this;
			}
		}
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			var units = FindObjectsOfType<Unit>();
			foreach (var u in units) {
				if (u.TeamId == 0) {
					team0Units.Add(u);
				} else {
					team1Units.Add(u);
				}
			}
		}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(new Vector3(-200, 1, bottomLine), new Vector3(200, 1, bottomLine));
		}
	}
}
