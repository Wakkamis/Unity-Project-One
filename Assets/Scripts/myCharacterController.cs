using UnityEngine;
using System.Collections;

public class myCharacterController : MonoBehaviour {

	public float characterSpeed; //.25
	public float cameraRotateSpeed; //5
	public float xFriction; //1
	public float yFriction; //.5
	public float lerpSpeed; //50
	public float jumpForce; //600

	private float xDeg;
	private float yDeg;
	private Quaternion fromRotation;
	private Quaternion toRotation;
	private Quaternion oldCharacterRotation;
	private float toRotateTo;
	private Quaternion newCharacterRotation;
	private Vector3 startPosition;
	private Quaternion startRotation;

	GameObject playerCharacter;
	Rigidbody playerRB;
	Camera camera3RD;
	GameObject cam3RDObj;
	GameObject camera3RDTarget;
	private int zoomLevel;
	public int zoomSpeed = 2;

	Animation PCAnimation;

	//bool wPressed = false;
	//bool sPressed = false;
	//bool heroCanRotate = true;
	bool spacePressed = false;

	// Use this for initialization
	void Start () 
	{
		playerCharacter = GameObject.Find ("Box_Hero");
		playerRB = this.GetComponent<Rigidbody> ();
		PCAnimation = playerCharacter.GetComponent<Animation> ();
		cam3RDObj = GameObject.Find ("camera 3rd person");
		camera3RD = cam3RDObj.GetComponent<Camera> ();
		camera3RDTarget = GameObject.Find ("camera 3rd Person Target");
		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		characterMovement ();
		cameraActions ();
	}

	void characterMovement()
	{
		if (Input.GetKey ("escape")) 
		{
			Application.Quit();
		}
		if (Input.GetKey ("r")) 
		{
			this.transform.position = new Vector3(10,1.25f,-40);
			this.transform.rotation = Quaternion.Euler(0,0,0);
		}
		if (Input.GetMouseButton(0))
		{
			PCAnimation.CrossFade ("Attack", .1f);
		}
		if (Input.GetMouseButtonUp(0))
		{
			PCAnimation.CrossFade ("Idle", 3f);
		}
		if (Input.GetMouseButton(1))
		{
			PCAnimation.CrossFade ("Block", .1f);
		}
		if (Input.GetMouseButtonUp(1))
		{
			PCAnimation.CrossFade ("Idle", .1f);
		}
		if (Input.GetKey ("w") && Input.GetMouseButton(1)) 
		{
			PCAnimation.CrossFade ("Walking Block", .1f);
		}
		if (Input.GetKeyUp ("w") && Input.GetMouseButtonUp(1)) 
		{
			PCAnimation.CrossFade ("Idle", .1f);
		}
		if (Input.GetKey ("w")) 
		{
			playerRB.MovePosition(playerRB.position + Vector3.forward * characterSpeed);
			//this.transform.position += transform.forward * characterSpeed;
			PCAnimation.CrossFade ("Walk", .1f);
			rotateCharacter ();
		}
		if (Input.GetKeyUp ("w")) 
		{
			print ("let go");
			//wPressed = false;
			//heroCanRotate = true;
			PCAnimation.CrossFade ("Idle", .25f);
		}
		if (Input.GetKey ("s")) 
		{
			playerRB.MovePosition(playerRB.position - Vector3.forward * characterSpeed);
			//this.transform.position -= transform.forward * characterSpeed;
			PCAnimation.CrossFade ("Walk", .1f);
			rotateCharacter ();
		}
		if (Input.GetKeyUp ("s")) 
		{
			//sPressed = false;
			PCAnimation.CrossFade ("Idle", .25f);
		}
		if (Input.GetKey ("a")) 
		{
			toRotateTo = camera3RDTarget.transform.eulerAngles.y-90;
			newCharacterRotation = Quaternion.Euler(0,toRotateTo,0);
			PCAnimation.CrossFade ("Walk", .1f);
			this.transform.position += transform.forward * characterSpeed;
			StraifCharacter();
		}
		if (Input.GetKeyUp ("a")) 
		{
			toRotateTo = camera3RDTarget.transform.eulerAngles.y;
			newCharacterRotation = Quaternion.Euler(0,toRotateTo,0);
			PCAnimation.CrossFade ("Idle", .25f);
			StraifCharacter();
		}
		if (Input.GetKey ("d")) 
		{
			toRotateTo = camera3RDTarget.transform.eulerAngles.y+90;
			newCharacterRotation = Quaternion.Euler(0,toRotateTo,0);
			PCAnimation.CrossFade ("Walk", .1f);
			this.transform.position += transform.forward * characterSpeed;
			StraifCharacter();
		}
		if (Input.GetKeyUp ("d")) 
		{
			toRotateTo = camera3RDTarget.transform.eulerAngles.y;
			newCharacterRotation = Quaternion.Euler(0,toRotateTo,0);
			PCAnimation.CrossFade ("Idle", .25f);
			StraifCharacter();
		}
		if (Input.GetKey ("space")) 
		{
			PCAnimation.CrossFade ("Jump", .05f);
			jumpCharacter ();
		}
		if (Input.GetKeyUp ("space")) 
		{
			spacePressed = false;
			PCAnimation.CrossFade ("Idle", 1);
		}
		if (Input.GetMouseButton (1)) 
		{
			rotateCharacter ();
		}
	}

