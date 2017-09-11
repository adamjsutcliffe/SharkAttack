using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpCage : MonoBehaviour {

	public void OnTriggerEnter(Collider collider) {
		
		if (collider.gameObject.CompareTag("Cage")) {
			print ("Cage hit bottom");
//			Destroy (collider.gameObject);
			collider.gameObject.GetComponent<DragScript>().isDraggable = false;
			collider.gameObject.GetComponentInParent<Manager>().CageMissed();
		}
	}
}
