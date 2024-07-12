using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class QRCode_Photon : MonoBehaviourPunCallbacks
{
    public GameObject localObjectPrefab; // �o�O�w�m������A�Ω�b�Ҧ��Ȥ�ݤW��Ҥ�
    private GameObject localObjectInstance; // �o�O����������ڪ�����

    //����panel
    public GameObject qrCodePanelWPhoton;

    public void OnQRCodeScanned()
    {
        // ���]�����}�l�ɧA�w�g���@�Ӫ���A�ڭ̦b�����o��
        localObjectInstance = GameObject.Find("QRCode(Clone)");
        // �T�O���餣��null
        if (localObjectInstance)
        {
            Vector3 position = localObjectInstance.transform.position;
            Quaternion rotation = localObjectInstance.transform.rotation;

            // �R�������í쪫��
            Destroy(localObjectInstance);

            // �b�Ҧ��Ȥ�ݤW��ҤƤ@�ӷs����
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
