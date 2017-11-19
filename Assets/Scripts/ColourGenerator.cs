using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator : MonoBehaviour {

	public GameObject suit;
	public GameObject shark;
	public SphereCollider collider;

	public void SetSwimmerColour(Material colour)
	{
		suit.GetComponent<SkinnedMeshRenderer> ().material = colour;
	}

	public void RemoveShark(bool removeCollider)
	{
        print("remove shark and collider ? " + removeCollider);
        Destroy(shark);
        if (removeCollider)
        {
            Destroy(collider);
        }

	}
}
