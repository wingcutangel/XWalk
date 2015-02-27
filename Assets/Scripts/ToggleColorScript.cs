using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleColorScript : MonoBehaviour {
	static Color idleColor;
	static Color selectedColor;
	static Color highlightColor;

	private Text theText;
	// Use this for initialization
	void Start () {
		theText = GetComponentInChildren<Text>();
		selectedColor = GetComponent<Button>().colors.normalColor;
		highlightColor = GetComponent<Button>().colors.highlightedColor;
		idleColor = theText.color;
		ColorBlock idleColors = GetComponent<Button>().colors;
		ColorBlock selectedColors = idleColors;
		selectedColors.normalColor = highlightColor;
		GetComponent<Button>().onClick.AddListener(() => {
			foreach( Button theButton in transform.parent.GetComponentsInChildren<Button>())
			{
				theButton.colors = idleColors;
				theButton.GetComponentInChildren<Text>().color = idleColor;
			}
			GetComponent<Button>().colors = selectedColors;
			GetComponentInChildren<Text>().color = selectedColor;
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
