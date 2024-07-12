using UnityEngine;
using Autodesk.Fbx;
using System.Collections.Generic;
using System.IO;



public class WriteFBXonMouseClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject modelToExport = this.gameObject; // 獲取這個腳本附加的GameObject
            if (modelToExport == null)
            {
                Debug.LogError("Model not found in the scene.");
                return;
            }

            string fbxFilePath = Path.Combine(Application.dataPath, "ModifiedModel.fbx");
            fbxFilePath = Path.GetFullPath(fbxFilePath);
            Debug.Log($"The file that will be written is {fbxFilePath}");

            ExportFBX(modelToExport, fbxFilePath);
        }
    }

    void ExportFBX(GameObject model, string filePath)
    {
        Mesh combinedMesh = CombineMeshes(model);
        if (combinedMesh == null || combinedMesh.vertexCount == 0 || combinedMesh.triangles.Length == 0)
        {
            Debug.LogError("Failed to create a valid combined mesh.");
            return;
        }
        using (var fbxManager = FbxManager.Create())
        {
            var fbxIOSettings = FbxIOSettings.Create(fbxManager, Globals.IOSROOT);
            fbxManager.SetIOSettings(fbxIOSettings);

            var fbxExporter = FbxExporter.Create(fbxManager, "Exporter");
            int fileFormat = fbxManager.GetIOPluginRegistry().FindWriterIDByDescription("FBX ascii (*.fbx)");
            bool status = fbxExporter.Initialize(filePath, fileFormat, fbxIOSettings);

            if (!status)
            {
                Debug.LogError($"Failed to initialize exporter, reason: {fbxExporter.GetStatus().GetErrorString()}");
                return;
            }

            var fbxScene = FbxScene.Create(fbxManager, "Scene");
            var fbxRootNode = fbxScene.GetRootNode();
            var fbxNode = FbxNode.Create(fbxScene, model.name);
            var fbxMesh = FbxMesh.Create(fbxScene, "TheMesh");
            ConvertMeshToFbx(combinedMesh, fbxMesh);
            fbxNode.SetNodeAttribute(fbxMesh);
            fbxRootNode.AddChild(fbxNode);

            status = fbxExporter.Export(fbxScene);

            if (!status)
            {
                Debug.LogError("Failed to export the scene.");
            }

            fbxScene.Destroy();
            fbxExporter.Destroy();
        }
    }

    Mesh CombineMeshes(GameObject parentObject)
    {
        List<CombineInstance> combine = new List<CombineInstance>();
        Matrix4x4 parentWorldToLocal = parentObject.transform.worldToLocalMatrix;
        Matrix4x4 scaleMatrix = Matrix4x4.Scale(new Vector3(100, 100, 100)); // Scaling by 100
        Matrix4x4 mirrorXMatrix = Matrix4x4.Scale(new Vector3(-1, 1, 1));

        foreach (Transform child in parentObject.transform)
        {
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                if (meshFilter.sharedMesh.isReadable)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = meshFilter.sharedMesh;
                    Matrix4x4 childTransform = Matrix4x4.TRS(child.localPosition, child.localRotation, child.localScale);
                    ci.transform = mirrorXMatrix * scaleMatrix * parentWorldToLocal * childTransform; // Apply scaling here
                    combine.Add(ci);
                }
                else
                {
                    Debug.LogError("Cannot combine mesh that does not allow access: " + child.name);
                }
            }
        }

        Mesh combinedMesh = new Mesh();
        if (combine.Count > 0)
        {
            combinedMesh.CombineMeshes(combine.ToArray(), true, true);
        }
        return combinedMesh;
    }



    void ConvertMeshToFbx(Mesh mesh, FbxMesh fbxMesh)
    {
        if (fbxMesh == null)
        {
            Debug.LogError("FBX Mesh has not been initialized.");
            return;
        }
        if (mesh == null)
        {
            Debug.LogError("Mesh data is null.");
            return;
        }

        int numVertices = mesh.vertexCount;
        fbxMesh.InitControlPoints(numVertices);
        for (int i = 0; i < numVertices; i++)
        {
            Vector3 vertex = mesh.vertices[i];
            fbxMesh.SetControlPointAt(new FbxVector4(vertex.x, vertex.y, vertex.z), i);
        }

        if (mesh.triangles == null || mesh.triangles.Length == 0)
        {
            Debug.LogError("Mesh triangles data is null or empty.");
            return;
        }

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            fbxMesh.BeginPolygon();
            fbxMesh.AddPolygon(mesh.triangles[i]);
            fbxMesh.AddPolygon(mesh.triangles[i + 1]);
            fbxMesh.AddPolygon(mesh.triangles[i + 2]);
            fbxMesh.EndPolygon();
        }
    }
}
