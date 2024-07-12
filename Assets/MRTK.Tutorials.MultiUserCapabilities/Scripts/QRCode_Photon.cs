using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QRCode_Photon : MonoBehaviourPunCallbacks
{
    public GameObject localObjectPrefab; // 這是預置的物體，用於在所有客戶端上實例化
    private GameObject localObjectInstance; // 這是場景中的實際物體實例

    //隱藏panel
    public GameObject qrCodePanelWPhoton;

    public void OnQRCodeScanned()
    {
        // 假設場景開始時你已經有一個物體，我們在此取得它
        localObjectInstance = GameObject.Find("QRCode(Clone)");
        // 確保物體不為null
        if (localObjectInstance)
        {
            Vector3 position = localObjectInstance.transform.position;
            Quaternion rotation = localObjectInstance.transform.rotation;

            // 摧毀或隱藏原物體
            Destroy(localObjectInstance);

            // 在所有客戶端上實例化一個新物體
            PhotonNetwork.Instantiate(localObjectPrefab.name, position, rotation);

            HideQRCodePanel();
        }
        else
        {
            Debug.LogWarning("Local object instance not found!");
        }
    }
    public void HideQRCodePanel()
    {
        if(qrCodePanelWPhoton != null)
        {
            qrCodePanelWPhoton.SetActive(false);
        }
        else
        {
            Debug.LogWarning("QRCodePanel is not set!");
        }
    }
}
