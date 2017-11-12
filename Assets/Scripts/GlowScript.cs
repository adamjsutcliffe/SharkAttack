using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowScript : MonoBehaviour {

	public Material red;
	public Material green;

	public void MakeGlowGreen()
	{
		GetComponent<MeshRenderer> ().material = green;
	}

	public void MakeGlowRed()
	{
		GetComponent<MeshRenderer> ().material = red;
	}
}
