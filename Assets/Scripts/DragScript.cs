using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour {

	public bool isDraggable;
	public GameObject outline;
	public GameObject Halo;
    private Manager gameManager;

	float distance = 70;
	float originalY = 0; 

	private bool hasHitWater;

	void Start()
	{
		hasHitWater = false;
        gameManager = transform.parent.GetComponent<Manager>();
        print("Cage has manager: " + gameManager);
	}

	void Update()
	{
		if (transform.position.y < 0 && !hasHitWater) {
			print ("HIT WATER!!");
			hasHitWater = true;
            //if (gameManager != null)
            //{
                gameManager.PlaySplash();
            //}
		}
	}

	void OnMouseDown()
	{
		if (!isDraggable) { return; }
		originalY = transform.position.y;
		outline.SetActive (true);
		Halo.SetActive (true);
	}

	void OnMouseDrag()
	{
		if (!isDraggable) { return; }
		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
		transform.position = objPosition;

		Vector3 outlinePosition = new Vector3 (objPosition.x, 5, objPosition.z);
		outline.transform.position = outlinePosition;
	}

	void OnMouseUpAsButton() 
	{
		Halo.SetActive (false);
		if (!isDraggable) { return; }
		if (PositionInsideBeach()) { return; }
		outline.SetActive (false);
		print("Parent: " + this.transform.parent);
        gameManager.SpawnCage ();
	}
		

	bool PositionInsideBeach()
	{
		print("Position in side beach: " + transform.position.z);
		return transform.position.z < 24.0f;;
	}
}
