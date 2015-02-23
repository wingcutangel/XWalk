using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraHotspot : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GetComponentInChildren<Button>().onClick.AddListener(() =>{Camera.main.GetComponent<OrbitCamera>().switchFocusPoint(gameObject);});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
