using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
				return units.OrderBy(e => (e.transform.position - center).magnitude)
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
					float d = (e.transform.position - center).magnitude;
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
	}
}

