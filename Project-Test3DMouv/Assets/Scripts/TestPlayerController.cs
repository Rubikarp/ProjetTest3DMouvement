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
        Vector2 moveValue = new Vector2(Input.GetAxis("Vertical"),Input.GetAxis("Horizontal"));
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
        
        transform.Translate(new Vector3(transform.forward.z*-moveValue.x,0,transform.forward.z*moveValue.y));
        
    }

    void AvatarRotation(){


    }

}
