using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInteractor : PlayerComponent
{
   public event Action<IInteractable> OnInteractHover;
   public event Action<IInteractable> OnInteractUnhover;
   
   [SerializeField] private float _interactionRange;
   private Item _item;

   [Header("Input")] 
   [SerializeField] private InputActionReference _carryAction;
   
   private PlayerCamera _camera;
   private PlayerCarry _playerCarry;

   private void Awake()
   {
      _camera = GetComponentInChildren<PlayerCamera>();
      _playerCarry = GetComponentInChildren<PlayerCarry>();
   }

   private void Start()
   {
      _carryAction.action.started += OnCarryAction;
   }

   public override void OnDestroy()
   {
      base.OnDestroy();

      _carryAction.action.started -= OnCarryAction;
   }

   private void FixedUpdate()
   {
      RaycastHit hit;
      Vector3 cameraPos = _camera.Transform.position;
      Vector3 cameraDirection = _camera.Transform.TransformDirection(Vector3.forward);

      Color debugRayColor = Color.yellow;
      if (Physics.Raycast(cameraPos, cameraDirection, out hit, _interactionRange, 1 << 7))
      {
         IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
         if (interactable != null && interactable !=  _item)
         { 
            Debug.Log($"Hovering on Interactable '{interactable.GetType().Name}'");
            
            _item = (Item)interactable;
            OnInteractHover?.Invoke(interactable);
         }
         
         debugRayColor = Color.red;
      }
      else if (_item != null)
      {
         Debug.Log($"Unhovering on Interactable '{_item.GetType().Name}'");
         
         _item = null;
         OnInteractUnhover?.Invoke(_item);
      }
      
      Debug.DrawRay(cameraPos, cameraDirection * _interactionRange, debugRayColor);
   }

   private void Interact()
   {
      
   }

   private void OnCarryAction(InputAction.CallbackContext context)
   {
      if (_playerCarry.Item != null)
      {
         _playerCarry.Drop();
         return;
      }
      
      if (_item != null && _playerCarry.Item == null)
      {
         _playerCarry.Carry(_item);
      }
   }
}
