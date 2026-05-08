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
        _planetScooper.OnScoopProgress += OnScoopProgress;
    }
    
    private void OnDestroy()
    {
        _planetScooper.OnScoopProgress -= OnScoopProgress;
    }

    private void OnScoopProgress(Planet planet, float progress)
    {
        _progress.text = progress.ToString("F0") + "%";
    }
}
