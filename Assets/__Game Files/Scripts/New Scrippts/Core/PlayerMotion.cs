
using UnityEngine;

namespace Nasser.io.PUN2
{
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField] float moveSpeed;


        [Header("Ground Check")]
        [SerializeField] float jumpForce;
        [SerializeField] Transform groundCheckObject;
        [SerializeField] float groundCheckRaduis;
        [SerializeField] LayerMask whatIsGround;


        private Rigidbody rb;

        private float horizontalInput;
        private float verticalInput;

        private float sprintSpeed;
        private float normalSpeed;



        private bool isSprinting;
        private bool isJumping;

        private Vector3 moveDirection;

        private const string horizontalStirng = "Horizontal";
        private const string verticalStirng = "Vertical";
        private void Start()
        {
            Camera.main.enabled = false;
            rb = GetComponent<Rigidbody>();

            sprintSpeed = moveSpeed * 1.75f;
            normalSpeed = moveSpeed;

        }

        private void Update()
        {
            GetInput();
            CheckForSprint();
            CheckForJump();
        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

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
            if (colliders.Length!=0)
            {

               
                if (Input.GetKeyDown(KeyCode.Space) )
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(groundCheckObject.position, groundCheckRaduis);
        }
    }
}
