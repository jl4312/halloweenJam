using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	private Camera mainCamera;
	// Use this for initialization
	void Awake () {
		mainCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();

	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (transform.position + mainCamera.transform.rotation * Vector3.back, mainCamera.transform.rotation * Vector3.up);
	}
}
