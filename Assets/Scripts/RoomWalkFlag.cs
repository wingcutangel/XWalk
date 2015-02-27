﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomWalkFlag : MonoBehaviour {
	public RectTransform flag;
	public bool returningAnim = false;
	public GameObject POVCamera;
	Button theButton;
	CanvasScaler theCanvasScaler;
	AnimationState anim;
	// Use this for initialization
	void Start () {
		theCanvasScaler = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<CanvasScaler>() as CanvasScaler;
		string animName = POVCamera.GetComponent<Animation>().clip.name;
		anim = POVCamera.GetComponent<Animation>()[animName];
		theButton = flag.GetComponent<Button> ();
		theButton.onClick.AddListener(() => {anim.speed = 1; transform.parent.parent.GetComponentInChildren<Animation> ().Play();});

	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint (transform.position);
			float scaleFactorX = theCanvasScaler.referenceResolution.x / Screen.width;
			float scaleFactorY = theCanvasScaler.referenceResolution.y / Screen.height;
			flag.anchoredPosition = new Vector2(screenPos.x * scaleFactorX, screenPos.y * scaleFactorX);
		}
	}
}