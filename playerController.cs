using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    private float moveSpeed;
    public float walkSpeed;
    public float jumpHeight;           //sprunghöhe
    public float gravityScale;         //gravitation

    public float coyoteTime;           //wie lange man nach der kante noch springen darf
    private float coyoteTimeCounter;   //Zählt Zeit runter

    public float jumpBufferTime;       //Wie lange wird der Sprung-Knopf gedrückt
    private float jumpBufferCounter;   //Zählt Zeit runter

    private Transform cameraTransform;    //Referenz zur kamera
    private PlayerControls controls;
    private CharacterController controller;    //Referenz der Komponente
    private Vector3 playerVelocity;            //speicherort der Fall-Geschwindigkeit
    private bool groundedPlayer;               //steht der spieler auf Boden
    private float turnSmoothVelocity;
    public float turnSmoothTime;


    private void Awake() 
     {     
        controls = new PlayerControls();
        controller = GetComponent<CharacterController>();  //holt die Komponente

        if (Camera.main != null) cameraTransform = Camera.main.transform;          //Hauptkamera wird für die bestimmung von "Vorne" geholt
     }

    private void OnEnable() => controls.Enable();  //die kontroller wird aktiviert
    private void OnDisable() => controls.Disable(); //die Kontroller wird deaktiviert
    
    private void Update()
    {
        
        groundedPlayer = controller.isGrounded;  //der charactercontroller kontrolliert automatisch

        if (groundedPlayer)                   //coyoteTimelock
        {
            coyoteTimeCounter = coyoteTime;   //am Boden ist der Timer aufgeladen

            if (playerVelocity.y < 0)         //am Boden wird Fallgeschwindigkeit Resetted
            {
                playerVelocity.y = -0.5f;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;   //in der Luft läuft Zeit ab
        }


        if (controls.Controls.Jump.triggered)  //jump-Buffer
        {

            jumpBufferCounter = jumpBufferTime;   //bei Knopfdruck wird Buffer geladen

        }else
        {

            jumpBufferCounter -= Time.deltaTime;          //Zeit läuft sonst ab
        }


        if (groundedPlayer && playerVelocity.y < 0)   //falls spieler auf boden ist und fallgeschwindigkeit negativ
        {
            playerVelocity.y = -0.5f;                 //fallgeschwindigkeit wird auf -0.5 zurückgesetzt, für bodenklebung
        }

    
        Vector2 inputVector = controls.Controls.Movement.ReadValue<Vector2>();   //spannt raum, in dem sich spieler bewegt
        Vector3 moveDir = Vector3.zero;

        if (inputVector.magnitude >= 0.1f)  //bewegt sich der spieler überhaupt?
        {
            float targetAngle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;  //Winkel berechnung, in die man sich bewegt: Atan2 berechnet Winkel aus StickInput
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //winkel glättung => der spieler soll sich elegant drehen, nicht hin schnappen

            transform.rotation = Quaternion.Euler(0f, angle, 0f);       //drehung
            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  //Bewegungsrichtung aus drehung berechnen

            moveSpeed = controls.Controls.Sprint.IsPressed() ? 2 * walkSpeed : walkSpeed;  // sprintmechanik
            moveDir = direction.normalized * moveSpeed;  //Bewegungspeicherung
        }

        if (controls.Controls.Jump.triggered && groundedPlayer)        //sprungmechanik
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityScale);
        }

        playerVelocity.y += gravityScale * Time.deltaTime;   //Gravitation auf y-Achse draufrechnen
        controller.Move((moveDir + playerVelocity) * Time.deltaTime);   //move wird einmal ausgeführt, indem horizontal und vertikal addiert werden
    }
}
