using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "InGame/GunData", order = 1)]
public class GunDataConfig : BaseScriptable
{
    [SerializeField] private List<GunData> gunData;

    public static Dictionary<string, GunData> GunData;

    public override void Init()
    {
        GunData = new Dictionary<string, GunData>();

        foreach (var item in gunData)
        {
            GunData[item.gunID] = item;
        }
    }
}

[System.Serializable]
public class GunData
{
    public string gunID;
    public BaseGun gunPrefab;
}