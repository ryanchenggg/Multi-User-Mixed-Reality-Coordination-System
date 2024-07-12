using QRTracking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyQRCodeManager : MonoBehaviour
{
    public QRCodesManager qRCodesManager;
    public TextMeshPro statusText;
    // Start is called before the first frame update
    public void StartScan()
    {
        qRCodesManager.StartQRTracking();
        statusText.text = "Started QRCode Tracking";
    }

    // Update is called once per frame
    public void StopScan()
    {
        qRCodesManager.StopQRTracking();
        statusText.text = "Stopped QRCode Tracking";
    }
}
