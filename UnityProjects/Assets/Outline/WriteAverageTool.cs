///Origin from: https://zhuanlan.zhihu.com/p/109101851
///You can change model tangents by the tool or add WriteMeshScript to gameobject.

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class PlugTangentTools
{
    [MenuItem("Tools/Write normal to tangent")]
    public static void WirteAverageNormalToTangentToos() {
        WriteNormalInTangents(Selection.activeGameObject);
    }

    public static void WriteNormalInTangents(GameObject pObject) {
        MeshFilter[] meshFilters = pObject.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters) {
            Mesh mesh = meshFilter.sharedMesh;
            WirteAverageNormalToTangent(mesh);
        }

        SkinnedMeshRenderer[] skinMeshRenders = pObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinMeshRender in skinMeshRenders) {
            Mesh mesh = skinMeshRender.sharedMesh;
            WirteAverageNormalToTangent(mesh);
        }
    }

    private static void WirteAverageNormalToTangent(Mesh mesh) {
        var averageNormalHash = new Dictionary<Vector3, Vector3>();
        for (var j = 0; j < mesh.vertexCount; j++) {
            if (!averageNormalHash.ContainsKey(mesh.vertices[j])) {
                averageNormalHash.Add(mesh.vertices[j], mesh.normals[j]);
            }
            else {
                averageNormalHash[mesh.vertices[j]] =
                    (averageNormalHash[mesh.vertices[j]] + mesh.normals[j]).normalized;
            }
        }

        var averageNormals = new Vector3[mesh.vertexCount];
        for (var j = 0; j < mesh.vertexCount; j++) {
            averageNormals[j] = averageNormalHash[mesh.vertices[j]];
        }

        var tangents = new Vector4[mesh.vertexCount];
        for (var j = 0; j < mesh.vertexCount; j++) {
            tangents[j] = new Vector4(averageNormals[j].x, averageNormals[j].y, averageNormals[j].z, 0);
        }
        mesh.tangents = tangents;
    }
}