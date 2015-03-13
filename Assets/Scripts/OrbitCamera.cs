﻿using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
	public GameObject _target;
	
	//The default distance of the camera from the target.
	public float _cameraRadius = 5f;
	public float _distance = 0f;
	public bool checkCollisions;
	//Control the speed of zooming and dezooming.
	public float _zoomStep = 1.0f;
	
	//The speed of the camera. Control how fast the camera will rotate.
	public float _xSpeed = 3f;
	public float _ySpeed = 3f;

	public float _lowAngle;
	public float _highAngle;
	public float _minDistance;
	public float _maxDistance;

	public LayerMask rayMask;

	//The position of the cursor on the screen. Used to rotate the camera.
	private float _x = 0.0f;
	private float _y = 0.0f;
	private float _xtarget = 0.0f;
	private float _ytarget = 0.0f;
	private float zoomTarget = 0.0f;
	private float _dx = 0.0f;
	private float _dy = 0.0f;
	private bool cameraFree = true;
	private bool bAutoOrbit;
	private bool hasGyro;	
	//Distance vector. 
	private Vector3 _distanceVector;
	private Vector3 _focusPosition;
	private Gyroscope gyro;

	/**
  * Move the camera to its initial position.
  */

	void Start ()
	{
		if (_target != null) {
			Vector2 angles = _target.transform.localEulerAngles;
			if(!_target.GetComponent<CameraPoint>())
			{
				_x = angles.y;
				_y = 0f;
			} else {
				CameraPoint cp = _target.GetComponent<CameraPoint>();
				_x = angles.y;
				_y = cp.startAngle;
				_distance = cp.startDistance;
			}
		} else {
			_target = new GameObject("CameraTarget");
			_x = 0f;
			_y = 0f;
		}
//		_distance = .75f * _maxDistance;
		_distanceVector = new Vector3(0.0f,0.0f,-_distance);
		_focusPosition = _target.transform.position;
		hasGyro = SystemInfo.supportsGyroscope;
		if (hasGyro) {
			Input.gyro.enabled = true;
			gyro = Input.gyro;
		}
		StartCoroutine(autoOrbit(3f));
	}

	void OnEnable() {
		if (hasGyro) {
			Input.gyro.enabled = true;
			gyro = Input.gyro;
		}
	}

	void OnDisable() {
		if (hasGyro) {
			Input.gyro.enabled = false;
		}
	}
	/**
  * Rotate the camera or zoom depending on the input of the player.
  */
	float touchDuration;
	Touch touch;

	void Update()
	{
		if (hasGyro) {
			Vector3 gyroRotationRate = gyro.rotationRateUnbiased;
			_xtarget += -gyroRotationRate.y * 0.2f;
			_ytarget += -gyroRotationRate.x * 0.15f;
		}
		RotateControls ();
		Zoom ();		
		Rotate ();
	}

	/**
  * Rotate the camera when the first button of the mouse is pressed.
  * 
  */
	void RotateControls()
	{
#if UNITY_ANDROID
		if (Input.touchCount == 1) {
			bAutoOrbit = false;

			_dx = Input.GetTouch (0).deltaPosition.x / 10 * _xSpeed * Time.deltaTime * 20;
			_dy = -Input.GetTouch (0).deltaPosition.y / 10 * _ySpeed * Time.deltaTime * 20;
		} 
#else
			if (Input.GetButton ("Fire1")) {
				bAutoOrbit = false;
				_dx = Input.GetAxis ("Mouse X") * _xSpeed;
				_dy = -Input.GetAxis ("Mouse Y") * _ySpeed;
		}
#endif
			_xtarget += _dx;
			_ytarget += _dy;
			_dx =_dy= 0f;

	}

	void Rotate()
	{
//		_distanceVector = new Vector3(0.0f,0.0f,-_distance);
		_distanceVector.z = -_distance;
//		if (cameraFree)
//		{
			float dTime = Time.deltaTime;
			_x = _x + (_xtarget * dTime * 4);
			_xtarget -= _xtarget * dTime * 4;
			if (_xtarget < 0.01f && _xtarget > -0.01f){
				_xtarget = 0f;
			}
			_y = _y + (_ytarget * dTime * 4);
			_y = Mathf.Clamp(_y, _lowAngle, _highAngle);
			_ytarget -= _ytarget * dTime * 4;
			if (_ytarget < 0.01f && _ytarget > -0.01f){
				_ytarget = 0f;
			}
			float newDistance = _distance + (zoomTarget * dTime * 2);
			zoomTarget -= zoomTarget * dTime * 2;
			if (newDistance + (zoomTarget * dTime) > _maxDistance || newDistance + (zoomTarget * dTime) < _minDistance){
				zoomTarget = 0f;
			}
			newDistance = Mathf.Clamp(newDistance, _minDistance, _maxDistance);

			_distance = newDistance;
		_distanceVector.z = -_distance;
			if (zoomTarget < 0.01f && zoomTarget > -0.01f){
				zoomTarget = 0f;
			}
//			if (checkCollisions){
//				Ray ray = new Ray(transform.position + (5*-transform.forward), transform.forward);
//				RaycastHit hit = new RaycastHit();
//				float collisionDistance = 0f;
//				if (Physics.Raycast(ray, out hit, 400f))
//				{
//					collisionDistance = (hit.point - _focusPosition).magnitude;
//				}
//				
//				if (_distanceVector.magnitude < (collisionDistance + _cameraRadius))
//				{
//					_distance = collisionDistance;
//					_distanceVector = new Vector3(0.0f,0.0f,-(_distance + _cameraRadius));
//				}
//			}
			//Transform angle in degree in quaternion form used by Unity for rotation.
			Quaternion rotation = Quaternion.Euler (_y, _x, 0.0f);
			//The new position is the target position + the distance vector of the camera
			//rotated at the specified angle.

			Vector3 position = rotation * _distanceVector + _focusPosition;
			
			//Update the rotation and position of the camera.
			transform.rotation = rotation;
			transform.position = position;

//		}
	}
	
	void Zoom()
	{
		if (cameraFree == true)
		{
#if UNITY_ANDROID
			if (Input.touchCount > 1)
			{
				Touch touchZero = Input.GetTouch(0);
				Touch touchOne = Input.GetTouch(1);
				
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
				
				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).sqrMagnitude;
				float touchDeltaMag = (touchZero.position - touchOne.position).sqrMagnitude;

				// Find the difference in the distances between each frame.
				float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

				if ( deltaMagnitudeDiff < 0.0f )
				{
					zoomTarget -= _zoomStep/3f * (_focusPosition - transform.position).sqrMagnitude / (100f) * Mathf.Abs (deltaMagnitudeDiff) * 20f * Time.deltaTime;
				}
				else if ( deltaMagnitudeDiff > 0.0f )
				{
					zoomTarget += _zoomStep/3f * (_focusPosition - transform.position).sqrMagnitude / (100f) * Mathf.Abs (deltaMagnitudeDiff) * 20f * Time.deltaTime;
				}
			}
#else
				if ( Input.GetAxis("Mouse ScrollWheel") > 0.0f )
				{
					zoomTarget -= _zoomStep * (_focusPosition - transform.position).magnitude / (100f);
				}
				else if ( Input.GetAxis("Mouse ScrollWheel") < 0.0f )
				{
					zoomTarget += _zoomStep * (_focusPosition - transform.position).magnitude / (100f);
				}
#endif
			}
			zoomTarget = Mathf.Clamp(zoomTarget , _minDistance - _distance, _maxDistance - _distance);
	}


	public IEnumerator autoOrbit(float speed){
		bAutoOrbit = true;
		cameraFree = false;
		while (bAutoOrbit) {
			_xtarget += speed * Time.deltaTime;
			yield return null;
		}
		cameraFree = true;
	}
	public void switchFocusPoint(GameObject newTarget){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newTarget.transform.position);
	}

	public void switchFocusPoint(Vector3 newTarget){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newTarget);
	}

	public void switchFocusPoint(CameraPoint newPoint){
		StopCoroutine("moveCameraFocus");
		StartCoroutine("moveCameraFocus", newPoint);
	}
	
	IEnumerator moveCameraFocus(Vector3 newPosition){

		Vector3 oldFocus = _focusPosition;
		Vector3 newFocus = newPosition;
		float startTime = Time.time;
		float travelTime = (newFocus-_focusPosition).magnitude / 10f;
		if (travelTime > 0.1f){
			while(Time.time <= startTime + travelTime){
				float factor = (Time.time - startTime)/travelTime;
				_focusPosition = Vector3.Lerp(oldFocus, newFocus, Mathf.SmoothStep(0f, 1f, factor));
				yield return null;
			}

			_focusPosition = newFocus;
//			_target = newTarget;
		}
	}

	IEnumerator moveCameraFocus(CameraPoint newPoint){
		_maxDistance = Mathf.Max(newPoint.maxDistance,_maxDistance);
		_minDistance = Mathf.Min(newPoint.minDistance, _minDistance);
		_highAngle = Mathf.Max(newPoint.maxAngle, _highAngle);
		_lowAngle = Mathf.Min(newPoint.minAngle, _lowAngle);
		Vector3 oldFocus = _focusPosition;
		Vector3 newFocus = newPoint.gameObject.transform.position;
		float oldDistance = _distance;
		float oldAngle = _y;
		Quaternion oldHeading = transform.rotation;
		oldHeading.eulerAngles = new Vector3(0f, oldHeading.eulerAngles.y,0f);
		float startTime = Time.time;
		float travelTime = (newFocus-_focusPosition).magnitude / 10f;
		if (travelTime > 0.1f){
			float factor;
			while(Time.time <= startTime + travelTime){
				factor = (Time.time - startTime)/travelTime;
				_focusPosition = Vector3.Lerp(oldFocus, newFocus, Mathf.SmoothStep(0f, 1f, factor));
				_x = Quaternion.Slerp(oldHeading, newPoint.gameObject.transform.rotation, Mathf.SmoothStep(0f, 1f, factor)).eulerAngles.y;
				_y = Mathf.Lerp(oldAngle, newPoint.startAngle, Mathf.SmoothStep(0f, 1f, factor));
				_distance = Mathf.Lerp(oldDistance, newPoint.startDistance, Mathf.SmoothStep(0f, 1f, factor));
				yield return null;
			}
			
			_focusPosition = newFocus;
			_maxDistance = newPoint.maxDistance;
			_minDistance = newPoint.minDistance;
			_highAngle = newPoint.maxAngle;
			_lowAngle = newPoint.minAngle;
		}
	}

	private int tempCullingMask;
	private CameraClearFlags tempClearFlags;

	public void freezeCamera()
	{
		tempCullingMask = GetComponent<Camera>().cullingMask;
		GetComponent<Camera>().cullingMask = 0;
		tempClearFlags =  GetComponent<Camera>().clearFlags;
		GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
	}

	public void UnfreezeCamera()
	{
		GetComponent<Camera>().cullingMask = tempCullingMask;
		GetComponent<Camera>().clearFlags = tempClearFlags;
	}
} //End class