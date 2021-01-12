using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public float MovementSpeed = 2f;
    public float Gravity = 9.8f;
    private float velocity = 0;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * this.MovementSpeed;
        float vertical = Input.GetAxis("Vertical") * this.MovementSpeed;
        characterController.Move((Vector3.right * horizontal + Vector3.forward * vertical) * Time.deltaTime);

        if (characterController.isGrounded)
        {
            velocity = 0;
        }
        else
        {
            velocity -= Gravity * Time.deltaTime;
            characterController.Move(new Vector3(0, velocity, 0));
        }
    }
}
