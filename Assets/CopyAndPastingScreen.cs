using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyAndPastingScreen : MonoBehaviour {

    public InputField patron_number, item_numbers_box, patron_name_box;
    public Text index_indicator_label, top_header_label;


    int current_index_display = -1;

    CheckOutItem[] all_items_ref;



    void OnEnable()
    {
        all_items_ref = GameController.GetInstance().all_items;
        if(all_items_ref.Length > 0)
        {
            current_index_display = 0;
            UpdateDisplayStuff(all_items_ref[0]);
           
        }

    }


    public void UpdateDisplayStuff(CheckOutItem item)
    {
       
        patron_number.text = item.patron_string;
        patron_name_box.text = item.patron_name;

        string items_string_list = string.Empty;
        int item_count = item.items.Length;

        for(int i=0; i< item_count; i++)
        {
            if (i > 0)
            {
                items_string_list += "\n";
            }

            items_string_list += item.items[i];
        }

        item_numbers_box.text = items_string_list;
        index_indicator_label.text = "item " + (current_index_display + 1)  + " of " + all_items_ref.Length;
        top_header_label.text = "reading file - " + ReadACheckoutFileScreen.file_location;
    }


    public void PreviousButton()
    {
        if (current_index_display > 0)
        {
            current_index_display--;
            UpdateDisplayStuff(all_items_ref[current_index_display]);
        }
    }

    public void Nextbutton()
    {
        if (current_index_display < all_items_ref.Length - 1)
        {
            current_index_display++;
            UpdateDisplayStuff(all_items_ref[current_index_display]);
        }
    }



}
