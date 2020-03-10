using UnityEngine;

class WriteMeshScript : MonoBehaviour
{
    private void Awake() {
        PlugTangentTools.WriteNormalInTangents(gameObject);
    }
}

