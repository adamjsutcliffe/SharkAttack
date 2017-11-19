using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

	public enum playerState
	{
		swimming,
		diving,
		surfacing,
		stationary
	};

	public playerState state = playerState.swimming;
	public GameObject swimmer;
	public bool sharkCaptured = false;

	float totalDive;

	void Start ()
	{
		totalDive = 0;
		state = playerState.swimming;
	}

	void Update ()
	{

		if (ApplicationModel.isPaused) {
			print ("game paused");
			return;
		}

		if (!sharkCaptured) {
//			print ("surfacing: shark not captured; Time: " + Time.deltaTime + "local position: " + swimmer.transform.localPosition);
			transform.RotateAround (swimmer.transform.localPosition, Vector3.up, 40 * Time.deltaTime);

		}

		switch (state) {
		case playerState.swimming:
			{
				if (transform.position.y < 0.7f)
				{
					var newPosition = new Vector3 (transform.position.x, transform.position.y + 0.12f, transform.position.z);
					transform.position = newPosition;
				}
				break;
			}
		case playerState.diving:
			{
				totalDive += 10 * Time.deltaTime;
				if (totalDive < 15) {
//					Vector3 swimmerPosition = new Vector3 (swimmer.transform.position.x, -totalDive, swimmer.transform.position.z);
//					swimmer.transform.position = swimmerPosition;
					Vector3 swimmerPosition = new Vector3 (transform.position.x, -totalDive, transform.position.z);
					transform.position = swimmerPosition;
				} else {
					state = playerState.stationary;
				}
				break;
			}
		case playerState.surfacing:
			{
				totalDive -= 10 * Time.deltaTime;

				if (totalDive > -2) {
					
//					Vector3 swimmerPosition = new Vector3 (swimmer.transform.position.x, -totalDive, swimmer.transform.position.z);
//					swimmer.transform.position = swimmerPosition;
					Vector3 swimmerPosition = new Vector3 (transform.position.x, -totalDive, transform.position.z);
					transform.position = swimmerPosition;
				} else {
					state = playerState.stationary;	
				}
				break;
			}
		case playerState.stationary:
			{
				break;
			}
		}
	}

	public void Dive ()
	{

		if (state == playerState.diving) {
			return;
		}
		state = playerState.diving;
		print ("DIVE!!");
	}

	public void Surface ()
	{
		if (state == playerState.surfacing) {
			return;
		}
		state = playerState.surfacing;
		print ("SURFACE!!");
	}
}
