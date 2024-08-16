using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavegInv : MonoBehaviour
{
    public List<GameObject> allTabs;

    public void OpenTab(GameObject activeTab) {
        foreach (GameObject tab in allTabs)
        {
            tab.SetActive(false);
        }

        activeTab.SetActive(true);
    }
}
