using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageCollision : MonoBehaviour {
	
	public GameObject shark;
	public Collider sphereCollider;

	void Start () {
		Vector3 rotation = new Vector3 (0, Random.Range (0, 360), 0);
		transform.Rotate(rotation);
	}

	public void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.CompareTag("Cage") && shark != null) 
		{
			print ("Cage collided with shark!!");
			sphereCollider.enabled = false;
			collider.enabled = false;
			collider.gameObject.GetComponent<DragScript>().isDraggable = false;
			collider.gameObject.GetComponentInParent<Manager>().CageCollided();
			collider.gameObject.transform.parent = null;
			collider.gameObject.GetComponent<CapsuleCollider>().enabled = false;
			var meshColliders = collider.gameObject.GetComponentsInChildren <MeshCollider> ();
			foreach (MeshCollider mesh in meshColliders) 
			{
				mesh.enabled = false;
			}
			Rigidbody cageBody = collider.gameObject.GetComponent<Rigidbody> ();
			Destroy (cageBody);
			collider.gameObject.transform.parent = shark.transform;
			collider.gameObject.transform.localPosition = Vector3.zero;
			collider.gameObject.transform.rotation = shark.transform.rotation;
			collider.gameObject.transform.Rotate (new Vector3 (90, 0, 0));
			collider.gameObject.transform.localPosition = new Vector3(-0.15f,0.3f,-1.1f);

			GameObject cageDoor = collider.gameObject.transform.Find ("Cage_door").gameObject;
			if (cageDoor != null) {
				Vector3 doorRotation = new Vector3 (0, 32, 0);
				cageDoor.transform.Rotate (doorRotation);
			}

			shark.GetComponent<MovementScript> ().Surface();
			shark.GetComponent<MovementScript> ().sharkCaptured = true;
		}
	}
}
