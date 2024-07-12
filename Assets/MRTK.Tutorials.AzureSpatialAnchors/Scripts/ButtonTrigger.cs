using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using RestSharp;


public class ButtonTrigger : MonoBehaviour
{
    private AnchorModuleScript qrcodePhotonInstance;
    private GameObject qrcodePhotonGameObject;

    void Update()
    {
        if (qrcodePhotonInstance == null)
        {
            qrcodePhotonGameObject = GameObject.FindWithTag("QRCodePhotonTag");
            if (qrcodePhotonGameObject != null)
            {
                qrcodePhotonInstance = qrcodePhotonGameObject.GetComponent<AnchorModuleScript>();
                Debug.Log("Found QRCodePhoton instance!"); //valid
            }
        }
    }
    public async void StartAzure() //�Q���U�ɰ����쪫���StartAzureSession
    {
        // assume qrcode_photon object with the following tag
        
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.StartAzureSession();
            Debug.Log("qr code photon instance is not null");
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }
    public async void StopAzure() //�Q���U�ɰ����쪫���StopAzureSession
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.StopAzureSession();
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }
    public async void CreateAnchor() //�Q���U�ɰ����쪫���CreateAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.CreateAzureAnchor(qrcodePhotonGameObject);
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }

    public async void RemoveAnchor() //�Q���U�ɰ����쪫���RemoveAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.RemoveLocalAnchor(qrcodePhotonGameObject);
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }

    public async void FindAnchor() //�Q���U�ɰ����쪫���FindAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.FindAzureAnchor();
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }
    public async void DeleteAnchor() //�Q���U�ɰ����쪫���DeleteAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.DeleteAzureAnchor();
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }

    public async void ShareAnchor() //�Q���U�ɰ����쪫���ShareAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.ShareAzureAnchorIdToNetwork();
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }

    public async void LoadAnchor() //�Q���U�ɰ����쪫���LoadAnchor
    {
        if (qrcodePhotonInstance != null)
        {
            qrcodePhotonInstance.GetAzureAnchorIdFromNetwork();
        }
        else
        {
            Debug.LogWarning("QRCodePhoton instance not found!");
        }
    }

}
