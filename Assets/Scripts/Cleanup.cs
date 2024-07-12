using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    private GameObject localObjectInstance; // 這是場景中的實際物體實例

    public void CleanItemsUp()
    {
        // 假設場景開始時你已經有一個物體，我們在此取得它
        localObjectInstance = GameObject.Find("QRCode(Clone)");
        // 確保物體不為null
        if (localObjectInstance)
        {
            Debug.Log("Local object gonna be cleaned up");
            Destroy(localObjectInstance);
        }
        else
        {
            Debug.LogWarning("Local object instance not found!");
        }
    }
}