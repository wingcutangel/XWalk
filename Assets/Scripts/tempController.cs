using UnityEngine;
using System.Collections;

public class tempController : MonoBehaviour {
	public GameObject[] targets;
	private int index;
	// Use this for initialization
	void Start () {
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)){
			print ("keyPressed");
			Camera.main.GetComponent<OrbitCamera>().switchFocusPoint(targets[index]);
			index++;
			if(index >= targets.Length - 1){
				index = 0;
			}
		}
	}
}
