using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class TestPlayerController : MonoBehaviour
{

    [SerializeField] float walkSpeed = 1.0f;
    [SerializeField] float runSpeed = 2.0f;
    [SerializeField] float rotationSpeed = 2.0f;

    float currentSpeed = 1.0f;
    Rigidbody avatarRB;
    CapsuleCollider avatarCol;

    Vector2 moveValue;

    bool isRunning = false;


    void Start()
    {
        avatarRB = GetComponent<Rigidbody>();
        avatarCol = GetComponent<CapsuleCollider>();
    }

    void Update() {
    }
    void FixedUpdate() {
        AvatarMove();
        AvatarRotation();  
    }

    void AvatarMove(){
        Vector2 moveValue = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        if (moveValue.magnitude>1){
            moveValue.Normalize();
        }
        if (moveValue.magnitude>0.5 && Input.GetButton("Running"))
        {
            isRunning = true;
        }else{
            isRunning = false;
        }


        if (isRunning){
            currentSpeed = runSpeed;
        }else{
            currentSpeed = walkSpeed;
        }

        Vector3 avatarOrientation = Vector3.ProjectOnPlane(transform.forward,Vector3.up);
        Vector2 avatarOrientation2D = new Vector2(avatarOrientation.x,avatarOrientation.z); 

        float angle = Mathf.Atan2(moveValue.x,moveValue.y) - Mathf.Atan2(avatarOrientation2D.x,avatarOrientation2D.y);
        Debug.Log(angle.ToString());
        Debug.Log(moveValue.ToString());
        moveValue.x = Mathf.Cos(angle) - Mathf.Sin(angle);
        moveValue.y = Mathf.Sin(angle) + Mathf.Cos(angle);
        Debug.Log(moveValue.ToString());



        transform.Translate(moveValue.x*currentSpeed*Time.deltaTime,0,moveValue.y*currentSpeed*Time.deltaTime);
        
        
    }

    void AvatarRotation(){


    }

}
