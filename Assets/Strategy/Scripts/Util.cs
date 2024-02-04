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
				return units.OrderBy(e => Util.XZDistance(e.NavBody.position, center))
					.FirstOrDefault();
			}
		
		public static Unit GetNearestLiveEnemy(int teamId,
			Vector3 pos,
			float minRadius,
			float maxRadius,
			UnitLayer layers) {
				return GetNearest(pos,
					GetLiveUnits(
						GetUnits(pos, minRadius, maxRadius, (teamId + 1) % 2, layers)));
			}
		
		public static Unit GetNearestEnemy(int teamId,
			Vector3 pos,
			float minRadius,
			float maxRadius,
			UnitLayer layers) {
				return Util.GetNearest(pos,
					GetUnits(pos,
					minRadius,
					maxRadius,
					(teamId + 1) % 2,
					layers));
			}
		
		public static IEnumerable<Unit> GetUnits(Vector3 center,
			float minRadius,
			float maxRadius,
			int teamId,
			UnitLayer layers = UnitLayer.ALL)
		{
			return GameController.Instance.GetTeam(teamId)
				.Where(e => {
					if ((e.config.unitLayer & layers) == 0) {
						return false;
					}
					float d = Util.XZDistance(e.NavBody.position, center);
					if (d < minRadius || d >= maxRadius) {
						return false;
					}
				return true;
			});
		}
		
		public static bool InRadiusXZ(Vector3 a, Vector3 b, float radius) {
			return InRadiusXZ(a, b, 0, radius);
		}
		
		public static bool InRadiusXZ(Vector3 a, Vector3 b, float minRadius, float maxRadius) {
			if (Mathf.Abs(a.x - b.x) >= maxRadius || Mathf.Abs(a.z - b.z) >= maxRadius) {
				return false;
			}
			float d = (a-b).XZ().magnitude;
			return d >= minRadius && d < maxRadius;
		}
		
		public static IEnumerable<Unit> GetUnitsInRange(Vector3 center,
			float radius,
			IEnumerable<Unit> units) {
				return units.Where(e => InRadiusXZ(center, e.NavBody.position, radius));
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
		
		public static IEnumerator<float> AlignAgentRotation(Transform body,
			Func<Vector3> velocityProvider,
			float lerpFactor) {
			while (true) {
				var vel = velocityProvider();
				if (vel.magnitude > 0) {
					vel.y = 0;
					var forward = body.forward;
					forward.y = 0;
					var dir = Vector3.Lerp(forward.normalized, vel.normalized, lerpFactor);
					body.LookAt(body.position + dir, Vector3.up);
				}
				yield return Timing.WaitForOneFrame;
			}
		}
		
		public static IEnumerator<float> AlignAgentPosition(Transform body,
			Func<Vector3> positionProvider, float lerpFactor) {
				while (true) {
					var pos = positionProvider();
					body.position = Vector3.Lerp(body.position, pos, lerpFactor);
					yield return Timing.WaitForOneFrame;
				}
			}
		
		public static float GetLookAtAngle(Transform self, Transform target) {
			var lookv = target.transform.position - self.position;
			lookv.y = 0;
			var forward = self.forward;
			forward.y = 0;
			lookv.Normalize();
			forward.Normalize();
			return Vector3.Angle(forward, lookv);
		}
		
		public static IEnumerator<float> LookAtTarget(Transform self,
			Transform target,
			float angleThreshold,
			float lerpFactor) {
			while (true) {
				var lookv = target.position - self.position;
				lookv.y = 0;
				var forward = self.forward;
				forward.y = 0;
				lookv.Normalize();
				forward.Normalize();
				float angle = Vector3.Angle(forward, lookv);
				if (angle < angleThreshold)  {
					break;
				}
				
				lookv = Vector3.Lerp(forward, lookv, lerpFactor);
				
				self.LookAt(self.position + lookv, Vector3.up);
				yield return Timing.WaitForOneFrame;
			}
		}
		
		public static void Foreach<T>(this IEnumerable<T> array, Action<T> action) {
			foreach (var e in array) {
				action(e);
			}
		}
		
		public static T RandomElement<T>(List<T> array) {
			if (array.Count == 0) {
				return default(T);
			}
			int n = UnityEngine.Random.Range(0, array.Count);
			return array[n];
		}
	}
}

