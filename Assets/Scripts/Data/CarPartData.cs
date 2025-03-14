using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarPartsDatabase", menuName = "CarParts/Database")]
public class CarPartsDatabase : ScriptableObject
{
    public List<CarPartData> carParts;
}

[System.Serializable]
public class CarPartData
{
    public CarParts partType;
    public string partName;
    public Sprite icon;
    public int purchaseCost;
    public int repairReward;
}