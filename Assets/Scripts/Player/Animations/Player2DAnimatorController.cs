using System.Collections;
using UnityEngine;

namespace PlayerAnimation
{
    public class Player2DAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        [Range(0,5)] 
        [SerializeField] private float comboResetWindown;

        private float velocityZ = 0f;
        private float velocityX = 0f;
        private int comboCount = 0;
        private bool isWaitingAttack;
        private Coroutine attackCoroutine;
        private Coroutine resetComboCoroutine;


        private void Update()
        {
            bool forwardPress = Input.GetKey("w");
            bool backPress = Input.GetKey("s");
            bool rightPress = Input.GetKey("d");
            bool leftPress = Input.GetKey("a");
            bool isAttacking = Input.GetKeyDown(KeyCode.Space);

            DeceleratePlayerVelocity();
            SetPlayerMovement(forwardPress, backPress, rightPress, leftPress);

            if (isAttacking)
            {
                if (attackCoroutine != null)
                {
                    isWaitingAttack = true;
                }
                else
                {
                    Combo();
                }
            }
        }

        private void SetPlayerMovement(bool forwardPress, bool backPress, bool rightPress, bool leftPress)
        {
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

        private void Combo()
        {
            string attack = "";

            comboCount = Mathf.Clamp(comboCount, 0, 2);

            if (comboCount == 0)
            {
                attack = "Attacking";
            }
            if (comboCount == 1)
            {
                attack = "Attacking2";
            }
            if (comboCount == 2)
            {
                attack = "Attacking3";
                comboCount = -1;
            }

            if (resetComboCoroutine != null)
            {
                StopCoroutine(resetComboCoroutine);
            }

            Debug.Log(comboCount);
            attackCoroutine =  StartCoroutine(Attack(attack));

        }

        private IEnumerator Attack(string attack)
        {
            animator.SetTrigger(attack);
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);
            yield return new WaitForSeconds(info.length);
            comboCount++;
            attackCoroutine = null;

            if (isWaitingAttack)
            {
                isWaitingAttack = false;
                Combo();
            }
            
            resetComboCoroutine = StartCoroutine(ResetCombo());
        }

        private IEnumerator ResetCombo()
        {
            yield return new WaitForSeconds(comboResetWindown);
            comboCount = 0;
        }

    }
}
