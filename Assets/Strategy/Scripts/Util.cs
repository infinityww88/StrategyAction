using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;
using ProjectDawn.Navigation.Hybrid;
using MEC;
using System;

namespace Strategy {
	
	public static class Util
	{
		public static int EnemyTeamId(int myTeamId) {
			return (myTeamId + 1) % 2;
		}
		
		public static IEnumerable<Unit> GetLiveUnits(IEnumerable<Unit> units) {
			return units.Where(e => !e.IsDead);
		}
		
		public static Unit GetNearest(Vector3 center,
			IEnumerable<Unit> units) {
				return units.OrderBy(e => (e.transform.position - center).XZ().magnitude)
					.FirstOrDefault();
			}
		
		public static Unit GetNearestEnemy(int teamId,
			Vector3 pos,
			float minRadius,
			float maxRadius) {
				return Util.GetNearest(pos,
					GetUnits(pos,
					minRadius,
					maxRadius,
					(teamId + 1) % 2));
			}
		
		public static IEnumerable<Unit> GetUnits(Vector3 center,
			float minRadius,
			float maxRadius,
			int teamId)
		{
			return GameController.Instance.GetTeam(teamId)
				.Where(e => {
					float d = (e.transform.position - center).XZ().magnitude;
					if (d < minRadius || d >= maxRadius) {
						return false;
					}
				return true;
			});
		}
		
		public static string UnitsInfo(IEnumerable<Unit> units) {
			string info = "";
			foreach (var u in units) {
				info += u.gameObject.name + ", ";
			}
			return info;
		}
		
		public static Vector2 XZ(this Vector3 self) {
			return new Vector2(self.x, self.z);
		}
		
		public static float XZDistance(Vector3 a, Vector3 b) {
			return (a - b).XZ().magnitude;
		}
		
		public static void DrawPathGizmos(List<Vector3> path, Color color) {
			Gizmos.color = color;
			for (int i = 0; i < path.Count - 1; i++) {
				Gizmos.DrawLine(path[i], path[i+1]);
			}
			
			for (int i = 0; i < path.Count; i++) {
				if (i == 0) {
					Gizmos.color = Color.green;
				} else if (i == path.Count - 1) {
					Gizmos.color = Color.blue;
				} else {
					Gizmos.color = Color.red;
				}
				Gizmos.DrawSphere(path[i], 0.3f);
			}
		}
		
		public static IEnumerator<float> AlignAgentRotation(Transform body, Func<Vector3> velocityProvider) {
			while (true) {
				var vel = velocityProvider();
				vel.y = 0;
				var forward = body.forward;
				forward.y = 0;
				var dir = Vector3.Lerp(forward, vel, 0.01f);
				body.LookAt(body.position + dir, Vector3.up);
				yield return Timing.WaitForOneFrame;
			}
			
		}
	}
}

