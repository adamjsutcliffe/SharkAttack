using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageAppearance : MonoBehaviour {

	public Material[] colours;
	public GameObject cageBody;
	public GameObject cageDoor;


	public void SetColour(int value) 
	{
		print ("Set cage colour: " + value);
		cageBody.GetComponent<Renderer> ().material = colours[value];
		cageDoor.GetComponent<MeshRenderer> ().material = colours[value];

	}

}
