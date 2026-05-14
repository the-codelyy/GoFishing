using System;
using TMPro;
using UnityEngine;

public class PlanetScoopUI : MonoBehaviour
{
    [SerializeField] private PlanetScooper _planetScooper;

    [Space] 
    [SerializeField] private TextMeshProUGUI _progress;

    private void Start()
    {
        _planetScooper._progress.OnValueChanged += OnScoopProgress;
    }
    
    private void OnDestroy()
    {
        _planetScooper._progress.OnValueChanged -= OnScoopProgress;
    }

    private void OnScoopProgress(float oldValue, float newValue)
    {
        _progress.text = newValue.ToString("F0") + "%";
    }
}
