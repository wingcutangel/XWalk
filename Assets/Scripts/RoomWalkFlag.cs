using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomWalkFlag : MonoBehaviour {
//	public RectTransform flag;
//	public bool returningAnim = false;
//	public GameObject POVCamera;
	Button theButton;
	CanvasScaler theCanvasScaler;
	Animation anim;
	AnimationState animState;
	// Use this for initialization
	void Start () {
//		theCanvasScaler = GameObject.FindGameObjectWithTag ("MainCanvas").GetComponent<CanvasScaler>() as CanvasScaler;
		string animName = GetComponent<Animation>().clip.name;
		anim = GetComponent<Animation>();
		animState = GetComponent<Animation>()[animName];
//		theButton = flag.GetComponent<Button> ();
//		theButton.onClick.AddListener(() => {animState.speed = 1; anim.Play();});

	}
	
	// Update is called once per frame
	void Update () {
//		if (Camera.main) {
//			Vector3 screenPos = Camera.main.WorldToScreenPoint (transform.position);
//			float scaleFactorX = theCanvasScaler.referenceResolution.x / Screen.width;
//			float scaleFactorY = theCanvasScaler.referenceResolution.y / Screen.height;
//			flag.anchoredPosition = new Vector2(screenPos.x * scaleFactorX, screenPos.y * scaleFactorX);
//		}
		if(Input.GetKeyUp(KeyCode.W)){
			playForward();
		}
		if(Input.GetKeyUp(KeyCode.S)){
				playBackwards();
		}
	}

	private void playBackwards(){
		animState.speed = -1; 
		if (!anim.isPlaying){
			animState.time = 1;
			anim.Play();
		}
	}

	private void playForward(){
		animState.speed = 1;

		if (!anim.isPlaying){
			animState.time = 0f;
			anim.Play();
		}
	}
}
