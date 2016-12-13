using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineCheckOutScreen : MonoBehaviour {



    public GameObject CancelButton, SaveButton, ItemSidePanel, namePanel;
    public InputField PatronInputField, ItemInputField, nameInputField;
    public ScrollRect item_holder;//SET IN EDITOR
    public Text item_cart_label__with_counter, message_text;//SET IN EDITOR
    string label_string = "Item cart: ";
    string warning_symbol_unicode = "";

    int patron_barcode_length = 14;
    int item_barcode_length = 14;

    List<string> current_scanned_items_list = new List<string>();
    string current_scanned_patron_barcode = string.Empty;

    
    

    public GameObject item_prefab;//SET IN EDITOR

    void Awake()
    {
        CancelButton.SetActive(false);
        SaveButton.SetActive(false);
        ItemSidePanel.SetActive(false);
        namePanel.SetActive(false);
        message_text.text = string.Empty;
        PatronInputField.Select();
    }



    public void PatronScanned()
    {
        if(PatronInputField.text != current_scanned_patron_barcode && current_scanned_items_list.Count > 0 )
        {
            ItemInputField.text = string.Empty;
            current_scanned_items_list.Clear();
            item_cart_label__with_counter.text = label_string + current_scanned_items_list.Count.ToString();

            ItemBox[] item_boxes = FindObjectsOfType<ItemBox>();
            if (item_boxes != null)
            {
                for (int i = 0; i < item_boxes.Length; i++)
                {
                    Destroy(item_boxes[i].gameObject);
                }
            }
        }



        if (PatronInputField.text.Length == patron_barcode_length) {


   


            //first check if we already have this number in the list
            CheckOutItem doesItExist = FindIfExisting(PatronInputField.text);
            if (doesItExist != null)
            {
                Debug.Log("need to recall this card numbers checked out items");
                DisplayMessage("Recalled patron from list");
                RefillInfoFromExistingPatron(doesItExist);
            }
            else
            {

                CancelButton.SetActive(true);
                SaveButton.SetActive(true);
                ItemSidePanel.SetActive(true);
                namePanel.SetActive(true);
                current_scanned_patron_barcode = PatronInputField.text;
                ClearMessage();

                if (!PatronInputField.text.StartsWith("21"))
                {
                    Debug.Log("trying to display warning in red");
                    DisplayMessage("<color=red>this may not be a valid number</color>");
                }

                ItemInputField.Select();
            }

        }
        else
        {
            if(!(PatronInputField.text.Length == 0))
            DisplayMessage("Invalid patron barcode. Must be 14 digits.");
        }
    }



    public void ItemScanned()
    {

        string scanned_content = ItemInputField.text;

        if (scanned_content.Length != item_barcode_length)
        {
            if(!(scanned_content.Length == 0))
            DisplayMessage("invalid item barcode. Must be 14 digits.");
           
            return;
        } //ensure that a full barcode is read

        if (current_scanned_items_list.Contains(scanned_content))
        {
            //Debug.Log("this item is already scanned: " + scanned_content);
            DisplayMessage("Item already in cart");
           
        }
        else
        {
            //Debug.Log("new item: " + scanned_content);
            ItemInputField.text = string.Empty;//clear out the input field

            //add it into list
            current_scanned_items_list.Add(scanned_content);

            item_cart_label__with_counter.text = label_string + current_scanned_items_list.Count.ToString();

            //add it into the view
            GameObject item_obj = Instantiate(item_prefab) as GameObject;
            item_obj.GetComponent<ItemBox>().SetBarcodeText(scanned_content);
            item_obj.transform.SetParent(item_holder.content, false);

           
            ClearMessage();
        }

        Invoke("ReselectItemInputField", 0.1f);


    }

    public void ReselectItemInputField()
    {
        ItemInputField.Select();
    }


    public void DisplayMessage(string msg)
    {
        message_text.text = warning_symbol_unicode + " " + msg;
    }
    public void ClearMessage()
    {
        message_text.text = string.Empty;
    }




    public void CancelButtonAction()
    {
        PatronInputField.text = string.Empty;
        ItemInputField.text = string.Empty;
        nameInputField.text = string.Empty;
        nameInputField.text = string.Empty;

        current_scanned_patron_barcode = string.Empty;
        current_scanned_items_list.Clear();
        item_cart_label__with_counter.text = label_string + current_scanned_items_list.Count.ToString();

        ItemBox[] item_boxes = FindObjectsOfType<ItemBox>();
        if(item_boxes != null)
        {
            for(int i=0; i< item_boxes.Length; i++)
            {
                Destroy(item_boxes[i].gameObject);
            }
        }

        //WARNING, Disable these things AFTER deleting the item
        //boxes above. If they are inactive the FindObjectsOfType
        //doesn't find them.
        CancelButton.SetActive(false);
        SaveButton.SetActive(false);
        ItemSidePanel.SetActive(false);
        namePanel.SetActive(false);


        PatronInputField.Select();
        ClearMessage();

    }




    public void SaveButtonAction()
    {
        PatronInputField.Select();

        CheckOutItem item = new CheckOutItem();
        item.patron_string = current_scanned_patron_barcode;
        item.items = current_scanned_items_list.ToArray();
        item.patron_name = nameInputField.text;

        Debug.Log("saving item: " + item.ToString() + "with this many check out items:  " + item.items.Length);
        GameController.GetInstance().AddCheckOutItem(item);

        CancelButtonAction();
    }




    public void DeleteIndividualItem(string barcode2delete)
    {
        bool isDeleted =  current_scanned_items_list.Remove(barcode2delete);
        if (isDeleted) Debug.Log("deleted this item: " + barcode2delete);
        else
        {
            DisplayMessage("COULD NOT DELETE ITEM: " + barcode2delete);
            Debug.Log("COULD NOT DELETE ITEM: " + barcode2delete);
        }
        item_cart_label__with_counter.text = label_string + current_scanned_items_list.Count.ToString();

        ItemInputField.Select();
    }


    public CheckOutItem FindIfExisting(string scanned_patron_id)
    {
        for(int i=0; i < GameController.GetInstance().check_out_items_list.Count; i++)
        {
            if (GameController.GetInstance().check_out_items_list[i].patron_string == scanned_patron_id)
            {
                return GameController.GetInstance().check_out_items_list[i];
            }
        }
        Debug.Log("no match");
        return null;
    }


    public void RefillInfoFromExistingPatron(CheckOutItem patron_item_unit_thingy)
    {
        CancelButton.SetActive(true);
        SaveButton.SetActive(true);
        ItemSidePanel.SetActive(true);
        namePanel.SetActive(true);
        current_scanned_patron_barcode = PatronInputField.text;
        nameInputField.text = patron_item_unit_thingy.patron_name;


        ItemInputField.Select();

        int item_counter = patron_item_unit_thingy.items.Length;

        for(int i=0; i < item_counter; i++)
        {
            //add it into the view
            GameObject item_obj = Instantiate(item_prefab) as GameObject;
            item_obj.GetComponent<ItemBox>().SetBarcodeText(patron_item_unit_thingy.items[i]);
            //add item string into list
            current_scanned_items_list.Add(patron_item_unit_thingy.items[i]);
            item_obj.transform.SetParent(item_holder.content, false);
        }

        item_cart_label__with_counter.text = label_string + current_scanned_items_list.Count.ToString();

    }
    


}
