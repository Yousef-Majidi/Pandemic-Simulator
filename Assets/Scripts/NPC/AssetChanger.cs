using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetChanger", menuName = "ScriptableObjects/AssetChanger", order = 1)]
public class AssetChanger : ScriptableObject
{
    public GameObject UpdateAsset(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject asset = Instantiate(prefab, position, rotation);
        return asset;
    }
}
