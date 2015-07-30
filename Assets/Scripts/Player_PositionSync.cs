using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_PositionSync : NetworkBehaviour {

    [SyncVar] private Vector3 syncPlayerPosition;

    [SyncVar] private Quaternion syncPlayerRotation;

    [SerializeField] private Transform playerTransform;
    [SerializeField] float lerpRate = 15;

    private Vector3 lastPlayerPosition;
    private float thresholdPosition = 0.5f;

    private Quaternion lastPlayerRotation;
    private float thresholdRotation = 5;
	
    void Update()
    {
        lerpPosition();
    }

	void FixedUpdate () 
    {
        TransmitPosition();
	}

    void lerpPosition()
    {
        if(!isLocalPlayer)
        {
            playerTransform.position = Vector3.Lerp(playerTransform.position, syncPlayerPosition, Time.deltaTime * lerpRate);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer (Vector3 playerPosition)
    {
        syncPlayerPosition = playerPosition;
    }

    [Command]
    void CmdProvideRotationToServer (Quaternion playerRotation)
    {
        syncPlayerRotation = playerRotation;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(playerTransform.position, lastPlayerPosition) > thresholdPosition)
        {
            CmdProvidePositionToServer(playerTransform.position);
            lastPlayerPosition = playerTransform.position;
        }
        if(isLocalPlayer)
        {
            if(Quaternion.Angle(playerTransform.rotation,lastPlayerRotation) > thresholdRotation)
            {
                CmdProvideRotationToServer(playerTransform.rotation);
                lastPlayerRotation = playerTransform.rotation;
            }

        }
   
    }
}
