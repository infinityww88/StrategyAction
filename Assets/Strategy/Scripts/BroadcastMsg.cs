using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
	
	public class BroadcastMsg : MonoBehaviour
	{
		public GameObject msgTarget;
		
		public void Broadcast(string msg) {
			msgTarget.BroadcastMessage(msg);
		}
		
		public void SendMessageUp(string msg) {
			msgTarget.SendMessage(msg);
		}
		
		public void SendMessage(string msg) {
			msgTarget.SendMessage(msg);
		}
	}

}
