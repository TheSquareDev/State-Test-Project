using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAnimation
{
    public class PlayerBaseAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        private void Update()
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.SetBool("isAtacking", false);

            if (Input.GetKey("w"))
            {
                playerAnimator.SetBool("isWalking", true);
            }
            if (Input.GetKey("space"))
            {
                playerAnimator.SetBool("isAtacking", true);
            }
        }

    }
}
