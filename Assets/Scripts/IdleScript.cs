using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class IdleScript : MonoBehaviour {
	float elapsedTime = 0f;
	public float idleLimit = 5f;
	private Action updateTimer;
	CanvasGroupState cgs;
	// Use this for initialization
	void Start () {
		cgs = GetComponent<CanvasGroupState> ();
		updateTimer += countTime;
	}
	
	// Update is called once per frame
	void Update () {
		updateTimer();
	}

	private IEnumerator showAbout(bool show){
		Image theImage = GetComponent<Image>();
		float startTime = Time.time;
		float startValue = theImage.color.a;
		float endValue;
		if (show){
			endValue = 1f;
		} else
		{
			endValue = 0f;
		}
		Color tempColor = theImage.color;
		while(Time.time <=startTime + .2f){
			tempColor.a = Mathf.Lerp(startValue, endValue, (Time.time - startTime)/.2f);
			theImage.color = tempColor;
			yield return null;
		}
		tempColor.a = endValue;
		theImage.color = tempColor;
	}

	private void countTime(){
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= idleLimit){
			openAbout();
		}
		if (Input.GetButtonDown("Fire1") || Input.touchCount > 0){
			elapsedTime = 0f;
		}
	}

	public void openAbout(){
		cgs.enable ();
		StartCoroutine (Camera.main.GetComponent<OrbitCamera> ().autoOrbit (3f));
		updateTimer -= countTime;
		updateTimer += closeAbout;
	}

	private void closeAbout(){
		if (Input.GetButtonDown("Fire1") || Input.touchCount > 0){
			cgs.disable();
			updateTimer -= closeAbout;
			elapsedTime = 0f;
			updateTimer += countTime;
		}
	}
}
