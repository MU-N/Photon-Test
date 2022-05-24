
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class PlayerMotion : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] float moveSpeed;


        [Header("Ground Check")]
        [SerializeField] float jumpForce;
        [SerializeField] Transform groundCheckObject;
        [SerializeField] float groundCheckRaduis;
        [SerializeField] LayerMask whatIsGround;

        [Header("Weapon")]
        [SerializeField] Transform weaponParent;
        #endregion

        #region Private Variables

        private Rigidbody rb;

        private float horizontalInput;
        private float verticalInput;

        private float sprintSpeed;
        private float normalSpeed;

        private float movementCounter;
        private float idleCounter;

        private bool isSprinting;
        private bool isJumping;

        private Vector3 moveDirection;

        private Vector3 weaponParentOrigin;
        private Vector3 targetHeadBobPosition;

        private const string horizontalStirng = "Horizontal";
        private const string verticalStirng = "Vertical";

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            Camera.main.enabled = false;
            rb = GetComponent<Rigidbody>();

            sprintSpeed = moveSpeed * 1.75f;
            normalSpeed = moveSpeed;

            weaponParentOrigin = weaponParent.localPosition;

        }

        private void Update()
        {
            GetInput();
            CheckForSprint();
            CheckForJump();
            CheckForHeadBob();
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        #endregion

        #region Methods
        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw(horizontalStirng);
            verticalInput = Input.GetAxisRaw(verticalStirng);

            moveDirection = new Vector3(horizontalInput, 0, verticalInput);
            moveDirection.Normalize();

        }

        private void CheckForSprint()
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (isSprinting && verticalInput > 0 && !isJumping)
            {
                moveSpeed = sprintSpeed;

            }
            else
            {
                moveSpeed = normalSpeed;
            }
        }

        private void CheckForJump()
        {
            Collider[] colliders = Physics.OverlapSphere(groundCheckObject.position, groundCheckRaduis, whatIsGround);
            if (colliders.Length != 0)
            {


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(Vector3.up * jumpForce);
                    isJumping = true;
                }
            }
            else
                isJumping = false;

        }

        private void MovePlayer()
        {
            Vector3 motionVelocity = transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
            motionVelocity.y = rb.velocity.y;
            rb.velocity = motionVelocity;
        }

        private void CheckForHeadBob()
        {
            if (horizontalInput == 0 && verticalInput == 0)
            {
                //idle
                HeadBob(idleCounter, .01f, .01f);
                idleCounter += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPosition, Time.deltaTime * 2f);
            }
            else if(!isSprinting)
            {
                HeadBob(movementCounter, .035f, .035f);
                movementCounter += Time.deltaTime * 3f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPosition, Time.deltaTime * 6f);
            }
            else
            {
                HeadBob(movementCounter, .05f, .075f);
                movementCounter += Time.deltaTime * 7f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPosition, Time.deltaTime * 10f);
            }
            
        }
        private void HeadBob(float z, float xIntensity, float yIntensity)
        {
            targetHeadBobPosition =  weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity, Mathf.Sin(z * 2f) * yIntensity,0f);
           
        }

        #endregion

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheckObject.position, groundCheckRaduis);
        }
        #endregion
    }
}
