using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceController : MonoBehaviour {
	public float sidePanelOutTime = 0.5f;
	public float infoPanelOutTime = 0.5f;
	public RectTransform sidePanel;
	public RectTransform infoPanel;
	public Button menuButton;
	public OrbitCamera mainCamera;
	public CameraPoint mapView;
	public int lastPinGroup;
	public CanvasGroupState zoomedInButtons;
	public CanvasGroupState walkButtons;
	public CanvasGroupState zoomedInButtonsWOInfo;
	public CanvasGroupState walkButtonsWOInfo;
	public GameObject EOInfo;
	public GameObject OSInfo;

	public GoogleAnalyticsV3 googleAnalytics;

	void Start(){
//		Screen.SetResolution(1280, 800, true);
		Screen.SetResolution(1920, 1080, true);
		Application.targetFrameRate = 30;
	}
	// Use this for initialization
	public void returnToMenu(){
		mainCamera.switchFocusPoint (mapView);
		slidePanelOut (); 
		GetComponent<GUIPanelScript> ().switchToPinGroup (lastPinGroup);
		zoomedInButtons.disable ();
		zoomedInButtonsWOInfo.disable ();
	}

	public void showZoomedInButtons(){
		walkButtons.disable ();
		walkButtonsWOInfo.disable ();
		zoomedInButtons.enable ();
		slidePanelIn ();
		GetComponent<GUIPanelScript> ().switchToPinGroup (-1);
	}
	public void showWalkButtons(){
		googleAnalytics.LogEvent("XWalk", "Interface", "Walk", 1);
		zoomedInButtons.disable ();
		zoomedInButtonsWOInfo.disable ();
		walkButtons.enable ();
		slidePanelIn ();
		GetComponent<GUIPanelScript> ().switchToPinGroup (-1);
	}
	public void showZoomedInButtonsWOInfo(){
		googleAnalytics.LogEvent("XWalk", "Interface", "Walk", 1);
		walkButtons.disable ();
		walkButtonsWOInfo.disable ();
		zoomedInButtonsWOInfo.enable ();
		slidePanelIn ();
		GetComponent<GUIPanelScript> ().switchToPinGroup (-1);
	}
	public void showWalkButtonsWOInfo(){
		zoomedInButtons.disable ();
		zoomedInButtonsWOInfo.disable ();
		walkButtonsWOInfo.enable ();
		slidePanelIn ();
		GetComponent<GUIPanelScript> ().switchToPinGroup (-1);
	}
	public void slidePanelOut(){
		StopCoroutine("IEslidePanelOut");
		StartCoroutine("IEslidePanelOut", true);
		menuButton.onClick.RemoveAllListeners();
		menuButton.onClick.AddListener(slidePanelIn);
	}

	public void slidePanelIn(){
		StopCoroutine("IEslidePanelOut");
		StartCoroutine("IEslidePanelOut", false);
		menuButton.onClick.RemoveAllListeners();
		menuButton.onClick.AddListener(slidePanelOut);
	}

	IEnumerator IEslidePanelOut(bool direction){
		float startTime = Time.time;
		float startPosition;
		float endPosition;
		Rect tempRect = Camera.main.rect;
		float endX;
		float startX = tempRect.x;
		if (direction){
			startPosition = sidePanel.anchoredPosition.x;
			endPosition = 276.8f;
			endX = endPosition/GetComponent<CanvasScaler>().referenceResolution.x;
		} else
		{
			startPosition = sidePanel.anchoredPosition.x;
			endPosition = 0f;
			endX = 0f;
		}

		Vector2 newPos = sidePanel.anchoredPosition;
		while (Time.time <= startTime + sidePanelOutTime){
			float factor = (Time.time - startTime)/sidePanelOutTime;
			newPos.x = Mathf.SmoothStep(startPosition, endPosition, factor);
			sidePanel.anchoredPosition = newPos;
			tempRect.x = Mathf.SmoothStep(startX, endX, factor);
			Camera.main.rect = tempRect;
			yield return null;
		}
		newPos.x = endPosition;
		sidePanel.anchoredPosition = newPos;
		tempRect.x = endX;
		Camera.main.rect = tempRect;
	}

	public void infoPanelOut(){
		googleAnalytics.LogEvent("XWalk", "Interface", "Info Panel", 1);

		StopCoroutine("IEinfoPanelOut");
		StartCoroutine("IEinfoPanelOut", true);
	}
	
	public void infoPanelIn(){
		StopCoroutine("IEinfoPanelOut");
		StartCoroutine("IEinfoPanelOut", false);
	}

	IEnumerator IEinfoPanelOut(bool direction){
		float startTime = Time.time;
		float startPosition;
		float endPosition;

		if (direction){
			startPosition = infoPanel.anchoredPosition.x;
			endPosition = 467.9f;
		} else
		{
			startPosition = infoPanel.anchoredPosition.x;
			endPosition = 0f;
		}
		
		Vector2 newPos = infoPanel.anchoredPosition;
		while (Time.time <= startTime + infoPanelOutTime){
			float factor = (Time.time - startTime)/infoPanelOutTime;
			newPos.x = Mathf.SmoothStep(startPosition, endPosition, factor);
			infoPanel.anchoredPosition = newPos;
			yield return null;
		}
		newPos.x = endPosition;
		infoPanel.anchoredPosition = newPos;
	}

	public void switchToOSInfo(){
		OSInfo.SetActive (true);
		EOInfo.SetActive (false);
	}

	public void switchToEOInfo(){
		OSInfo.SetActive (false);
		EOInfo.SetActive (true);
	}
}
