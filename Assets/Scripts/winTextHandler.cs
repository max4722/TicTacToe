using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class winTextHandler : MonoBehaviour {

	void Update () {
	
	}

	public void showText(string txt)
	{
		Text t = GetComponent<Text>();
		t.enabled = true;
		t.text = txt;
	}
}