	void jumpCharacter()
	{
		if (spacePressed == false) 
		{
			spacePressed = true;
			this.GetComponent<Rigidbody> ().AddForce (transform.up * jumpForce, ForceMode.Force);
		}
	}

	void rotateCharacter()
	{
		oldCharacterRotation = this.transform.rotation;
		toRotateTo = camera3RDTarget.transform.eulerAngles.y;
		//newCharacterRotation = Quaternion.Euler(0,toRotateTo,0);
		this.transform.rotation = Quaternion.Slerp(oldCharacterRotation,newCharacterRotation, .75f);
		//newCharacterRotation = Quaternion.AngleAxis(toRotateTo, transform.up);
		playerRB.MoveRotation (newCharacterRotation*transform.rotation);

	}

	void StraifCharacter()
	{
		oldCharacterRotation = this.transform.rotation;
		this.transform.rotation = Quaternion.Slerp(oldCharacterRotation,newCharacterRotation, .25f);
	}

	void cameraActions()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		zoomCamLevel();
		//look at target
		camera3RD.transform.LookAt(camera3RDTarget.transform);
		//rotate
		xDeg += Input.GetAxis("Mouse X") * cameraRotateSpeed * xFriction;
		yDeg -= Input.GetAxis("Mouse Y") * cameraRotateSpeed * yFriction;
		fromRotation = camera3RDTarget.transform.rotation;
		toRotation = Quaternion.Euler(yDeg,xDeg,0);
		camera3RDTarget.transform.rotation = Quaternion.Lerp(fromRotation,toRotation,Time.deltaTime  * lerpSpeed);
		//cap horizontal rotation
		checkLimits ();

	}

	void checkLimits()
	{	
		if (yDeg > 23) 
		{
			yDeg = 23;	
		}
		else if(yDeg < -25) 
		{	
			yDeg = -25;	
		}	
	}

	void zoomCamLevel(){
		
		if ((zoomLevel >= 0) && (zoomLevel <= 10)) {
			
			zoomCam ();
			
		} 
		else if (zoomLevel <= -1) {
			
			zoomInCam ();
			
		}
		else if (zoomLevel >= 11) {
			
			zoomOutCam ();
			
		}
		
	}
	
	void zoomCam (){
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			camera3RD.transform.position -= camera3RD.transform.TransformDirection(Vector3.back)*zoomSpeed;
			zoomLevel -= 1;
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			camera3RD.transform.position += camera3RD.transform.TransformDirection(Vector3.back)*zoomSpeed;
			zoomLevel += 1;
			
		}
		
	}
	
	void zoomOutCam(){
		
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
		{
			camera3RD.transform.position -= camera3RD.transform.TransformDirection(Vector3.back)*zoomSpeed;
			zoomLevel -= 1;
		}
		
	}
	
	void zoomInCam(){
		
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
		{
			camera3RD.transform.position += camera3RD.transform.TransformDirection(Vector3.back)*zoomSpeed;
			zoomLevel += 1;
			
		}
		
	}
}
