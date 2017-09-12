using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageCollision : MonoBehaviour {
	
	public GameObject shark;
	public Collider sphereCollider;
	// Use this for initialization
	void Start () {
		Vector3 rotation = new Vector3 (0, Random.Range (0, 360), 0);
		transform.Rotate(rotation);
	}
	
	// Update is called once per frame
	// void Update () {
		
	// }

	public void OnTriggerEnter(Collider collider) {

		if (collider.gameObject.CompareTag("Cage")) 
		{
			
			collider.gameObject.GetComponent<DragScript>().isDraggable = false;
			collider.gameObject.GetComponentInParent<Manager>().CageCollided();
			collider.gameObject.transform.parent = null;
			collider.gameObject.GetComponent<Rigidbody> ().useGravity = false;
			collider.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
			collider.gameObject.transform.parent = shark.transform;
			collider.gameObject.transform.localPosition = Vector3.zero;
			collider.gameObject.transform.rotation = shark.transform.rotation;
			collider.gameObject.transform.Rotate (new Vector3 (90, 0, 0));
			collider.gameObject.transform.localPosition = new Vector3(-0.3f,0.3f,-1.7f);

			GameObject cageDoor = collider.gameObject.transform.Find ("Cage_door").gameObject;
			if (cageDoor != null) {
				Vector3 doorRotation = new Vector3 (0, 32, 0);
				cageDoor.transform.Rotate (doorRotation);
			}
//			//MAYBE: add new shark 'struggle' animation or speed up swim anim
			shark.GetComponent<MovementScript> ().Surface();
			shark.GetComponent<MovementScript> ().sharkCaptured = true;
			sphereCollider.enabled = false;
		}
	}

	public void resetCage() {
		//TODO: reset cage
		print("NEED TO RESET CAGE!!");
	}
}
