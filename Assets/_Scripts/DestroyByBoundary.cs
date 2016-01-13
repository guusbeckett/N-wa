using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
	public GameController gameControler;
	public int missPenalty;
		void OnTriggerExit(Collider other) {
			if (other.tag == "Bolt")
			gameControler.scorePenalty (missPenalty);
		Destroy (other.gameObject);
	}
}
