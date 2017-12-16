using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using System;

public class Manager : MonoBehaviour
{
	private float swimmerShownDuration;
	private int scoreMultiplier;

	public Slider diffSlider;
	public Slider exposureSlider;
	public Slider timeSlider;
	public Button pauseButton;
	public GameObject swimmer;
	public GameObject cage;
	public GameObject Beach;
    public GameObject cageImage;
	public GameObject GlowSphere;
	public Animator UIController;
	public Animator SoundController;
	private Animator GameAnimator;

	public Text hintText;
   // public Image cageImage;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI cageTimerText;

	public Material[] colours;

	Vector3[] spawnValues;
	Vector3 cagePosition;

	string cageColourText;

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

    private bool cageHintActive;



	enum playerState
	{
		swimming,
		diving,
		surfacing,
		stationary
	};

	List<GameObject> swimmers;
	List<GameObject> cages;

	void Start ()
	{
		pauseButton.gameObject.SetActive (false);
		cageTimerText.gameObject.SetActive (false);
		GameAnimator = gameObject.GetComponent <Animator> ();
		GenerateGrid ();
       
	}


    void resetGame ()
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
		cageTimerText.text = "";
		cageTimerText.gameObject.SetActive (true);
		//clear cages 
		if (cages != null) {
			foreach (GameObject c in cages) {
				Destroy (c);
			}
		}
		//clear swimmers 
		if (swimmers != null) {
			foreach (GameObject s in swimmers) {
				Destroy (s);
			}
		}
	}

	void Update ()
	{
		if (ApplicationModel.isPaused) {
			print ("game paused");
			return;
		}

		if (swimmersSpawned) {
			totalTime += Time.deltaTime;
			if (totalTime < swimmerShownDuration) {
				hintText.text = "Memorise the swimmers position";
                cageImage.gameObject.SetActive(false);
			}
		}

		if (totalTime > swimmerShownDuration && !gameStarted) {
			print ("Make swimmers dive!!");
			gameStarted = true;
			foreach (GameObject s in swimmers) {

                MovementScript mS = s.GetComponentInChildren<MovementScript>();
                if (mS != null)
                {
                    mS.Dive();
                }
			}
		}

		if (gameStarted) {
			cageTimer += Time.deltaTime;

			if (cageTimer > totalCageTime && !cagesActive) {
				print ("unlock cages");
				hintText.text = "Drag the cages      to capture the sharks";
                cageImage.gameObject.SetActive(true);
                cagesActive = true;
				GlowSphere.GetComponent<GlowScript> ().MakeGlowGreen ();
				cageTimerText.gameObject.SetActive (true);
				foreach (GameObject c in cages) {
					print (c.name + " has a drag script? " + c.GetComponent<DragScript> ());
					c.GetComponent<DragScript> ().isDraggable = true;
				}
			} 
			else if (!cagesActive) {
				
				cageTimerText.gameObject.SetActive (true);
				hintText.text = "Wait for the cages to unlock";
                cageImage.gameObject.SetActive(false);
				int remainingTime = Mathf.FloorToInt (totalCageTime - cageTimer + 1);
				cageTimerText.text = "Cages unlock in:\n" + remainingTime + " secs";
			} 
			else 
			{
				int remainingCount = diffCount - cageSpawnCount + 1;
				cageTimerText.text = remainingCount + "\n" + cageColourText + " CAGE" + (remainingCount > 1 ? "S" : "") + "\nremaining";
			}
				
            
        }

       
	}


	#region Button handling
	public void PressSwimmerInfoNext()
	{
		UIController.SetTrigger ("ShowSwimmerSelection");
	}

	public void PressSwimmerSelectionNext()
	{
		UIController.SetTrigger ("ShowExposureMenu");
	}

	public void PressExposureInfoNext()
	{
		UIController.SetTrigger ("ShowExposureSelection");
	}

	public void PressExposureSelectionNext()
	{
		UIController.SetTrigger ("ShowCageMenu");
	}

	public void PressCageInfoNext()
	{
		UIController.SetTrigger ("ShowCageSelection");
	}

	public void PressCageSelectionNext()
	{
		UIController.SetTrigger ("PlayGame");
		startGame ();
	}

	public void PressedReplay()
	{
		UIController.SetTrigger ("ReplayGame");
		startGame ();
	}

	public void PressedEndDone ()
	{
		GameAnimator.SetTrigger ("EndDrama");
		UIController.SetTrigger ("CloseEndMenu");
		resetGame ();
	}

	public void showEndPanel ()
	{
		UIController.SetTrigger ("EndGame");
	}

    public void BackSwimmerMenu()
    {
        UIController.SetTrigger("BackSwimmerMenu");
    }

	public void PressPauseGame ()
	{
		ApplicationModel.isPaused = true;
		pauseButton.gameObject.SetActive (false);
		hintText.gameObject.SetActive (false);
        cageHintActive = cageImage.gameObject.activeSelf;
        cageImage.gameObject.SetActive(false);
		cageTimerText.gameObject.SetActive (false);
		UIController.SetTrigger ("ShowPauseMenu");
	}

	public void PressResumeGame ()
	{
		UIController.SetTrigger ("ResumeGame");
		pauseButton.gameObject.SetActive (true);
		hintText.gameObject.SetActive (true);
        cageImage.gameObject.SetActive(cageHintActive);
		cageTimerText.gameObject.SetActive (true);
		ApplicationModel.isPaused = false;
	}

	public void PressQuitGame ()
	{
		GameAnimator.SetTrigger ("EndDrama");
        UIController.SetTrigger ("QuitGame");
		ApplicationModel.isPaused = false;
		resetGame ();
	}

	#endregion

	void startGame()
	{
		resetGame ();
		diffCount = (int)diffSlider.value;
		swimmers = new List<GameObject> ();
		cages = new List<GameObject>();
		setExposureTime();
		setCageTime ();
		print ("Diff: " + diffCount + " exposure time: " + swimmerShownDuration + " Cage Time: " + cageType);
		Invoke ("SpawnCage", 0.5f);
		Invoke ("SpawnSwimmers", 2.0f);
		pauseButton.gameObject.SetActive (true);
		GlowSphere.GetComponent<GlowScript> ().MakeGlowRed ();
		GlowSphere.SetActive (true);
		GameAnimator.SetTrigger ("StartDrama");
    //    cage.gameObject.SetActive(true);
    
    }


    void setCageTime ()
	{
		cageType = (int)timeSlider.value;

		switch (cageType) {
		case 2:
			totalCageTime = 5f;
			cageColourText = "<color=#C9B200FF>GOLD</color>";
			break;
		case 3:
			totalCageTime = 15f;
			cageColourText = "<color=#1E25B0FF>SAPPHIRE</color>";
			break;
		default:
			totalCageTime = 0.0f;
			cageColourText = "<color=#909090FF>SILVER</color>";
			break;
		}
		print ("cageTime: " + totalCageTime);
	}

	void setExposureTime()
	{
		swimmerShownDuration = (int)exposureSlider.value == 1 ? 11f : 4f;
		scoreMultiplier = (int)exposureSlider.value == 1 ? 1 : 2;
		print ("Level Exposure time: " + swimmerShownDuration + " score mulitplier: " + scoreMultiplier);
	}

	public void SpawnCage ()
	{
		cageSpawnCount += 1;
		print ("[SPAWN CAGES] Cage spawn count: " + cageSpawnCount + " Diff count: " + diffCount);
		if (cageSpawnCount > diffCount) {
			return;
		}
		
		Quaternion spawnRotation = Quaternion.identity;
		print ("Spawn cage: " + cagePosition);
		GameObject newCage = Instantiate (cage, cagePosition, spawnRotation);
		newCage.GetComponent<CageAppearance> ().SetColour (cageType - 1);
      //  newCage.GetComponent<Animation>().Play();
        newCage.name = "Cage" + cageSpawnCount;
		print ("Cage postion before: " + newCage.transform.position);
		newCage.transform.parent = this.transform;
		print ("Cage postion after: " + newCage.transform.position);
		bool dragCheck = cageTimer > totalCageTime;
		cages.Add(newCage);
		newCage.GetComponent<DragScript> ().isDraggable = dragCheck;

		int remainingCount = diffCount - cageSpawnCount + 1;
		cageTimerText.text = remainingCount + "\n" + cageColourText + " CAGE" + (remainingCount > 1 ? "S" : "") + "\nremaining";
	}

	void SpawnSwimmers ()
	{
		Quaternion spawnRotation = Quaternion.identity;
		shuffle (spawnValues);
		shuffle (colours);
		for (int i = 0; i < diffCount; i++) {
            Material color = colours[i];
			Vector3 spawnPosition = spawnValues [i];
			GameObject newSwimmer = Instantiate (swimmer, spawnPosition, spawnRotation);
            newSwimmer.GetComponent<ColourGenerator>().SetSwimmerColour(color);
			swimmers.Add (newSwimmer);

            Vector3 decoyPosition = spawnValues[i + diffCount];
            GameObject decoySwimmer = Instantiate(swimmer, decoyPosition, spawnRotation);
            ColourGenerator cg = decoySwimmer.GetComponent<ColourGenerator>();
            cg.SetSwimmerColour(color);
            cg.RemoveShark(false);
            swimmers.Add(decoySwimmer);
		}
		swimmersSpawned = true;
	}

	void GenerateGrid ()
	{
		var camera = Camera.main;
		float aspectRatio = (float)camera.pixelWidth / (float)camera.pixelHeight;
		float screenHeight = Mathf.Tan (camera.fieldOfView / 2 * Mathf.Deg2Rad) * camera.transform.position.y * 2;
		float screenWidth = screenHeight * aspectRatio;
		float squareHeight = screenHeight * 0.86f / 5.0f;
		float squareWidth = screenHeight * 0.9f / 4.0f;

		camera.gameObject.transform.position = new Vector3 (camera.gameObject.transform.position.x, camera.gameObject.transform.position.y, screenWidth/2);

		cagePosition = new Vector3 (squareHeight * 1.8f + camera.gameObject.transform.position.x * 0.5f, 6f, squareHeight * 0.9f); //squareWidth * 1.2f);
		GlowSphere.transform.position = cagePosition;

		int rows = 5;
		int cols = 4;
		int counter = 0;
		spawnValues = new Vector3[rows * cols];
		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {

				float xPosition = (squareHeight * row) + (squareHeight * 0.5f) + (camera.gameObject.transform.position.x * 0.5f) + (screenHeight * 0.07f);
				float zPosition = (squareWidth * col) + (squareWidth * 0.5f) + (screenWidth/2 * 0.6f);
				Vector3 spawnPosition = new Vector3 (xPosition, -2.25f, zPosition);
				spawnValues [counter] = spawnPosition;
				counter += 1;
			}
		}
	}

	void shuffle<T> (T[] texts)
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < texts.Length; t++) {
			T tmp = texts [t];
			int r = Random.Range (t, texts.Length);
			texts [t] = texts [r];
			texts [r] = tmp;
		}
	}

	public void PlaySplash ()
	{
		SoundController.SetTrigger ("PlaySplash");
	}

	public void PlayClap ()
	{
		SoundController.SetTrigger ("PlayClap");
	}

	public void PlayGasp ()
	{
		SoundController.SetTrigger ("PlayGasp");
	}

	public void CageCollided ()
	{
		print ("Cage Collided :)");
		usedCageCount += 1;
		correctCages += 1;
        Invoke("PlayClap", 0.5f);
		checkAllCagesUsed ();
	}

	public void CageMissed ()
	{
		print ("Cage Missed :(");
		usedCageCount += 1;
		PlayGasp ();
		checkAllCagesUsed ();
	}

	int gameScore ()
	{
		return correctCages * cageValueForType() * scoreMultiplier;
	}

	int cageValueForType()
	{
		switch (cageType) {
		case 2:
			return 25;
		case 3:
			return 50;
		default:
			return 10;
		}
	}

	void surfaceMissedSwimmers ()
	{
		foreach (GameObject s in swimmers) {
            MovementScript mS = s.GetComponentInChildren<MovementScript>();
            if (mS != null && !mS.sharkCaptured)
            {
                mS.Surface();
            }
			//if (!s.GetComponentInChildren<MovementScript> ().sharkCaptured) {
			//	s.GetComponentInChildren<MovementScript> ().Surface ();
			//}
		}
	}

	void checkAllCagesUsed ()
	{

		if (usedCageCount == diffCount) {
			print ("all cages used!! Correct: " + correctCages + "/" + diffCount);
			surfaceMissedSwimmers ();
			int newScore = gameScore ();

			int highScore = PlayerPrefs.GetInt ("high_score");
			if (newScore > highScore) {
				PlayerPrefs.SetInt ("high_score", newScore);
			}
			string scoreTextStr = "<color=#00FF00FF>You scored: " + newScore + "</color>";
			if (PlayerPrefs.HasKey ("prev_score")) {
				scoreTextStr += "\n\nPrevious score: " + PlayerPrefs.GetInt ("prev_score");
			}
			if (PlayerPrefs.HasKey ("high_score")) {
				scoreTextStr += "\n\nHighest score: " + PlayerPrefs.GetInt ("high_score");
			}
			scoreText.text = scoreTextStr;
			PlayerPrefs.SetInt ("prev_score", newScore);
			pauseButton.gameObject.SetActive (false);
			cageTimerText.gameObject.SetActive (false);
			GlowSphere.SetActive (false);
			hintText.text = "";
            cageImage.gameObject.SetActive(false);
			Invoke ("showEndPanel", 1.0f);
		}
	}


		
}
