using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPhysic
{
    public class PlayerPhysicsHandler : MonoBehaviour
    {

        [SerializeField] private CharacterController CharacterController;


        private PlayerInputsActions playerInputs;

        private void Awake()
        {
            playerInputs = new PlayerInputsActions();
        }

        private void Update()
        {
            CharacterController.Move(new Vector3(playerInputs.CharacterControls.Movement.ReadValue<Vector2>().x * Time.fixedDeltaTime,0,
                playerInputs.CharacterControls.Movement.ReadValue<Vector2>().y * Time.fixedDeltaTime));
        }

        private void OnEnable()
        {
            playerInputs.Enable();
        }

        private void OnDisable()
        {
            playerInputs.Disable();
        }






    }
}
