using UnityEngine;
using System.Collections;

public class SlideAnimation : MonoBehaviour {
	private Animation anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		anim["CameraSlide"].speed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow))
		{
			anim["CameraSlide"].time += Time.deltaTime/10f;
		}
		if(Input.GetKey(KeyCode.DownArrow))
		{
			anim["CameraSlide"].time -= Time.deltaTime/10f;
		}
	}
}
