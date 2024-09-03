using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAnimation
{
    public class PlayerBaseAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private float acceleration = 0.2f;
        [SerializeField] private float deceleration = 0.25f;

        private float velocity = 0f;

        private void Update()
        {
            bool forwardPressed = Input.GetKey("w");
            bool atackPressed = Input.GetKey("space");

            playerAnimator.SetBool("isAtacking", false);

            if (forwardPressed && velocity < 1)
            {
                velocity += Time.deltaTime * acceleration;
            }

            if (!forwardPressed && velocity > 0)
            {
                velocity -= Time.deltaTime * deceleration;
            }

            if (!forwardPressed && velocity < 0)
            {
                velocity = 0;
            }
            
            playerAnimator.SetFloat("Velocity", velocity);

            if (atackPressed)
            {
                playerAnimator.SetBool("isAtacking", true);
            }
        }
    }
}
