using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    private GameObject localObjectInstance; // �o�O����������ڪ�����

    public void CleanItemsUp()
    {
        // ���]�����}�l�ɧA�w�g���@�Ӫ���A�ڭ̦b�����o��
        localObjectInstance = GameObject.Find("QRCode(Clone)");
        // �T�O���餣��null
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