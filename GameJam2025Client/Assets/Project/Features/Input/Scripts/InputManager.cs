using Project.Core.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Features.Input.Scripts
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        [SerializeField] private InputActionAsset _inputActions;
        private InputAction _pointAction;
        private InputAction _clickAction;

        public bool IsPointDown => _clickAction.IsPressed();
        public Vector2 PointPosition => _pointAction.ReadValue<Vector2>();
        
        private void Start()
        {
            _pointAction = _inputActions.FindAction("Point");
            _clickAction = _inputActions.FindAction("Click");
            
            _pointAction.Enable();
            _clickAction.Enable();
        }
    }
}