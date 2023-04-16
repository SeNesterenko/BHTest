using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerManagement : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _rotationSpeed = 2f;
        [SerializeField] private UnityEvent _leftMouseButtonClicked;

        private void Update()
        {
            if (!isLocalPlayer) return;

            UserInput();
        }

        private void UserInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _leftMouseButtonClicked.Invoke();
            }
        
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            Move(horizontalInput, verticalInput);
        }
    
        private void Move(float horizontalInput, float verticalInput)
        {
            var moveStep = Time.deltaTime * _moveSpeed;
            var rotationStep = Time.deltaTime * _rotationSpeed;
        
            transform.Translate(Vector3.forward * verticalInput * moveStep);
            transform.Rotate(Vector3.up, horizontalInput * rotationStep);
        }
    }
}