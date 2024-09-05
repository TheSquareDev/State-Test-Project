using System.Collections;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.InputSystem;

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
            inputValue = playerInputs.CharacterControls.Movement.ReadValue<Vector2>();

            float movementOffset = Time.deltaTime * acceleration;

            velocityZ += movementOffset * inputValue.y;
            velocityX += movementOffset * inputValue.x;

            animator.SetFloat("VelocityX", velocityX);
            animator.SetFloat("VelocityZ", velocityZ);
        }
        private void DeceleratePlayerMovementAnimation()
        {
            float movementOffset = Time.deltaTime * deceleration;

            if (velocityZ != 0)
            {
                velocityZ += movementOffset * -velocityZ;
            }

            if (velocityX != 0)
            {
                velocityX += movementOffset *  -velocityX;
            }

            velocityZ = Mathf.Clamp(velocityZ, -1, 1);
            velocityX = Mathf.Clamp(velocityX, -1, 1);
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
