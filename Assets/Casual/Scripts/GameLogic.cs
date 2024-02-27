using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ScriptableObjectArchitecture;
using System.Linq;
using MEC;

using Random = UnityEngine.Random;

namespace ModelMatch {
	
	public class GameLogic : MonoBehaviour
	{
		public WallController wallController;
		
		public Transform ComponentsRoot;
		public Transform TaskRoot;
		
		public Material solidMaterial;
		
		public float m_AreaMargin = 0.5f;
		
		public LevelData m_Level;
		
		private List<Task> tasks = new	List<Task>();
		private Task currTask = null;
		
		public int oneLayerNum = 10;
		public float heightStep = 1f;
		
		[Button]
		private void InitLevel() {
			wallController.AlignWalls();
			InitTask();
			InitComponents();
		}
		
		// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		protected void Start()
		{
			Timing.CallDelayed(0.1f, () => {
				InitLevel();
				NextTask();
			});
		}
	
		[Button]
		void Blow() {
			var compRigids = ComponentsRoot.GetComponentsInChildren<Rigidbody>();
			foreach (var body in compRigids) {
				var xz = UnityEngine.Random.insideUnitCircle;
				float mag = UnityEngine.Random.Range(3, 15);
				var angle = UnityEngine.Random.Range(0, 60);
				var rot = Quaternion.AngleAxis(angle, new Vector3(xz.x, 0, xz.y));
				var force = rot * Vector3.up;
				force = force.normalized * mag;
				body.AddForce(force, ForceMode.Impulse);
			}
		}
		
		// This function is called when the object becomes enabled and active.
		protected void OnEnable()
		{
			GlobalManager.Instance.OnPickupComponent += OnPickupComponent;
			GlobalManager.Instance.OnBlow += Blow;
		}
		
		// This function is called when the behaviour becomes disabled () or inactive.
		protected void OnDisable()
		{
			GlobalManager.Instance.OnPickupComponent -= OnPickupComponent;
			GlobalManager.Instance.OnBlow -= Blow;
		}
		
		void OnPickupComponent(GameObject o) {
			ComponentData comp = o.GetComponent<ComponentData>();
			Task currTask = tasks.First();
			if (currTask.ComponentAvailable(comp)) {
				StartCoroutine(TweenAssembleComponent(comp));
			}
		}
		
		IEnumerator TweenAssembleComponent(ComponentData comp) {
			yield return currTask.AssembleComponent(comp);
			if (currTask.Done()) {
				NextTask();
			}
		}
		
		[Button]
		private Task NextTask() {
			if (currTask != null) {
				Destroy(currTask.gameObject);
				tasks.RemoveAt(0);
			}
			if (tasks.Count == 0) {
				currTask = null;
				return currTask;
			}
			currTask = tasks.First();
			currTask.gameObject.SetActive(true);
			currTask.transform.localPosition = Vector3.zero;
			currTask.transform.localRotation = Quaternion.identity;
			currTask.transform.localScale = Vector3.one;
			currTask.transform.SetParent(TaskRoot, false);
			currTask.Begin();
			
			return currTask;
		}
		
		private void InitTask() {
			m_Level.tasks.ForEach(item => {
				var prefab = item.m_Model;
				for (int i = 0; i < item.m_Num; i++) {
					var taskObj = Instantiate(prefab, TaskRoot);
					tasks.Add(taskObj.AddComponent<Task>());
					taskObj.SetActive(false);
				}
			});
		}
		
		private void InitComponents() {
			List<GameObject> allComps = new List<GameObject>();
			
			m_Level.tasks.ForEach(item => {
				AddModelComponents(allComps, item.m_Model, item.m_Num);
			});
			m_Level.intersperses.ForEach(item => {
				AddModelComponents(allComps, item.m_Model, item.m_Num);
			});
			Spread(allComps);
		}
		
		private void AddModelComponents(List<GameObject> comps, GameObject prefab, int num) {
			for (int i = 0; i < num; i++) {
				var model = Instantiate(prefab);
				int n = model.transform.childCount;
				for (int j = 0; j < n; j++){
					var c = model.transform.GetChild(0);
					c.gameObject.AddComponent<Rigidbody>();
					var collider = c.gameObject.AddComponent<MeshCollider>();
					collider.convex = true;
					comps.Add(c.gameObject);
					c.transform.SetParent(ComponentsRoot);
				}
				Destroy(model);
			}
		}

		private void Spread(List<GameObject> allComps) {
			var posGenerator = SpreadPosGenerator(oneLayerNum, heightStep);
			allComps.ForEach(comp => {
				posGenerator.MoveNext();
				comp.transform.position = posGenerator.Current;
				comp.transform.rotation = Quaternion.Euler(
					Random.Range(0, 360f),
					Random.Range(0, 360f),
					Random.Range(0, 360f));
			});
		}
		
		IEnumerator<Vector3> SpreadPosGenerator(int oneLayerNum, float heightStep) {
			Vector4 area = wallController.GetPlayArea();
			float y = 0;
			while (true) {
				for (int j = 0; j < oneLayerNum; j++) {
					float x = Random.Range(area.x, area.y);
					float z = Random.Range(area.w, area.z);
					Vector3 pos = new Vector3(x, y, z) + transform.position;
					yield return pos;
				}
				y += heightStep;
			}
		}
		
		// Implement OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn.
		protected void OnDrawGizmos()
		{
			if (wallController != null) {
				Vector4 area = wallController.GetPlayArea();
				float l = area.x + m_AreaMargin,
					r = area.y - m_AreaMargin,
					t = area.z - m_AreaMargin,
					b = area.w + m_AreaMargin;
				Vector3 pos = new Vector3((l + r) / 2, transform.position.y, (t + b) / 2);
				DebugExtension.DrawBounds(new Bounds(pos,
					new Vector3(Mathf.Abs(r - l), 0.01f, Mathf.Abs(t - b))));
			}
			
			if (Utils.GetCameraZPlane(Camera.main,
				new Rect(0, 0, 1, 1),
				new Plane(Vector3.up, Vector3.zero),
				out Vector3 lb,
				out Vector3 lt,
				out Vector3 rt,
				out Vector3 rb)) {
				float l = lb.x, r = rt.x, t = rt.z, b = lb.z;
				DebugExtension.DrawBounds(new Bounds(new Vector3((l + r) / 2, 0, (t + b) / 2),
					new Vector3(Mathf.Abs(r - l), 0.01f, Mathf.Abs(t - b))), Color.red);
				}
		}
	}
}

