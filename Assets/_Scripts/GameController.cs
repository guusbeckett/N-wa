using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValues;
	public float ZValue3D = 60f;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public float speedIncrease;
	private int waveCounter;
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText waveCountText;
	public GUIText finalScoreText;
	private bool gameOver;
	private bool restart;
	private int score;
	public AudioSource gameMusic;
	public AudioSource gameOverMusic;
	public Camera mainCamera;
	public PlayerController player;
	public Vector3 cameraFPOffset;
	private bool isCameraMoving = false;
	public bool isCameraFirstPerson = false;
	public float cameraTransitionSpeed;
	private bool isSpawningSceneryWaves = false;
	private bool displayWarning = false;
	private bool usingScaler = false;
	public Skybox skybox_3d;
	public Light lightSun;
	private bool sunFadingIn;
	private bool sunFadingOut;
	public GameObject warningImage;
	public GameObject warningText;
	public GameObject hugeWarningText;
	void Start() {
		gameOver = false;
		restart = false;
		gameOverMusic.Stop ();
		gameMusic.Play ();
		restartText.text = "";
		gameOverText.text = "";
		finalScoreText.text = "";
		score = 0;
		hazard.GetComponent<MoverAstroid>().speed = -10;
		waveCounter = 1;
		UpdateScore ();
		StartCoroutine(spawnWaves ());
		usingScaler = false;
		player.verticalMovementState = PlayerController.VerticalMovementState.forwardsBackwards;
		displayWarning = false;
		warningImage.SetActive (false);
		warningText.SetActive (false);
		hugeWarningText.SetActive (false);
	}
	void Update (){
		if (restart && Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		if (waveCounter >= 10 && !isCameraMoving) {
			mainCamera.GetComponent<Transform>().position = player.GetComponent<Rigidbody>().position  + cameraFPOffset;
			mainCamera.GetComponent<Transform>().rotation = Quaternion.Euler(0.0f,0.0f,0);
		}
		if (Input.GetKeyDown (KeyCode.P)) {
			waveCounter = 9;
			hazardCount = 37;
		}
		if (Input.GetKeyDown (KeyCode.B) && hugeWarningText.activeSelf) {
			hugeWarningText.SetActive(false);
		}
		if (isCameraMoving) {
			float step = cameraTransitionSpeed * Time.deltaTime;
			player.userInputEnabled = false;
			mainCamera.GetComponent<Transform> ().position = Vector3.MoveTowards (mainCamera.GetComponent<Transform> ().position, player.GetComponent<Transform> ().position + cameraFPOffset, step);
			mainCamera.GetComponent<Transform> ().rotation = Quaternion.RotateTowards (mainCamera.GetComponent<Transform> ().rotation, player.GetComponent<Transform> ().rotation, step * 6f);
			if (mainCamera.GetComponent<Transform> ().position.y < player.GetComponent<Transform> ().position.y + cameraFPOffset.y + 2) {
				player.moveForwardsAutomatically = true;
				print("Moving!");
			}
			if (mainCamera.GetComponent<Transform> ().position == player.GetComponent<Transform> ().position + cameraFPOffset) {
				isCameraMoving = false;
				isCameraFirstPerson = true;
				player.moveForwardsAutomatically = false;
				player.userInputEnabled = true;
			}
		}
		if (sunFadingIn) {
			lightSun.GetComponent<Light>().intensity += 0.03f;
			if(lightSun.GetComponent<Light>().intensity >= 7) {
				sunFadingIn = false;
				player.ableToFire = false;
				hugeWarningText.SetActive(true);
			}
		}
		if (sunFadingOut) {
			lightSun.GetComponent<Light>().intensity -= 0.03f;
			if(lightSun.GetComponent<Light>().intensity <= 2) {
				sunFadingOut = false;
				hugeWarningText.SetActive(false);
				player.ableToFire = true;
			}
		}
	}
	IEnumerator spawnWaves () {
		yield return new WaitForSeconds (startWait);
		float spawnY = new float();
		spawnY = spawnValues.y;
		while (true) {
			if (!isCameraMoving) {
				for (int i = 0; i < hazardCount; i++) {
					if (player.verticalMovementState == PlayerController.VerticalMovementState.upDown) {
						spawnY = Random.Range (-6.5f, 6.5f);
					}
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnY, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity; 
					hazard.GetComponent<MoverAstroid>().speed *= speedIncrease;
					if(!usingScaler) {
						hazard.GetComponent<MoverAstroid>().scaler = new Vector3(1.0f,1.0f,1.0f);
					}
					Instantiate (hazard, spawnPosition, spawnRotation);
					yield return new WaitForSeconds (spawnWait);
				}
				hazardCount = hazardCount + hazardCount / 6;
				spawnWait /= 1.2f;
				hugeWarningText.SetActive (false);
				yield return new WaitForSeconds (waveWait);
				UpdateScore ();
				if (gameOver)
				{
					finalScoreText.text = "You died in wave " + waveCounter + " with a score of " + score;
					restartText.text = "Press 'R' to restart";
					restart = true;
					break;
				}
				waveCounter++;
				UpdateScore();
				if (waveCounter == 10) {
					isCameraMoving = true;
					isSpawningSceneryWaves = true;
//					StartCoroutine(spawnSceneryWaves ());
					mainCamera.GetComponent<Camera>().orthographic = false;
					player.tilt = 1;
					player.boundary.xMin = -20.0f;
					spawnValues.x = player.boundary.xMax = 20.0f;
					spawnValues.z = ZValue3D;
					usingScaler = true;
					player.verticalMovementState = PlayerController.VerticalMovementState.upDown;
					hazardCount = 300;
					spawnWait = 0.03f;
				}
				if (waveCounter == 14 || waveCounter == 24 || waveCounter == 34 || waveCounter ==  44) {
					warningImage.SetActive (true);
					warningText.SetActive (true);
					sunFadingIn = true;
				}
				else if (waveCounter == 16 || waveCounter == 26 || waveCounter == 36 || waveCounter ==  46) {
					warningImage.SetActive (false);
					warningText.SetActive (false);
					sunFadingOut = true;
				}

			}
			else {
				yield return new WaitForSeconds (5);
			}
		}
	}
	public void AddScore (int newScoreValue) {
		score += newScoreValue;
		UpdateScore ();
	}
	void UpdateScore () {
		scoreText.text = "Score: " + score;
		waveCountText.text = "Wave: " + waveCounter;
	}
	public void GameOver ()	{
		gameOverText.text = "Game Over!";
		gameOver = true;
		gameMusic.Stop ();
		gameOverMusic.PlayDelayed (0.5f);
	}

	public void scorePenalty(int penalty) {
		score -= penalty;
		if (score < 0)
			score = 0;
		UpdateScore ();
	}
	
	IEnumerator spawnSceneryWaves () {
		float minSpawnY = spawnValues.y - 4;
		float maxSpawnY = spawnValues.y - 10;
		Vector3 spawnPosition;
		while (isSpawningSceneryWaves) {
			for (int i = 0; i < hazardCount; i++) {
				if (i % 2 == 0) 
					spawnPosition = new Vector3 (Random.Range (-spawnValues.x-30, spawnValues.x+30), Random.Range(minSpawnY, maxSpawnY) , spawnValues.z);
				else 
					spawnPosition = new Vector3 (Random.Range (-spawnValues.x-30, spawnValues.x+30), Random.Range(-minSpawnY, -maxSpawnY) , spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity; 
				hazard.GetComponent<MoverAstroid>().speed *= speedIncrease ;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (0.02f);
			}
		}
	}
}
