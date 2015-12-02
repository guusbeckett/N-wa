using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject hazard;
	public Vector3 spawnValues;
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
	}
	void Update (){
		if (restart && Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		if (waveCounter > 2) {
			mainCamera.GetComponent<Transform>().position = player.GetComponent<Rigidbody>().position;
			mainCamera.GetComponent<Transform>().rotation = player.GetComponent<Rigidbody>().rotation;
		}
	}
	IEnumerator spawnWaves () {
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity; 
				hazard.GetComponent<MoverAstroid>().speed *= speedIncrease;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			hazardCount=hazardCount + hazardCount;
			spawnWait/=2f;
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
			if (waveCounter == 3) {
				mainCamera.GetComponent<Camera>().orthographic = false;
				player.tilt = 1;
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
}
