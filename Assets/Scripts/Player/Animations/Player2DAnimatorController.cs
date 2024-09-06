using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerAnimation
{
    public class Player2DAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        [Range(0,5)] 
        [SerializeField] private float comboResetWindown;

        private Vector2 velocity = Vector2.zero;
        private int comboCount = -1;
        private bool isWaitingAttack;
        private Coroutine attackCoroutine;
        private PlayerInputsActions playerInputs;
        private Vector2 inputValue = Vector2.zero;

        private void Awake()
        {
            playerInputs = new PlayerInputsActions();
        }

        private void Update()
        {
            UpdatePlayerMovementAnimation();
            DeceleratePlayerMovementAnimation();
        }

        private void OnEnable()
        {
            playerInputs.Enable();
            playerInputs.CharacterControls.Attack.started += SetPlayerAttack;

        }

        private void OnDisable()
        {
            playerInputs.Disable();
            playerInputs.CharacterControls.Attack.started -= SetPlayerAttack;
        }

        private void OnDestroy()
        {
            playerInputs.Dispose();
        }

        public void SetPlayerAttack(InputAction.CallbackContext value)
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

        private void UpdatePlayerMovementAnimation()
        {
            float movementOffset = Time.deltaTime * acceleration;

            velocity.y += movementOffset * characterController.velocity.z;
            velocity.x += movementOffset * characterController.velocity.x;

            animator.SetFloat("VelocityX", velocity.x);
            animator.SetFloat("VelocityZ", velocity.y);
        }
        private void DeceleratePlayerMovementAnimation()
        {
            float movementOffset = Time.deltaTime * deceleration;

            if (velocity.y != 0)
            {
                velocity.y += movementOffset * -velocity.y;
            }

            if (velocity.x != 0)
            {
                velocity.x += movementOffset *  -velocity.x;
            }

            if (velocity.sqrMagnitude < 0.00035f)
            {
                velocity = Vector2.zero;
            }

            velocity.y = Mathf.Clamp(velocity.y, -1, 1);
            velocity.x = Mathf.Clamp(velocity.x, -1, 1);
        }

        private void Combo()
        {
            if (comboCount == 2)
            {
                comboCount = -1;
            }

            Debug.Log(comboCount);
            attackCoroutine =  StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            comboCount++;
            animator.SetInteger("Combo", comboCount);
            yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(1).IsName("Idle"));
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);
            yield return new WaitForSeconds(info.length);
            attackCoroutine = null;

            if (isWaitingAttack)
            {
                isWaitingAttack = false;
                Combo();
            }
            else
            {
                comboCount = -1;
                animator.SetInteger("Combo",comboCount);
            }
        }
    }
}
