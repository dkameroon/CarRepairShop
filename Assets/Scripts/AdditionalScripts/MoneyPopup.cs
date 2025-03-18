using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    private TextMeshProUGUI popupText;
    private Animator animator;

    private void Awake()
    {
        popupText = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void ShowPopup(int amount)
    {
        popupText.text = $"+{amount}";
        animator.SetTrigger("Show");
    }

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}