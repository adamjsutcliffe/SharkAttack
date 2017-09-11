using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator : MonoBehaviour {

	public GameObject suit;
	public Material[] colours;

	// Use this for initialization
	void Start () {

		int randomColour = Random.Range(0, colours.Length);
		print("load suit colour, size: " + colours.Length + " random: " + randomColour);
		suit.GetComponent<SkinnedMeshRenderer> ().material = colours[randomColour];
	}
}
