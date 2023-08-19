using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour
{
    public float horAcceleration = 10f;
    public float forwardAcceleration = 10f;
    public float gravity = 9.8f;
    public float deceleration = 20f;
    public float forwardSpeed = 0f, horSpeed = 0f;
    public float maxForwardSpeed = 30f, maxHorSpeed = 10f;
    public float difficultyBar = 300f;
    public bool isControlEnable = true;
    public GameManager gameManager;
    public TextMeshProUGUI scoreText;
    public Vector3 velocity;
    public CharacterController controller;
    [SerializeField] private float Rotation_speed;
    private Vector3 playerVelocity;
    private float inputMovement = 0;
    private bool isGameOver = false;
    private Quaternion initialRotation;
    
    public void OnMovement(InputAction.CallbackContext value) 
    {
        inputMovement = value.ReadValue<float>();
    }

    void Start() 
    {
        controller = GetComponent<CharacterController>();
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

        playerVelocity = Vector3.forward * forwardSpeed + Vector3.right * horSpeed + Vector3.down * gravity;

        velocity = controller.velocity;
        controller.Move(playerVelocity * Time.deltaTime);
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
}
