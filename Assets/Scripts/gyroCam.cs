using UnityEngine;
using System.Collections;

public class gyroCam : MonoBehaviour {

	// iPhone gyroscope-controlled camera demo v0.3 8/8/11
	// Perry Hoberman <hoberman@bway.net>
	// Directions: Attach this script to main camera.
	// Note: Unity Remote does not currently support gyroscope. 
	
	private bool gyroBool;
	private Gyroscope gyro;
	private Quaternion rotFix;
	private Quaternion camRot;
	private GameObject camParent;
	void Start() {
		
		var originalParent = transform.parent; // check if this transform has a parent
		camParent = new GameObject ("camParent"); // make a new parent
		camParent.transform.position = transform.position; // move the new parent to this transform position
		transform.parent = camParent.transform; // make this transform a child of the new parent
		camParent.transform.parent = originalParent; // make the new parent a child of the original parent
		
		gyroBool = SystemInfo.supportsGyroscope;
		
		if (gyroBool) {
			
			gyro = Input.gyro;
			gyro.enabled = true;
			
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				camParent.transform.eulerAngles = new Vector3(90,180,0);
			} else if (Screen.orientation == ScreenOrientation.Portrait) {
				camParent.transform.eulerAngles = new Vector3(90,180,0);
			}
			
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				rotFix = new Quaternion(0f,0f,1f,0f);
			} else if (Screen.orientation == ScreenOrientation.Portrait) {
				rotFix = new Quaternion(0f,0f,1f,0f);
			}
			//Screen.sleepTimeout = 0;
		} else {
			print("NO GYRO");
		}
	}
	
	void Update () {
		if (gyroBool) {
			camRot = gyro.attitude * rotFix;
			transform.localRotation = camRot;
		}	
	}

	void OnDisable() {
		gyro.enabled = false;
		Destroy (camParent);
	}
}
