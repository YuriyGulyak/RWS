using System;
using UnityEngine;

[Serializable]
public class BestLapKeyStorageItem
{
    public string playerPrefsKey;
    public string dreamloPublicCode;
    public string dreamloPrivateCode;
}

[CreateAssetMenu]
public class BestLapKeyStorage : ScriptableObject
{
    public BestLapKeyStorageItem[] items;
}
