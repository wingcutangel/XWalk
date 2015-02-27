using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InterfaceController : MonoBehaviour {
	public float sidePanelOutTime = 0.5f;
	public RectTransform sidePanel;
	public Button menuButton;
	private bool sidePanelVisible = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
			endPosition = 259f;
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
}
