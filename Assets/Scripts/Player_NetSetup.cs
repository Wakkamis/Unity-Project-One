using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetSetup : NetworkBehaviour {

    [SerializeField] GameObject thirdParsonCam;
    [SerializeField] GameObject thirdPersonCamTarget;

	// Use this for initialization
	void Start () 
    {
        if(isLocalPlayer)
        {
            GameObject.Find("Main Camera").SetActive(false);
            thirdPersonCamTarget.SetActive(true);
            GetComponent<MyCharController_vC>().enabled = true;
            thirdParsonCam.GetComponent<Camera>().enabled = true;
            thirdParsonCam.GetComponent<AudioListener>().enabled = true;
        }
	
	}
	
}
