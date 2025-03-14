using UnityEngine;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI partInfoText;

    public void Initialize(CarParts part, int count)
    {
        partInfoText.text = $"{part.ToString()}: {count}";
    }
}