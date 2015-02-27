using UnityEngine;
using System.Collections;

public class GUIPanelScript : MonoBehaviour {
	private bool cameraReduced = false;
	public RectTransform infoPanel;
	public CanvasGroupState[] stateControllers;
	private int activePanelGroup = -1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void toggleCameraSize(){
		cameraReduced = !cameraReduced;
		StopCoroutine ("shrinkCamera");
		if (cameraReduced) {
			StartCoroutine ("shrinkCamera", true);
		} else {
			StartCoroutine ("shrinkCamera", false);
		}

	}

	private IEnumerator shrinkCamera(bool shrink){
		float cameraFrom;
		float cameraTo;
		float panelFrom;
		float panelTo;
		float duration = 0.5f;
		if (shrink) {
			cameraFrom = 1f;
			cameraTo = .7f;
			panelFrom = 576f;
			panelTo = 0f;
		} else {
			cameraFrom = .7f;
			cameraTo = 1f;
			panelFrom = 0f;
			panelTo = 576f;
		}
		Rect oldRect = Camera.main.rect;
		Vector2 oldPanelPos = infoPanel.anchoredPosition;
		float startTime = Time.time;
		while (Time.time <= startTime + duration){
			oldRect.width = Mathf.Lerp(cameraFrom, cameraTo, (Time.time - startTime)/duration);
			Camera.main.rect = oldRect;
			oldPanelPos.x = Mathf.Lerp (panelFrom, panelTo, (Time.time - startTime)/duration);
			infoPanel.anchoredPosition = oldPanelPos;
			yield return null;
		}
		oldRect.width = cameraTo;
		Camera.main.rect = oldRect;
		oldPanelPos.x = panelTo;
		infoPanel.anchoredPosition = oldPanelPos;
	}

	public void switchToPinGroup(int group){
		if (group != activePanelGroup){
			if (activePanelGroup > -1)
			{
				stateControllers[activePanelGroup].disable();
			}
			activePanelGroup = group;
			if (activePanelGroup > -1)
			{
				stateControllers[group].enable();
			}
		}
		if (group != -1) {
			GetComponent<InterfaceController>().lastPinGroup = group;
		}
	}
}
