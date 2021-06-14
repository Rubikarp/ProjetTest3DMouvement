using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TestPlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private float currentSpeed = 1.0f;

    private Rigidbody rb;
    private CapsuleCollider col;

    private Vector2 inputValue;
    private Vector2 moveValue;

    private bool isRunning = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        //Stick Input
        inputValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputValue.magnitude > 1)
        {
            inputValue.Normalize();
        }

        //IsRunning
        if (inputValue.magnitude > 0.5 && Input.GetButton("Running"))
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

        moveValue = inputValue.x * orient2dHori + inputValue.y * orient2dVert;

        transform.Translate(
            moveValue.x * currentSpeed * Time.deltaTime, 
            0,
            moveValue.y * currentSpeed * Time.deltaTime);


        //Debug
        Debug.DrawRay(Vector3.zero, new Vector3(orient2dVert.x, 0, orient2dVert.y), Color.red);
        Debug.DrawRay(Vector3.zero, new Vector3(orient2dHori.x, 0, orient2dHori.y), Color.green);

        Debug.DrawRay(Vector3.zero, new Vector3(moveValue.x, 0, moveValue.y), Color.yellow);

    }

    private void AvatarRotation()
    {
    }
}