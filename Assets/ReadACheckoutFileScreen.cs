using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadACheckoutFileScreen : MonoBehaviour {

    public GameObject fileBrowser;//SET IN EDITOR
    public GameObject disable_screen;//set in editor
    public static string file_location;


    public void OpenAFileBroswer()
    {
        Instantiate(fileBrowser);
        disable_screen.SetActive(true);
    }

    public void OpenPickedfile(string file_path)
    {
        Debug.Log("open Picked File:" + file_path);
        GameController.GetInstance().OpenACheckoutFile(file_path);
        GameController.GetInstance().GoToCopyAndPastingScreen();
    }

    public void SetFileLocation(string file_string)
    {
        file_location = file_string;
      
        disable_screen.SetActive(false);
        OpenPickedfile(file_location);
    }

    public void CancelBrowse()
    {
        disable_screen.SetActive(false);
    }
}
