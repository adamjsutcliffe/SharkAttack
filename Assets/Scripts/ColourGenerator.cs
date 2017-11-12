using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator : MonoBehaviour {

	public GameObject suit;
	public GameObject shark;
	public SphereCollider collider;

	//public Material[] colours;

	//void Awake () {

	//	int randomColour = Random.Range(0, colours.Length);
	//	print("load suit colour, size: " + colours.Length + " random: " + randomColour);
	//	suit.GetComponent<SkinnedMeshRenderer> ().material = colours[randomColour];
	//}

	public void SetSwimmerColour(Material colour)
	{
		suit.GetComponent<SkinnedMeshRenderer> ().material = colour;
	}

	public void RemoveShark()
	{
        //TODO: destroy shark object and swimmers collider
        print("remove shark and collider");
        Destroy(shark);
        Destroy(collider);
	}
}
