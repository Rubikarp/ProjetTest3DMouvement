using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TestPlayerController : MonoBehaviour
{
    [Header("Run Stat")]
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 2.0f;
    [Header("Jump Stat")]
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float jumpSpeed = 6.0f;
    [SerializeField] private float fallSpeed = 10.0f;
    private float startJumpPos;
    

    [Header("Look Stat")]
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float lookSpeedX = 1.0f;
    [SerializeField] private float lookSpeedY = 2.0f;
    [SerializeField, Range(15,90)] private float lookLimit = 45.0f;

    private float currentSpeed = 1.0f;
    private float rotationMouse = 0;

    [Header("Component")]
    [SerializeField] private Camera playerCamera;
    private Rigidbody rb;
    private CapsuleCollider col;

    [Header("Input")]
    private Vector2 inputMoveValue;
    private Vector2 inputCamValue;
    private bool inputJump;
    

    [Header("State")]
    private bool isRunning = false;
    private bool isJumping = false;
    private bool isFalling = false;
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        GetInput();
        CheckState();
    }

    private void GetInput()
    {
        //Stick Input Move
        inputMoveValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Stick Input Cam
        inputCamValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //Stick Input Cam
        inputJump = Input.GetButton("Jump");

        if (inputMoveValue.magnitude > 1) { inputMoveValue.Normalize(); }

    }

    private void CheckState()
    {
        //IsRunning
        if (inputMoveValue.magnitude > 0.5 && Input.GetButton("Running"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        
    }

    private void FixedUpdate()
    {
        AvatarMove();
        AvatarRotation();
        AvatarJump();
        AvatarFall();
    }

    private void AvatarMove()
    {
        //CurrentRunSpeed
        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        Vector3 avatarOrientation = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        Vector2 orient2dVert = new Vector2(avatarOrientation.x, avatarOrientation.z);
        Vector2 orient2dHori = -Vector2.Perpendicular(orient2dVert);

        Vector2 moveValue = inputMoveValue.x * orient2dHori + inputMoveValue.y * orient2dVert;

        transform.Translate( moveValue.x * currentSpeed * Time.deltaTime, 0,  moveValue.y * currentSpeed * Time.deltaTime, Space.World);

        # region DebugMoveDir
        Debug.DrawRay(transform.position, new Vector3(orient2dVert.x, 0, orient2dVert.y) * 2, Color.red);
        Debug.DrawRay(transform.position - (new Vector3(orient2dHori.x, 0, orient2dHori.y) * 2), new Vector3(orient2dHori.x, 0, orient2dHori.y) * 4, Color.green);

        Debug.DrawRay(transform.position, new Vector3(moveValue.x, 0, moveValue.y), Color.yellow);
        #endregion
    }

    private void AvatarRotation()
    {
        // Axis Y
        rotationMouse += -inputCamValue.y * lookSpeedY;
        // Check if rotationMouse is between lookLimit : if not return the case value : rotationMouse>lookLimit return lookLimit ;  rotationMouse<-lookLimit return -lookLimit
        rotationMouse = Mathf.Clamp(rotationMouse, -lookLimit, lookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationMouse, 0, 0);

        // Axis X
        transform.rotation *= Quaternion.Euler(0, inputCamValue.x * lookSpeedX, 0);
    }

    private void AvatarJump()
    {
        if (inputJump && !isJumping && !isFalling){
            isJumping = true;
            isFalling = false;
            startJumpPos = transform.position.y;
            StartCoroutine("Jump");
        }

    }

    private void AvatarFall()
    {
        if (!isJumping && !isFalling){
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1f))
            {
                isFalling = true;
                StartCoroutine("Fall");
            }
        }
    }

    private IEnumerator Fall(){
        while(isFalling)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1f))
            {
                isFalling = false;
            }
            else
            {
                transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);    
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Jump()
    {
        while(isJumping)
        {
            if(!isFalling)
            {
                if (transform.position.y >= startJumpPos+jumpHeight)
                {
                    isFalling = true;
                }
                else
                {
                    transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1f))
                {
                    isJumping = false;
                    isFalling = false;
                }
                else
                {
                    transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);    
                }

            }
            yield return new WaitForEndOfFrame();
        }
    }
}