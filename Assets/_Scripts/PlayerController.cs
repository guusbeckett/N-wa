﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	
	public float fireRate;
	private float nextFire = 0.5f;
	private float moveSpeed = 1;
	public bool userInputEnabled = true;
	public bool moveForwardsAutomatically = false;
	public bool ableToFire = true;
	public VerticalMovementState verticalMovementState = VerticalMovementState.forwardsBackwards;
	void Update () {
		if (Input.GetButton ("Fire1") && Time.time > nextFire && userInputEnabled  && ableToFire) {
			Instantiate (shot, shotSpawn.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
			nextFire = Time.time + fireRate;
			GetComponent<AudioSource>().Play ();
		}
		if (Input.GetButton ("Fire3") && userInputEnabled)
			moveSpeed = 0.5f;
		else
			moveSpeed = 1f;
	}

	void FixedUpdate ()
	{
		float moveHorizontal = 0.0f;
		float moveVertical = 0.0f;
		if (userInputEnabled) {
			moveHorizontal = Input.GetAxis ("Horizontal");
			moveVertical = Input.GetAxis ("Vertical");
		} else if (moveForwardsAutomatically) {
			moveVertical = 0.3f;
		}
		Vector3 movement;
			
			switch (verticalMovementState) {
				case VerticalMovementState.forwardsBackwards:
					movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
					break;
				case VerticalMovementState.upDown:
					movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
					break;
				case VerticalMovementState.none:
					movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
					break;
				default:
					movement = new Vector3 (0.0f, 0.0f, 0.0f);
					break;
			}

		GetComponent<Rigidbody> ().velocity = movement * speed * moveSpeed;
	
		GetComponent<Rigidbody> ().position = new Vector3 
		(
			Mathf.Clamp (GetComponent<Rigidbody> ().position.x, boundary.xMin, boundary.xMax), 
			Mathf.Clamp (GetComponent<Rigidbody> ().position.y, boundary.yMin, boundary.yMax), 
			Mathf.Clamp (GetComponent<Rigidbody> ().position.z, boundary.zMin, boundary.zMax)
		);
	
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody> ().velocity.x * -tilt);

	}

	public enum VerticalMovementState {forwardsBackwards, upDown, none};
}