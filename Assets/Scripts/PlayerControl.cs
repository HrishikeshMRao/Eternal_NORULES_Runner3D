using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    public float slideTime = 0.75f;
    public float horAcceleration = 10f;
    public float forwardAcceleration = 10f;
    public float gravity = 20f;
    public float deceleration = 20f;
    public float forwardSpeed = 0f, horSpeed = 0f, verticleSpeed = 0f;
    public float maxForwardSpeed = 30f, maxHorSpeed = 10f;
    public float difficultyBar = 300f;
    public bool isControlEnable = true;
    public GameManager gameManager;
    public TextMeshProUGUI scoreText;
    public Vector3 velocity;
    public Animator animator;
    public CharacterController controller;
    [SerializeField] private float Rotation_speed;
    private Vector3 controllerCenter;
    private Vector3 playerVelocity;
    private float inputMovement = 0;
    private float inputVerticle = 0;
    private float effectiveGravity; // Same as gravity, but will become dowble if while jumping, player slides
    private bool isGameOver = false;
    private Quaternion initialRotation;

    
    public void OnMovement(InputAction.CallbackContext value) 
    {
        inputMovement = value.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        inputVerticle = value.ReadValue<float>();

        if(inputVerticle > 0 && controller.isGrounded && !animator.GetBool("isSliding")) {
            verticleSpeed = 7f; // Took arbitary tested value for now
            animator.SetBool("isJumping", true);
            Debug.Log("jumping");
        } else if(inputVerticle < 0 && controller.isGrounded) {
            StartSlide(false);
        } else if(inputVerticle < 0) {
            StartSlide(true);
        }
    }

    void Start() 
    {
        controller = GetComponent<CharacterController>();

        effectiveGravity = gravity;
    }

    void Update()
    {
        if(transform.position.y < 0 && !isGameOver) {
            gameManager.GameOver("isFalling");
            isGameOver = true;
        }

        if(isControlEnable)
            Move();
        else
            Decelerate();

        scoreText.text = "Score : " + transform.position.z.ToString("0");

    }

    void Move()
    {
        if(transform.position.z > difficultyBar) {
            difficultyBar *= 1.25f;
            maxForwardSpeed *= 1.1f;
            maxHorSpeed += 1;
        }

        horSpeed += inputMovement != 0f? inputMovement * horAcceleration * Time.deltaTime : - horSpeed * 0.35f * horAcceleration * Time.deltaTime;
        
        horSpeed = Mathf.Clamp(horSpeed, -maxHorSpeed, maxHorSpeed);

        if ((transform.rotation.y <= 0.4f) && (transform.rotation.y >= -0.4f))
        {
            rotation();
        }
        if (inputMovement.Equals(0f))
        {
            //initialRotation = transform.rotation;
            //Quaternion targetRotation = new Quaternion(0, 0, 0, 0);
            //transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, Rotation_speed);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        forwardSpeed += forwardAcceleration * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxForwardSpeed);

        if(!controller.isGrounded)
            verticleSpeed -= effectiveGravity * Time.deltaTime;

        playerVelocity = Vector3.forward * forwardSpeed + Vector3.right * horSpeed + Vector3.up * verticleSpeed;

        velocity = controller.velocity;
        controller.Move(playerVelocity * Time.deltaTime);

        if(controller.isGrounded)
            animator.SetBool("isJumping", false);

    }    

    private void rotation()
    {
         transform.Rotate(0,0.7f*horSpeed,0);
    }
    void Decelerate()
    {
        forwardSpeed -= deceleration * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, 0, maxForwardSpeed);

        playerVelocity = Vector3.forward * forwardSpeed;

        controller.Move(playerVelocity * Time.deltaTime); 
    }

    void StartSlide(bool wasJumping)
    {
        controllerCenter = controller.center;
        controllerCenter.y = -0.5f;

        controller.center = controllerCenter;
        controller.height = 1;

        animator.SetBool("isJumping", false);
        animator.SetBool("isSliding", true);

        Invoke("StopSliding", slideTime);

        if(wasJumping) {
            effectiveGravity = 2 * gravity;
        }
    }

    void StopSliding()
    {
        controllerCenter = controller.center;
        controllerCenter.y = -0.05f;
        
        controller.center = controllerCenter;
        controller.height = 1.9f;

        animator.SetBool("isSliding", false);

        effectiveGravity = gravity; 
    }
}
