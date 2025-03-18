using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarPartsDatabase", menuName = "CarParts/Database")]
public class CarPartsDatabase : ScriptableObject
{
    public List<CarPartData> carParts;
    
    public CarPartData GetCarPartData(CarParts part)
    {
        foreach (var typePart in carParts)
        {
            if (carParts.Contains(typePart))
            {
                return typePart;
            }
        }
        return null;
    }
}

[System.Serializable]
public class CarPartData
{
    public CarParts partType;
    public string partName;
    public Sprite icon;
    public int purchaseCost;
    public int repairReward;
    public float repairTime;
    
    
}