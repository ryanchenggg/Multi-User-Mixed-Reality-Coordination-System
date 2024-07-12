using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace QRTracking
{
    public class QRCodesVisualizer : MonoBehaviour
    {
        public GameObject qrCodePrefab;
        public TextMeshPro LatestQRCodeDetails;

        private SortedDictionary<System.Guid, GameObject> qrCodesObjectsList;
        private Queue<GameObject> objectPool = new Queue<GameObject>();
        private Queue<ActionData> pendingActions = new Queue<ActionData>();

        struct ActionData
        {
            public enum Type
            {
                Added,
                Updated,
                Removed
            };
            public Type type;
            public Microsoft.MixedReality.QR.QRCode qrCode;

            public ActionData(Type type, Microsoft.MixedReality.QR.QRCode qRCode) : this()
            {
                this.type = type;
                this.qrCode = qRCode;
            }
        }

        private GameObject GetFromPool()
        {
            if (objectPool.Count > 0)
            {
                var obj = objectPool.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                return Instantiate(qrCodePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }

        private void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            qrCodesObjectsList = new SortedDictionary<System.Guid, GameObject>();

            // Unsubscribe from events to avoid double subscription
            QRCodesManager.Instance.QRCodesTrackingStateChanged -= Instance_QRCodesTrackingStateChanged;
            QRCodesManager.Instance.QRCodeAdded -= Instance_QRCodeAdded;
            QRCodesManager.Instance.QRCodeUpdated -= Instance_QRCodeUpdated;
            QRCodesManager.Instance.QRCodeRemoved -= Instance_QRCodeRemoved;

            if (qrCodePrefab == null)
            {
                throw new System.Exception("Prefab not assigned");
            }

            ClearAllObjects();

            // Subscribe to events
            QRCodesManager.Instance.QRCodesTrackingStateChanged += Instance_QRCodesTrackingStateChanged;
            QRCodesManager.Instance.QRCodeAdded += Instance_QRCodeAdded;
            QRCodesManager.Instance.QRCodeUpdated += Instance_QRCodeUpdated;
            QRCodesManager.Instance.QRCodeRemoved += Instance_QRCodeRemoved;
        }

        private void Instance_QRCodesTrackingStateChanged(object sender, bool status)
        {
            if (status) // if 掃描開始
            {
                // [MODIFIED]
                ClearAllObjects();
            }

        }

        private void Instance_QRCodeAdded(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            // [MODIFIED]
            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Added, e.Data));
            }
        }

        private void Instance_QRCodeUpdated(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Updated, e.Data));
            }
        }

        private void Instance_QRCodeRemoved(object sender, QRCodeEventArgs<Microsoft.MixedReality.QR.QRCode> e)
        {
            lock (pendingActions)
            {
                pendingActions.Enqueue(new ActionData(ActionData.Type.Removed, e.Data));
            }
        }

        private void ClearAllObjects()
        {
            // 清除 qrCodesObjectsList 中的物件
            foreach (var obj in qrCodesObjectsList.Values)
            {
                // [MODIFIED]
                Destroy(obj);
            }
            qrCodesObjectsList.Clear();

            // 清除 objectPool 中的物件
            while (objectPool.Count > 0)
            {
                Destroy(objectPool.Dequeue());
            }

            Debug.Log("All objects cleared and initialized");
        }

        private void HandleEvents()
        {
            lock (pendingActions)
            {
                while (pendingActions.Count > 0)
                {
                    var action = pendingActions.Dequeue();

                    if (action.type == ActionData.Type.Added)
                    {
                        GameObject qrCodeObject = GetFromPool();
                        qrCodeObject.GetComponent<SpatialGraphNodeTracker>().Id = action.qrCode.SpatialGraphNodeId;
                        qrCodeObject.GetComponent<QRCode>().qrCode = action.qrCode;
                        LatestQRCodeDetails.text = action.qrCode.Data;
                        qrCodesObjectsList.Add(action.qrCode.Id, qrCodeObject);
                    }
                    else if (action.type == ActionData.Type.Updated)
                    {
                        if (!qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {
                            GameObject qrCodeObject = GetFromPool();
                            qrCodeObject.GetComponent<SpatialGraphNodeTracker>().Id = action.qrCode.SpatialGraphNodeId;
                            qrCodeObject.GetComponent<QRCode>().qrCode = action.qrCode;
                            qrCodesObjectsList.Add(action.qrCode.Id, qrCodeObject);
                        }
                    }
                    else if (action.type == ActionData.Type.Removed)
                    {
                        if (qrCodesObjectsList.ContainsKey(action.qrCode.Id))
                        {
                            ReturnToPool(qrCodesObjectsList[action.qrCode.Id]);
                            qrCodesObjectsList.Remove(action.qrCode.Id);
                        }
                    }
                }
            }
        }

        void Update()
        {
            HandleEvents();
        }
    }
}