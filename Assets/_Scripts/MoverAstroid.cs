using UnityEngine;
using System.Collections;

public class MoverAstroid : MonoBehaviour {

	public float speed;
	public Vector3 scaler;
	void Start () {
		GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		if (scaler != new Vector3 (1.0f, 1.0f, 1.0f))
			GetComponent<Transform> ().localScale = new Vector3(0.01f, 0.01f, 0.01f);
	}

	void Update () {
		Vector3 scale = GetComponent<Transform> ().localScale;
		if (scale.x < 1f ) {
			scale.Scale(scaler);
			GetComponent<Transform> ().localScale = scale;
		}
	}
}
