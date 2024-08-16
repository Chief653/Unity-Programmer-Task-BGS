using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupNewItem : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public Image icon;
    public Button deleteBtn;

    public void SetupDet(Item item) {
        itemNameTxt.text = item.itemName;
        icon.sprite = item.itemIcon;
    }
}
