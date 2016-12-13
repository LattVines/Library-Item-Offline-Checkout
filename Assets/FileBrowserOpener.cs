using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileBrowserOpener : MonoBehaviour
{
    string output = "no file";
    FileBrowser fb =  new FileBrowser();

    ReadACheckoutFileScreen parent_screen;



    void OnGUI()
    {
       
        if (fb.draw())
        {
            if (fb.outputFile == null)
            {
                Debug.Log("Cancel hit");
                parent_screen = FindObjectOfType<ReadACheckoutFileScreen>();
                parent_screen.CancelBrowse();
                Destroy(this.gameObject);
            }
            else
            {
                parent_screen = FindObjectOfType<ReadACheckoutFileScreen>();
                Debug.Log("Ouput File = \"" + fb.outputFile.ToString() + "\"");
                parent_screen.SetFileLocation(fb.outputFile.ToString());
               
                Destroy(this.gameObject);
                /*the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"*/
            }
        }
    }
}
