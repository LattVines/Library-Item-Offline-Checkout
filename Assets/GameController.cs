using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




public class GameController : MonoBehaviour {

    public GameObject MainMnuScreen, OfflineCheckOutScreen, ReadACheckoutFileScreen, CopyAndPastingScreen;//SET IN EDITOR


    static GameController __instance__ = null;


    private void Awake()
    {

        if(__instance__ == null)
        {
            __instance__ = this;
        }
        else
        {
            Destroy(this.gameObject);

        }


        MainMnuScreen.SetActive(true);
        OfflineCheckOutScreen.SetActive(false);
        ReadACheckoutFileScreen.SetActive(false);
        CopyAndPastingScreen.SetActive(false);
        ResetListOnStartUpIfAvailable();
    }

    public static GameController GetInstance()
    {
        return __instance__;
    }


    public void GoToMainMenu() {
        MainMnuScreen.SetActive(true);
        OfflineCheckOutScreen.SetActive(false);
        ReadACheckoutFileScreen.SetActive(false);
        CopyAndPastingScreen.SetActive(false);


    }


    public void GoToOfflineCheckOutScreen() {
        MainMnuScreen.SetActive(false);
        OfflineCheckOutScreen.SetActive(true);
        ReadACheckoutFileScreen.SetActive(false);
        CopyAndPastingScreen.SetActive(false);


    }


    public void GoToReadACheckoutFileScreen() {

        MainMnuScreen.SetActive(false);
        OfflineCheckOutScreen.SetActive(false);
        ReadACheckoutFileScreen.SetActive(true);
        CopyAndPastingScreen.SetActive(false);

    }

    public void GoToCopyAndPastingScreen() {

        MainMnuScreen.SetActive(false);
        OfflineCheckOutScreen.SetActive(false);
        ReadACheckoutFileScreen.SetActive(false);
        CopyAndPastingScreen.SetActive(true);

       
    }



    public List<CheckOutItem> check_out_items_list = new List<CheckOutItem>();

    public CheckOutItem[] all_items;

    public void AddCheckOutItem(CheckOutItem item)
    {


        //if it exists in list it may be saving with new item.items.
        //find it and delete it first
        for(int i =0; i < check_out_items_list.Count; i++ )
        {
            if(check_out_items_list[i].patron_string == item.patron_string)
            {
                check_out_items_list.Remove(check_out_items_list[i]);
            }
        }

         check_out_items_list.Add(item);

      
        string date_string = DateTime.Today.ToShortDateString();
        date_string = date_string.Replace('\\', '-');
        date_string = date_string.Replace('/', '-');
        string file_name_with_date = "checkout_list_" + date_string + ".json";

         SaveAllDataToJSONFile(file_name_with_date);
 
         


    }

    public void ResetListOnStartUpIfAvailable()
    {
        string date_string = DateTime.Today.ToShortDateString();
        date_string = date_string.Replace('\\', '-');
        date_string = date_string.Replace('/', '-');
        string file_name_with_date = "checkout_list_" + date_string + ".json";

        OpenACheckoutFile(file_name_with_date);

        if(all_items != null && all_items.Length > 0)
        {
            //if it has content the file was found. Fill the list using the array
            Debug.Log("If file was found, refill the list");
            for(int i=0; i < all_items.Length; i++)
            {
                check_out_items_list.Add(all_items[i]);
            }

        }

    }

    public void OpenACheckoutFile(string file_path)
    {
        string read_content = FileMan.ReadFileContent(file_path);
        if (read_content != string.Empty)
        {
            all_items = JsonHelper.FromJson<CheckOutItem>(read_content);
        }


    }

    public void SaveAllDataToJSONFile(string file_name)
    {
        //CheckOutItem[] all_items = new CheckOutItem[check_out_items_list.Count]; ;
        //Debug.Log("attempting to save all data to JSON at file location: " + file_name);

        string building_json_string = string.Empty;

        for(int i =0; i < check_out_items_list.Count; i++)
        {

            if (building_json_string != string.Empty) building_json_string = building_json_string + ",";

            string js_converted = JsonUtility.ToJson(check_out_items_list[i]);

            building_json_string = building_json_string + js_converted;

            
        }

        building_json_string = "{\"Items\":[" + building_json_string + "]}";

        //DoJsonReadtest(building_json_string);



        FileMan.RewriteFile(building_json_string, file_name);
        


    }//end SaveAllDataToJSONFile



    public void DoJsonReadtest(string test_json_string)
    {
        CheckOutItem[] all_items;
        all_items = JsonHelper.FromJson<CheckOutItem>(test_json_string);
        Debug.Log("test reading json. Read this many from string: " + all_items.Length);

        Debug.Log("print read results:\n");
        for(int i=0; i< all_items.Length; i++)
        {
            Debug.Log("i" + i + " -- patron id:" + all_items[i].patron_string + "  itmes #: " + all_items[i].items.Length);
        }

    }



}






[System.Serializable]
public class CheckOutItem
{
    public string patron_string;
    public string patron_name;
    public string[] items; 
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

