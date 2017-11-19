using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Swimmer")
        {
            print("Hover collided with " + other.gameObject.tag + " other");
            other.gameObject.GetComponent<SwimmerGlowScript>().TurnOnGlow();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Swimmer")
        {
            print("Hover STOPPED colliding with " + other.gameObject.tag);
            other.gameObject.GetComponent<SwimmerGlowScript>().TurnOffGlow();
        }
    }
}
