using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasGroupState : MonoBehaviour {
	CanvasGroup cg;
	public float inDuration = .5f;
	public float outDuration = .5f;
	// Use this for initialization
	void Start () {
		cg = GetComponent<CanvasGroup> ();

	}
	
	public void enable(){
//		gameObject.SetActive (true);

		StopCoroutine ("lerpUp");
		StopCoroutine ("lerpDown");
		StartCoroutine ("lerpUp");
	}

	public void disable(){
		cg.blocksRaycasts = false;
		cg.interactable = false;
		StopCoroutine ("lerpUp");
		StopCoroutine ("lerpDown");
		StartCoroutine ("lerpDown");
	}
	private IEnumerator lerpUp()
	{
		float from = cg.alpha;
		float to = 1f;
		float startTime = Time.time;
		while (Time.time <= startTime + inDuration){
			cg.alpha = Mathf.SmoothStep(from, to, (Time.time - startTime)/inDuration);
			yield return null;
		}
		cg.alpha = to;
		cg.blocksRaycasts = true;
		cg.interactable = true;
	}

	private IEnumerator lerpDown(){
		float from = cg.alpha;
		float to = 0f;
		float startTime = Time.time;
		while (Time.time <= startTime + outDuration){
			cg.alpha = Mathf.SmoothStep(from, to, (Time.time - startTime)/outDuration);
			yield return null;
		}
		cg.alpha = to;
//		gameObject.SetActive (false);
	}
}
