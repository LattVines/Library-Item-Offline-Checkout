using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DateHeader : MonoBehaviour {

    Text date_text;
	void Awake()
    {
        date_text = GetComponent<Text>();
        date_text.text = "today is " + System.DateTime.Today.ToShortDateString();
    }
}
