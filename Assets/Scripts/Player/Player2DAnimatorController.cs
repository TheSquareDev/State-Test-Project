using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAnimation
{
    public class Player2DAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        private float velocityZ = 0f;
        private float velocityX = 0f;
        private int comboCount = 0;
        private List<int> comboChain= new List<int>();

        private void Update()
        {
            bool forwardPress = Input.GetKey("w");
            bool backPress = Input.GetKey("s");
            bool rightPress = Input.GetKey("d");
            bool leftPress = Input.GetKey("a");
            bool isAttacking = Input.GetKeyDown(KeyCode.Space);

            DeceleratePlayerVelocity();

            if (forwardPress && velocityZ < 1)
            {
                velocityZ += Time.deltaTime * acceleration;
            }
            if (backPress && velocityZ > -1)
            {
                velocityZ -= Time.deltaTime * acceleration;
            }
            if (rightPress && velocityX < 1)
            {
                velocityX += Time.deltaTime * acceleration;
            }
            if (leftPress && velocityX > -1)
            {
                velocityX -= Time.deltaTime * acceleration;
            }
            animator.SetFloat("VelocityZ", velocityZ);
            animator.SetFloat("VelocityX", velocityX);

            if (isAttacking)
            {
                atacar();
            }
        }
      
        private void atacar()
        {
            comboCount++;
            StartCoroutine(Combo());
        }

        private IEnumerator Combo()
        {
            int currentCombo = comboCount;
            animator.SetTrigger("Attacking");
            yield return new WaitForSeconds(3);

            if (comboCount <= currentCombo)
            {
                StopCoroutine(Combo());
            }
            int newCurrentCombo = comboCount;
            animator.SetTrigger("Attacking2");
            yield return new WaitForSeconds(3);

            if (comboCount > newCurrentCombo)
            {
                StopCoroutine(Combo());
            }
            animator.SetTrigger("Attacking3");
            yield return new WaitForSeconds(3);
            comboCount = 0;

        }

        private void DeceleratePlayerVelocity()
        {
            if (velocityZ != 0)
            {
                velocityZ += Time.deltaTime * deceleration * (0 - velocityZ);
            }

            if (velocityX != 0)
            {
                velocityX += Time.deltaTime * deceleration * (0 - velocityX);
            }

            velocityZ = Mathf.Clamp(velocityZ, -1, 1);
            velocityX = Mathf.Clamp(velocityX, -1, 1);
        }
    }
}
