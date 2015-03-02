using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {
	public Camera mainCam;
	public Camera execCam;
	public Animator screenFader;
	public GameObject WalkCamPrefab;
	public GameObject walkControls;
	private bool inExec = false;
	private GameObject walkCam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private IEnumerator switchCams(){
		screenFader.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1f);
		mainCam.gameObject.SetActive(!mainCam.gameObject.activeSelf);
//		execCam.gameObject.SetActive(!execCam.gameObject.activeSelf);
		walkControls.SetActive (!walkControls.activeSelf);
		if (walkCam) {
			Destroy (walkCam);
		} else {
			walkCam = Instantiate(WalkCamPrefab, execCam.transform.position, execCam.transform.rotation) as GameObject;
		}
		screenFader.SetTrigger("FadeIn");
	}
	public void mainView(){
		if (inExec){
			StopCoroutine("switchCams");
			StartCoroutine("switchCams");
			inExec = false;
		}
	}
	public void execView(){
		StopCoroutine("switchCams");
		StartCoroutine("switchCams");
		inExec = true;
	}
}
