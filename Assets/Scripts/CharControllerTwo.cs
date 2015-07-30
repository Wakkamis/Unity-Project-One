using UnityEngine;
using System.Collections;

public class CharControllerTwo : MonoBehaviour {

	public float characterSpeed = 10f;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	Rigidbody playerChar;

	// Use this for initialization
	void Start () 
	{
		playerChar = this.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (this.GetComponent<NetworkView>().isMine)
		{
			InputMovement();
			InputColorChange();
		} else
		{
			SyncedMovement();
		}
	}
	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W))
			playerChar.MovePosition(playerChar.position + Vector3.forward * characterSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.S))
			playerChar.MovePosition(playerChar.position - Vector3.forward * characterSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.D))
			playerChar.MovePosition(playerChar.position + Vector3.right * characterSpeed * Time.deltaTime);
		
		if (Input.GetKey(KeyCode.A))
			playerChar.MovePosition(playerChar.position - Vector3.right * characterSpeed * Time.deltaTime);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = playerChar.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = playerChar.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = playerChar.position;
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		playerChar.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}
	
	[RPC] void ChangeColorTo(Vector3 color)
	{
		this.GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z, 1f);
		
		if (this.GetComponent<NetworkView>().isMine)
			this.GetComponent<NetworkView>().RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
	}
}
