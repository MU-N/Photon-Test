
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

namespace Nasser.io.PUN2
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        #region Inspector Variables
        [SerializeField] float moveSpeed;

        [Header("Players Health")]
        [SerializeField] int maxHealth;

        [Header("Players Layer")]
        [SerializeField] int playersLayerIndex;

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

        private int currentHealth;

        private Vector3 moveDirection;

        private Vector3 weaponParentOrigin;
        private Vector3 targetHeadBobPosition;

        private const string horizontalStirng = "Horizontal";
        private const string verticalStirng = "Vertical";

        PhotonView view;

        private GameObject camerasParent;

        private PlayerSpwaner manager;
        private Weapon weapon;

        private Transform Canvas;
        private Image hpImage;
        private TMP_Text hpText;
        private TMP_Text ammoText;


        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            view = GetComponent<PhotonView>();
            if (!view.IsMine)
            {
                gameObject.layer = playersLayerIndex;
                return;
            }

            if (Camera.main)
                Camera.main.enabled = false;
            rb = GetComponent<Rigidbody>();
            weapon = GetComponent<Weapon>();
            manager = GameObject.Find("Spwan Locations").GetComponent<PlayerSpwaner>();
            sprintSpeed = moveSpeed * 1.75f;
            normalSpeed = moveSpeed;

            weaponParentOrigin = weaponParent.localPosition;

            camerasParent = transform.GetChild(1).gameObject;

            camerasParent.SetActive(view.IsMine);

            currentHealth = maxHealth;

            Canvas = GameObject.Find("Canvas").transform;
            hpImage = Canvas.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            hpText = Canvas.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
            ammoText = Canvas.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            UpdateHealth();


        }



        private void Update()
        {
            if (!view.IsMine) return;

            GetInput();
            CheckForSprint();
            CheckForJumpInput();
            CheckForHeadBob();
            UpdateHealth();
            UpdateAmmo();

        }
        private void FixedUpdate()
        {
            if (!view.IsMine) return;
            MovePlayer();
            CheckForJumpPhysics();
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



        private void CheckForJumpInput()
        {
            Collider[] colliders = Physics.OverlapSphere(groundCheckObject.position, groundCheckRaduis, whatIsGround);
            if (colliders.Length != 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                }
            }
            else
                isJumping = false;

        }

        private void CheckForJumpPhysics()
        {
            if(isJumping)
            rb.AddForce(Vector3.up * jumpForce);
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
            else if (!isSprinting)
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
            float aimAdj = 1f;
            if (weapon.isAiming) aimAdj = 0.1f;

            targetHeadBobPosition = weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity * aimAdj, Mathf.Sin(z * 2f) * yIntensity * aimAdj, 0f);

        }

        private void UpdateHealth()
        {
            hpImage.fillAmount = Mathf.Lerp(hpImage.fillAmount, (currentHealth / 100.0f), Time.deltaTime * 7f);
            //hpText.text = currentHealth.ToString();
            hpText.text = CustomLerp(hpImage.fillAmount * 100.0f, currentHealth, Time.deltaTime * 7f).ToString("F0");
        }

        private void UpdateAmmo()
        {
            weapon.UpdateAmmo(ammoText);
        }




        #endregion

        #region Public Methods

        [PunRPC]
        public void TakeDamageR(int _amount)
        {
            if (view.IsMine)
            {
                currentHealth -= _amount;
                if (currentHealth < 0)
                {
                    manager.Spwan();
                    PhotonNetwork.Destroy(gameObject);
                }
                UpdateHealth();
            }
        }

        private float CustomLerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
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
