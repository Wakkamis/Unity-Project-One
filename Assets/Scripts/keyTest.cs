using UnityEngine;
using System.Collections;

public class keyTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey("w"))
			Debug.Log("Pressing w");
		if(Input.GetKey("s"))
			Debug.Log("Pressing s");
		if(Input.GetKey("a"))
			Debug.Log("Pressing a");
		if(Input.GetKey("d"))
			Debug.Log("Pressing d");
		if(Input.GetKey("w") && Input.GetKey("a"))
			Debug.Log("Pressing w & a");
	}
}
