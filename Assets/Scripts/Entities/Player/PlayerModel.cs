using UnityEngine;

public class PlayerModel : PlayerComponent
{
    private PlayerCamera _camera;

    private void Awake()
    {
        _camera = transform.parent.GetComponentInChildren<PlayerCamera>();
    }

    private void Start()
    {
        int layer = LayerMask.NameToLayer("PlayerModel");
        SetLayerRecursively(gameObject, layer);
    }
    
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_camera.GetForward());
    }
    
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
	
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
