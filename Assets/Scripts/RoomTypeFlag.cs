﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomTypeFlag : MonoBehaviour {
	public RectTransform flag;
	CameraPoint camPoint;
	GameObject povCamera;
	CameraSwitch camSwitch;
	Button FOVButton;
	CanvasScaler theCanvasScaler;
	Vector2 tempVector;
	private float scaleFactorX;
	// Use this for initialization
	void Start () {
		tempVector = new Vector2 ();
		camPoint = GetComponentInChildren<CameraPoint> ();
		povCamera = transform.FindChild("POVCamera").gameObject;
		povCamera.gameObject.SetActive (false);
		camSwitch = GameObject.FindGameObjectWithTag ("CameraSwitcher").GetComponent<CameraSwitch> ();
		Button theButton = flag.gameObject.GetComponent<Button> (); 
		theButton.onClick.AddListener (focus);
		theCanvasScaler = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<CanvasScaler> () as CanvasScaler;
//		scaleFactorX = theCanvasScaler.referenceResolution.x / 1280f;//Screen.width;
		scaleFactorX = theCanvasScaler.referenceResolution.x / 1920f;//Screen.width;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Camera.main) {

			Vector3 screenPos = Camera.main.WorldToScreenPoint (transform.position);
//			float scaleFactorY = theCanvasScaler.referenceResolution.y / Screen.height;
			tempVector.x = screenPos.x * scaleFactorX;
			tempVector.y = screenPos.y * scaleFactorX;
			flag.anchoredPosition = tempVector;
		}
	}

	public void focus(){
		Camera.main.GetComponent<OrbitCamera> ().switchFocusPoint (camPoint);
		camSwitch.execCam = povCamera;
	}
}
