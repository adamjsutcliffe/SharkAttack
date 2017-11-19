using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmerGlowScript : MonoBehaviour {

    public GameObject GlowObject;

    public void TurnOnGlow()
    {
        GlowObject.SetActive(true);
    }

    public void TurnOffGlow()
    {
        GlowObject.SetActive(false);
    }
}
