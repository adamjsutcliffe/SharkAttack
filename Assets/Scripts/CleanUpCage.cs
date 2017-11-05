using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpCage : MonoBehaviour {

	public void OnTriggerEnter(Collider collider) {
		
		if (collider.gameObject.CompareTag("Cage")) {
			print ("Cage hit bottom");
			collider.gameObject.GetComponent<DragScript>().isDraggable = false;
			collider.gameObject.GetComponentInParent<Manager>().CageMissed();
			collider.gameObject.GetComponent<CapsuleCollider>().enabled = false;
			var meshColliders = collider.gameObject.GetComponentsInChildren <MeshCollider> ();
			foreach (MeshCollider mesh in meshColliders) 
			{
				mesh.enabled = false;
			}
			Rigidbody cageBody = collider.gameObject.GetComponent<Rigidbody> ();
			Destroy (cageBody);
		}
	}
}
