using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetChanger", menuName = "ScriptableObjects/AssetChanger", order = 1)]
public class AssetChanger : ScriptableObject
{
    [SerializeField]
    private GameObject _healthyAsset;

    [SerializeField]
    private GameObject _infectedAsset;

    public GameObject UpdateAsset(bool infected, Vector3 position, Quaternion rotation)
    {
        if (infected)
        {
            GameObject asset = Instantiate(_infectedAsset, position, rotation);
            return asset;
        }
        else
        {
            GameObject asset = Instantiate(_healthyAsset, position, rotation);
            return asset;
        }
    }
}
