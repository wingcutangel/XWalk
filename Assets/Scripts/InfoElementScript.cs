using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoElementScript : MonoBehaviour {
	public Sprite theImage;
	public string theText;
	public Image imagePanel;
	public Text textPanel;
	public GameObject theInfoPanel;
	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener(switchPanel);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void switchPanel(){
		theInfoPanel.SetActive (true);
		imagePanel.sprite = theImage;
		textPanel.text = theText;
	}
}
