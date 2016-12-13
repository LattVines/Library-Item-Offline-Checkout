using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour {

    Text barcode_text;
    OfflineCheckOutScreen parent_screen;

     void Awake()
    {
        barcode_text = GetComponentInChildren<Text>();
        parent_screen = FindObjectOfType<OfflineCheckOutScreen>();
    }


    public void SetBarcodeText(string barcode)
    {
        barcode_text.text = barcode;
    }

    public void DeleteItemButton()
    {
        parent_screen.DeleteIndividualItem(barcode_text.text);
        Destroy(this.gameObject);
    }

}
