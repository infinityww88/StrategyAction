using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class BroadcastMsg : MonoBehaviour
	{
		public void Broadcast(string msg) {
			gameObject.BroadcastMessage(msg);
		}
		
		public void SendMessageUp(string msg) {
			gameObject.SendMessageUpwards(msg);
		}
		
		public void SendMessage(string msg) {
			gameObject.SendMessage(msg);
		}
	}

}
