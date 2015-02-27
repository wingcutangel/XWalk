using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomTypeFlag : MonoBehaviour {
	public RectTransform flag;
	public GameObject walkIcons;
	CameraPoint camPoint;
	GameObject povCamera;
	CameraSwitch camSwitch;
	Button FOVButton;
	CanvasScaler theCanvasScaler;
	// Use this for initialization
	void Start () {
		camPoint = GetComponentInChildren<CameraPoint> ();
		povCamera = GetComponentInChildren<Camera> ().gameObject;
		povCamera.gameObject.SetActive (false);
		camSwitch = GameObject.FindGameObjectWithTag ("CameraSwitcher").GetComponent<CameraSwitch> ();
		Button theButton = flag.gameObject.GetComponent<Button> (); 
		theButton.onClick.AddListener (focus);
		theCanvasScaler = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<CanvasScaler> () as CanvasScaler;
		if (walkIcons){
			walkIcons.SetActive (false);
		}
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

	public void focus(){
		Camera.main.GetComponent<OrbitCamera> ().switchFocusPoint (camPoint);
		camSwitch.execCam = povCamera.GetComponent<Camera> ();
		if (walkIcons) {
			walkIcons.SetActive (true);
		}
	}
}
