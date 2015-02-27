using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ToggleColorScript : MonoBehaviour {
	static Color idleColor;
	static Color selectedColor;
	static Color highlightColor;
	static ColorBlock idleColors;
	private Text theText;
	static List<ToggleColorScript> menuButtons = new List<ToggleColorScript>();
	// Use this for initialization
	void Start () {
		theText = GetComponentInChildren<Text>();
		selectedColor = GetComponent<Button>().colors.normalColor;
		highlightColor = GetComponent<Button>().colors.highlightedColor;
		idleColor = theText.color;
		idleColors = GetComponent<Button>().colors;
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
		menuButtons.Add (this);
	}

	private void resetColors(){
		Button theButton = GetComponent<Button> ();
		theButton.colors = idleColors;
		theButton.GetComponentInChildren<Text>().color = idleColor;

	}

	public static void resetAllButtons(){
		foreach (ToggleColorScript tcs in menuButtons) {
			tcs.resetColors();
		}
	}
	public void resetAllButtonsWrapper(){
		resetAllButtons ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
