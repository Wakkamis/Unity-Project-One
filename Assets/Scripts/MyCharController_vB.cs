using UnityEngine;
using System.Collections;

public class MyCharController_vB : MonoBehaviour {

	//player variables
	private float playerMaxSpeed = 30;
	private float playerSpeed;
	private float playerRotateSpeed = .25f;
	private Rigidbody playerRigidBody;
	private Quaternion oldPlayerRotation;
	private Quaternion newPlayerRotation;
	private float playerToRotateTo;
	public float playerJumpForce = 600;

	//camera variables
	private Camera cam3rdPerson;
	private GameObject cam3rdPersonObj;
	private	GameObject cam3rdPersonTarget;
	private float camXDeg;
	private float camYDeg;
	private Quaternion camFromRotation;
	private Quaternion camToRotation;
	public float camRotateSpeed = 5f; 
	public float camLerpSpeed = 50f; 
	public float camXFriction = 1f; 
	public float camYFriction = .5f;
	private int camZoomLevel;
	public int camZoomSpeed = 2;
    private GameObject[] allCameras;

	//system Variables
	private bool spacePressed = false;

	// Use this for initialization
	void Start () 
	{
		//player setup
		playerRigidBody = this.GetComponent<Rigidbody> ();
		playerSpeed = playerMaxSpeed;

		//cam setup
		cam3rdPersonObj = GameObject.Find ("camera 3rd person");
		cam3rdPerson = cam3rdPersonObj.GetComponent<Camera> ();
		cam3rdPersonTarget = GameObject.Find ("camera 3rd Person Target");
        enableMyCam ();
	}

	void enableMyCam()
	{
		allCameras = GameObject.FindGameObjectsWithTag("MainCamera");
		for (int i = 0; i < allCameras.Length; i++)
		{
			allCameras[i].GetComponent<Camera>().enabled = false;
		}
		cam3rdPerson.enabled = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputPlayerMovement();
		cameraActions ();
	}

	void InputPlayerMovement()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			playerRigidBody.MovePosition (playerRigidBody.position + transform.forward * playerSpeed * Time.deltaTime);
			rotatePlayer ();
		}
		if (Input.GetKeyUp (KeyCode.W)) 
		{
			playerSpeed = playerMaxSpeed;
		}
		if (Input.GetKey (KeyCode.S)) 
		{
			playerRigidBody.MovePosition (playerRigidBody.position - transform.forward * playerSpeed * Time.deltaTime);
			rotatePlayer ();
		}
		if (Input.GetKeyUp (KeyCode.S)) 
		{
			playerSpeed = playerMaxSpeed;
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y + 90;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MovePosition (playerRigidBody.position + transform.forward * playerSpeed * Time.deltaTime);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
		}
		if (Input.GetKeyUp (KeyCode.D)) 
		{
			playerSpeed = playerMaxSpeed;
		}
		if (Input.GetKey (KeyCode.A)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y - 90;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MovePosition (playerRigidBody.position + transform.forward * playerSpeed * Time.deltaTime);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
		}
		if (Input.GetKeyUp (KeyCode.A)) 
		{
			playerSpeed = playerMaxSpeed;
		}
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.A)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y - 45;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
			playerSpeed = playerMaxSpeed / 2;
		}
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.D)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y + 45;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
			playerSpeed = playerMaxSpeed / 2;

		}
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y - 45;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
			playerSpeed = playerMaxSpeed / 2;
			playerRigidBody.MovePosition (playerRigidBody.position - transform.forward * playerSpeed * Time.deltaTime);
			
		}
		if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A)) 
		{
			oldPlayerRotation = playerRigidBody.transform.rotation;
			playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y + 45;
			newPlayerRotation = Quaternion.Euler (0, playerToRotateTo, 0);
			playerRigidBody.MoveRotation (Quaternion.Slerp (oldPlayerRotation, newPlayerRotation, playerRotateSpeed));
			playerSpeed = playerMaxSpeed / 2;
			playerRigidBody.MovePosition (playerRigidBody.position - transform.forward * playerSpeed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.Space)) 
		{
			if (spacePressed == false) 
			{
				spacePressed = true;
				this.GetComponent<Rigidbody> ().AddForce (transform.up * playerJumpForce, ForceMode.Force);
			}
		}
		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			spacePressed = false;
		}
		if (Input.GetKey (KeyCode.LeftShift)) 
		{
			playerSpeed = 60;
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) 
		{
			playerSpeed = playerMaxSpeed;
		}
		if (Input.GetMouseButton(0))
		{
		}
		if (Input.GetMouseButton(1))
		{
			rotatePlayer();
		}
		if (Input.GetKey (KeyCode.R)) 
		{
			this.transform.position = new Vector3(10,1.25f,-40);
			this.transform.rotation = Quaternion.Euler(0,0,0);
		}
        //quit application
		if (Input.GetKey (KeyCode.Escape)) 
		{
			Application.Quit();
		}
	}

	void rotatePlayer()
	{
		oldPlayerRotation = playerRigidBody.rotation;
		playerToRotateTo = cam3rdPersonTarget.transform.eulerAngles.y;
		newPlayerRotation = Quaternion.Euler(0,playerToRotateTo,0);
		playerRigidBody.MoveRotation (Quaternion.Slerp(oldPlayerRotation,newPlayerRotation, playerRotateSpeed));
		
	}

	void cameraActions()
	{
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
		zoomCamLevel();
		//look at target
		cam3rdPerson.transform.LookAt(cam3rdPersonTarget.transform);
		//rotate
		camXDeg += Input.GetAxis("Mouse X") * camRotateSpeed * camXFriction;
		camYDeg -= Input.GetAxis("Mouse Y") * camRotateSpeed * camYFriction;
		camFromRotation = cam3rdPersonTarget.transform.rotation;
		camToRotation = Quaternion.Euler(camYDeg,camXDeg,0);
		cam3rdPersonTarget.transform.rotation = Quaternion.Lerp(camFromRotation,camToRotation,Time.deltaTime  * camLerpSpeed);
		//cap horizontal rotation
		checkLimits ();
		
	}
	
	void checkLimits()
	{	
		if (camYDeg > 23) 
		{
			camYDeg = 23;	
		}
		else if(camYDeg < -25) 
		{	
			camYDeg = -25;	
		}	
	}
	
	void zoomCamLevel(){
		
		if ((camZoomLevel >= 0) && (camZoomLevel <= 10)) {
			
			zoomCam ();
			
		} 
		else if (camZoomLevel <= -1) {
			
			zoomInCam ();
			
		}
		else if (camZoomLevel >= 11) {
			
			zoomOutCam ();
			
		}
		
	}
	
	void zoomCam (){
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			cam3rdPerson.transform.position -= cam3rdPerson.transform.TransformDirection(Vector3.back)*camZoomSpeed;
			camZoomLevel -= 1;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			cam3rdPerson.transform.position += cam3rdPerson.transform.TransformDirection(Vector3.back)*camZoomSpeed;
			camZoomLevel += 1;
			
		}
		
	}
	
	void zoomOutCam(){
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			cam3rdPerson.transform.position -= cam3rdPerson.transform.TransformDirection(Vector3.back)*camZoomSpeed;
			camZoomLevel -= 1;
		}
		
	}
	
	void zoomInCam(){
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			cam3rdPerson.transform.position += cam3rdPerson.transform.TransformDirection(Vector3.back)*camZoomSpeed;
			camZoomLevel += 1;
			
		}
		
	}
}
