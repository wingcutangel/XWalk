using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {
	public Camera mainCam;
	public Camera execCam;
	public Animator screenFader;

	private bool inExec = false;
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
		execCam.gameObject.SetActive(!execCam.gameObject.activeSelf);
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
