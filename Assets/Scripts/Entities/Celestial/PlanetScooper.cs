using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlanetScooper : NetworkBehaviour
{
    public Planet Target
    {
        get;
        private set;
    }

    public float Progress
    {
        get => _progress.Value;
        private set => _progress.Value = value;
    }

    public bool IsScooping
    {
        get;
        private set;
    }

    public event Action<Planet> OnScoopStarted;
    public event Action<Planet, float> OnScoopProgress;
    public event Action<Planet> OnScoopComplete;

    [SerializeField] private float _scoopSpeed = 1.0f;
    private Coroutine _scoopCoroutine;

    [Space] 
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private Transform _containerSpawnPoint;
    
    public NetworkVariable<float> _progress = new NetworkVariable<float>();
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsHost)
        {
            BeginScoop(null);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (IsHost)
        {
            StopCoroutine(_scoopCoroutine);
        }
    }

    public void BeginScoop(Planet planet)
    {
        Target = planet;
        Progress = 0.0f;
        
        _scoopCoroutine = StartCoroutine(ScoopUntilComplete());
        OnScoopStarted?.Invoke(planet);
    }

    private IEnumerator ScoopUntilComplete()
    {
        IsScooping = true;
        while (Progress < 100.0f)
        {
            Progress += _scoopSpeed * Time.deltaTime;
            Progress = Mathf.Clamp(Progress, 0.0f, 100.0f);

            OnScoopProgress?.Invoke(Target, Progress);
            yield return null;
        }
        
        CompleteScooping();
    }

    private void CompleteScooping()
    {
        Progress = 100.0f;
        IsScooping = false;
        OnScoopComplete?.Invoke(Target);

        SpawnContainer();
    }

    private void SpawnContainer()
    {
        if (IsHost)
        {
            NetworkController.Instance.SpawnEntityRpc(_containerPrefab.GetComponent<NetworkObject>().PrefabIdHash, _containerSpawnPoint.position, Quaternion.identity);
        }
    }
}
