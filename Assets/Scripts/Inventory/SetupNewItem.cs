using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupNewItem : MonoBehaviour
{
    public List<TMP_Text> itemNameTxt;
    public List<Image> icon;
    public List<GameObject> newSlots;
    public Button deleteBtn;

    public void SetupDet(List<Item> itemToGive) {
        int index = 0;

        foreach (Item item in itemToGive)
        {
            itemNameTxt[index].text = item.itemName;
            icon[index].sprite = item.itemIcon;
            newSlots[index].SetActive(true);

            index++;
        }
    }

    public void SimpleSetupDet(Item item) { //Used in deletion of a item
        if(newSlots.Count > 1) {
            newSlots[1].SetActive(false);
            newSlots[2].SetActive(false);
        }

        itemNameTxt[0].text = item.itemName;
        icon[0].sprite = item.itemIcon;
    }
}
