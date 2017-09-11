using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour {

	public bool isDraggable = false;
	public GameObject outline;
	public GameObject beachPlane;

	float distance = 70;
	float originalY = 0; 
	Rect beachRect;

	void Start()
	{
		float width = beachPlane.transform.localScale.z;
		float height = beachPlane.transform.localScale.x;
		beachRect = new Rect (beachPlane.transform.position.z - width/2f, beachPlane.transform.position.x - height/2f, width, height);
		print ("Beach rect: " + beachRect);
	}

	void OnTouchDown()
//	void OnMouseDown() 
	{
		if (!isDraggable) { return; }
		originalY = transform.position.y;
		print ("OriginalY: " + originalY);
		outline.SetActive (true);
	}

	void OnMouseDrag()
	{
		if (!isDraggable) { return; }
		Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, distance);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
		transform.position = objPosition;
//		Vector2 point = new Vector2 (objPosition.z, objPosition.x);
//		bool insideCheck = beachRect.Contains (point);
//		print ("Drag position: " + point + " beach rect: " + beachRect + " inside: " + insideCheck);

		Vector3 outlinePosition = new Vector3 (objPosition.x, 5, objPosition.z);
		outline.transform.position = outlinePosition;
	}

	void OnMouseUpAsButton() 
	{
		if (!isDraggable) { return; }
		if (PositionInsideBeach()) { return; }
		outline.SetActive (false);
		//TODO: tell game manager to respawn a cage
		print("Parent: " + this.transform.parent);
		transform.parent.GetComponent<Manager> ().SpawnCage ();
	}
		

	bool PositionInsideBeach()
	{
		Vector2 point = new Vector2 (transform.position.z, transform.position.x);
		Vector2 cageSize = new Vector2 (transform.localScale.x, transform.localScale.y);

		Rect cageRect = new Rect (point, cageSize);
		bool overlapCheck = beachRect.Overlaps (cageRect);
		return overlapCheck;
	}
}
