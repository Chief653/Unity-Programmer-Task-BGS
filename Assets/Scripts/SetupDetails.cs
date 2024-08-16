using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupDetails : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public TMP_Text itemDescTxt;

    public void SetupDet(Item item) {
        itemNameTxt.text = item.itemName;
        itemDescTxt.text = item.itemDesc;
    }
}
