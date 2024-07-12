using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

#if MIXED_REALITY_OPENXR
using Microsoft.MixedReality.OpenXR;
#else
using QRTracking.WindowsMR;
#endif

namespace QRTracking
{
    public class SpatialGraphNodeTracker : MonoBehaviour
    {
        private System.Guid _id;
        private SpatialGraphNode node = null; //可以變為不做事?
        private bool isAligned = false; // 新增的布爾變量

        public System.Guid Id
        {
            get => _id;

            set
            {
                if (_id != value)
                {
                    _id = value;
                    InitializeSpatialGraphNode(force: true);
                }
            }
        }

        void Start()
        {
            InitializeSpatialGraphNode();
        }

        void Update()
        {
            if (isAligned || Id == new System.Guid("00000000-0000-0000-0000-000000000000")) return; 
            InitializeSpatialGraphNode();
            if (node != null)
            {
                if (node.TryLocate(FrameTime.OnUpdate, out Pose pose))
                {
                    if (CameraCache.Main.transform.parent != null)
                    {
                        pose = pose.GetTransformedBy(CameraCache.Main.transform.parent);
                    }

                    gameObject.transform.SetPositionAndRotation(pose.position, pose.rotation);
                    Debug.Log("Id= " + Id + " QRPose = " + pose.position.ToString("F7") + " QRRot = " + pose.rotation.ToString("F7"));
                    Debug.Log("Align");
                    isAligned = true; // 更新成功後設置為true
                }
                else
                {
                    Debug.LogWarning("Cannot locate " + Id);
                }
            }
        }

        public void ResetAlignment()
        {
            isAligned = false;
            node = null; // 將node設置為null，強制在下次更新時重新初始化
        }

        private void InitializeSpatialGraphNode(bool force = false)
        {
            if (node == null || force)
            {
                node = (Id != System.Guid.Empty) ? SpatialGraphNode.FromStaticNodeId(Id) : null;
                Debug.Log("Initialize SpatialGraphNode Id= " + Id);
            }
        }

    }
}
