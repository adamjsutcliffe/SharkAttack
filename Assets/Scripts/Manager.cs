using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour {

	public Slider diffSlider;
	public Slider timeSlider;
	public Button startButton;
	public Button timeButton;
	public Button endDoneButton;
	public Button pauseButton;
	public Button resumeButton;
	public Button quitButton;
	public GameObject swimmer;
	public GameObject cage;
	public Animator UIController;
	public Animator SoundController;
	private Animator GameAnimator;

	public Text hintText;
	public Text scoreText;

	public TextMeshProUGUI cageTimerText;

	Vector3[] spawnValues;
	Vector3 cagePosition;

	int diffCount = 1;
	int cageType = 1;
	int cageSpawnCount = 0;
	int usedCageCount = 0;
	int correctCages = 0;

	float totalTime = 0f;
	float cageTimer = 0f;
	float totalCageTime = 0f;

	bool swimmersSpawned = false;
	bool gameStarted = false;
	bool cagesActive = false;

	enum playerState {swimming, diving, surfacing, stationary};

	GameObject[] swimmers;
	GameObject[] cages;

	// Use this for initialization
	void Start () {

		startButton.onClick.AddListener(nextPressed);
		timeButton.onClick.AddListener(startPressed);
		endDoneButton.onClick.AddListener(endDonePressed);
		pauseButton.onClick.AddListener (pauseGame);
		resumeButton.onClick.AddListener (resumeGame);
		quitButton.onClick.AddListener (quitGame);
		pauseButton.gameObject.SetActive(false);
		GameAnimator = gameObject.GetComponent <Animator>();
		GenerateGrid();
	}

	void resetGame()
	{
		totalTime = 0f;
		cageTimer = 0f;
		totalCageTime = 0f;
		cageSpawnCount = 0;
		usedCageCount = 0;
		correctCages = 0;
		swimmersSpawned = false;
		gameStarted = false;
		cagesActive = false;
		hintText.text = "";
		//clear cages 
		foreach (GameObject c in cages) {
			Destroy (c);
		}
		//clear swimmers 
		foreach (GameObject s in swimmers) {
			Destroy (s);
		}
	}

	void Update () {

		if (ApplicationModel.isPaused)
		{
			print ("game paused");
			return;
		}

		if (swimmersSpawned) {
				totalTime += Time.deltaTime;
				if (totalTime < 5) {
					hintText.text = "Memorise the swimmers position";
				}
		}

		if (totalTime > 5 && !gameStarted) {
			print("Make swimmers dive!!");
			gameStarted = true;
			foreach (GameObject s in swimmers) {
				s.GetComponentInChildren<MovementScript>().Dive();
			}
			int remainingTime = Mathf.FloorToInt(totalCageTime - cageTimer);
			cageTimerText.text = "" + remainingTime;
		}
		if (gameStarted) {
			cageTimer += Time.deltaTime;
			if (cageTimer > totalCageTime && !cagesActive) {
				print ("unlock cages");
				hintText.text = "Use the cages to capture the sharks";
				cagesActive = true;
				cageTimerText.text = "";
				// set cage draggable active = true
				foreach (GameObject c in cages) {
					c.GetComponentInChildren<DragScript>().isDraggable = true;
				}
			} else if (!cagesActive) {
				hintText.text = "Wait for the cages to unlock";
				int remainingTime = Mathf.FloorToInt(totalCageTime - cageTimer + 1);
				cageTimerText.text = "" + remainingTime;
			}
		}
	}

	void nextPressed()
	{
		UIController.SetTrigger("ShowCageMenu");
	}

	void startPressed()
	{
		UIController.SetTrigger("PlayGame");
		diffCount = (int)diffSlider.value;
		swimmers = new GameObject[diffCount];
		cages = new GameObject[diffCount];
		resetGame ();
		setCageTime ();
		print ("Diff: " + diffCount + " Time: " + cageType);
		Invoke ("SpawnCage", 0.5f);
		Invoke ("SpawnSwimmers", 2.0f);
		pauseButton.gameObject.SetActive(true);
		GameAnimator.SetTrigger ("StartDrama");
	}

	void endDonePressed()
	{
		GameAnimator.SetTrigger ("EndDrama");
		UIController.SetTrigger("CloseEndMenu");
		resetGame();
	}

	void setCageTime() 
	{
		cageType = (int)timeSlider.value;

		switch (cageType) {
		case 2:
			totalCageTime = 5f;
			break;
		case 3:
			totalCageTime = 10f;
			break;
		default:
			totalCageTime = 0.0f;
			break;
		}
		print ("cageTime: " + totalCageTime);
	}

	public void SpawnCage()
	{
		print ("Spawn cage -> Total needed: " + diffCount + " total made: " + cageSpawnCount);
		if (cageSpawnCount >= diffCount) { return; }
		
		Quaternion spawnRotation = Quaternion.identity;
		GameObject newCage = Instantiate(cage, cagePosition, spawnRotation);
		newCage.GetComponent<CageAppearance> ().SetColour (cageType - 1);
		newCage.transform.parent = this.transform;
		bool dragCheck = cageTimer > totalCageTime;
		print("Drag check: " + dragCheck);
		
		print ("show cage at position: " + cagePosition + "with type: " + cageType);
		cages [cageSpawnCount] = newCage;
		newCage.GetComponent<DragScript> ().isDraggable = dragCheck;
		print("Cage is draggable: " + newCage.GetComponent<DragScript> ().isDraggable);
		cageSpawnCount += 1;
	}

	void SpawnSwimmers()
	{
		print ("Swimmers count 1: " + swimmers.Length);
		Quaternion spawnRotation = Quaternion.identity;
		for (int i = 0; i < diffCount; i++) {
			Vector3 spawnPosition = spawnValues[i];
			print ("swimmer position: " + spawnPosition);
			GameObject newSwimmer = Instantiate (swimmer, spawnPosition, spawnRotation);
			swimmers[i] = newSwimmer;
		}
		print ("Swimmers count 2: " + swimmers.Length);
		swimmersSpawned = true;
	}

	void GenerateGrid() 
	{
		float aspectRatio = (float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight;
		float screenHeight = Mathf.Tan(Camera.main.fieldOfView/2 * Mathf.Deg2Rad) * Camera.main.transform.position.y * 2;
		float screenWidth = screenHeight * aspectRatio;
		print ("Actual game view height: " + screenHeight + " width: " + screenWidth +" aspect ratio: " + aspectRatio);

		float squareHeight = screenHeight*0.9f / 4.0f;
		float squareWidth = screenHeight*0.9f / 3.0f;
		print ("square: " + squareWidth + ":" + squareHeight);
		cagePosition = new Vector3 (squareHeight*0.9f + Camera.main.gameObject.transform.position.x*0.5f, 6, squareWidth*1.25f);

		int rows = 4;
		int cols = 3;
		int counter = 0;
		spawnValues = new Vector3[rows * cols];
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {

				float xPosition = squareHeight * row + squareHeight*0.5f + Camera.main.gameObject.transform.position.x*0.5f + screenHeight*0.07f;
				float zPosition = squareWidth * col + squareWidth*0.5f + Camera.main.gameObject.transform.position.z*0.8f;
				Vector3 spawnPosition = new Vector3 (xPosition, -2.25f, zPosition);
				spawnValues [counter] = spawnPosition;
				counter += 1;
				print ("position: " + spawnPosition);
			}
		}
		shuffle (spawnValues);
	}

	void shuffle<T> (T[] texts)
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < texts.Length; t++ )
		{
			T tmp = texts[t];
			int r = Random.Range(t, texts.Length);
			texts[t] = texts[r];
			texts[r] = tmp;
		}
	}

	public void PlaySplash()
	{
		SoundController.SetTrigger ("PlaySplash");
	}

	public void PlayClap()
	{
		SoundController.SetTrigger ("PlayClap");
	}

	public void PlayGasp()
	{
		SoundController.SetTrigger ("PlayGasp");
	}

	public void CageCollided() {

		print("Cage Collided :)");
		usedCageCount += 1;
		correctCages += 1;
		PlayClap ();
		checkAllCagesUsed();
	}

	public void CageMissed() {
		print("Cage Missed :(");
		usedCageCount += 1;
		PlayGasp ();
		checkAllCagesUsed();
	}

	int gameScore()
	{
		return correctCages * 10 * cageType;
	}

	void surfaceMissedSwimmers()
	{
		foreach (GameObject s in swimmers) {
			if (!s.GetComponentInChildren<MovementScript> ().sharkCaptured) {
				s.GetComponentInChildren<MovementScript> ().Surface ();
			}
		}
	}

	void checkAllCagesUsed() {

		if (usedCageCount == diffCount) {
			print("all cages used!! Correct: " + correctCages + "/" + diffCount);
			surfaceMissedSwimmers();
			int newScore = gameScore ();

			int highScore = PlayerPrefs.GetInt ("high_score");
			if (newScore > highScore) {
				PlayerPrefs.SetInt ("high_score", newScore);
			}
			string scoreTextStr = "<color=#00FF00FF>You scored: "+ newScore + "</color>";
			if (PlayerPrefs.HasKey ("prev_score")) 
			{
				scoreTextStr += "\nPrevious score: " + PlayerPrefs.GetInt ("prev_score");
			}
			if (PlayerPrefs.HasKey ("high_score"))
			{
				scoreTextStr += "\nHighest score: " + PlayerPrefs.GetInt ("high_score");
			}
			scoreText.text = scoreTextStr;
			PlayerPrefs.SetInt ("prev_score", newScore);
			pauseButton.gameObject.SetActive(false);
			Invoke ("showEndPanel", 2.0f);
		}
	}

	void showEndPanel()
	{
		UIController.SetTrigger("EndGame");
	}


	void pauseGame()
	{
		ApplicationModel.isPaused = true;
		pauseButton.gameObject.SetActive(false);
		UIController.SetTrigger("ShowPauseMenu");
	}

	void resumeGame()
	{
		UIController.SetTrigger("ResumeGame");
		pauseButton.gameObject.SetActive(true);
		ApplicationModel.isPaused = false;
	}

	void quitGame()
	{
		UIController.SetTrigger("QuitGame");
		ApplicationModel.isPaused = false;
		resetGame ();
	}


}
