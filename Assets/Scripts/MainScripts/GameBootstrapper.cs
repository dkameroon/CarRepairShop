using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject liftPrefab;
    [SerializeField] private UIManager uiManager;
    public GameObject LiftPrefab => liftPrefab;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
}
