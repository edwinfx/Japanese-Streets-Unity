using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 0.5f;
    [SerializeField] bool lockCursor = true;
    [SerializeField] float gravity = -13.0f;

    [SerializeField][Range(0.0f,0.5f)] float moveSmoothTime = 0.03f;
    [SerializeField][Range(0.0f,0.5f)] float mouseSmoothTime = 0.03f;

    [Header("Headbob Settings")]
    [SerializeField] bool canUseHeadBob = true;
    [SerializeField] float walkBobSpeed = 14f;
    [SerializeField] float walkBobAmount = 0.5f;
    float defaultYPos = 0;
    float timer;

    [Header("Footstep Settings")]
    [SerializeField] float baseStepSpeed = 0.5f;
    [SerializeField] AudioSource FootstepAudioSource = default;
    [SerializeField] AudioClip[] concreteClips = default;
    float footstepTimer = 0;

    [Header("Pickup system")]
    public float pickupDistance = 100.0f;
    public InteractableComp sensedObject = null;
    public Camera fpsCamera = null;
    
    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //Hide Cursor
        if(lockCursor){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        defaultYPos = playerCamera.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateMouseLook();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        //Smooth Camera
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

        Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * pickupDistance, Color.red);

        if(Physics.Raycast(ray, out hit, pickupDistance)){
            InteractableComp obj = hit.collider.GetComponent<InteractableComp>();
            if(obj){
                sensedObject = obj;
            } else {
                sensedObject = null;
            }
        }else{
            sensedObject = null;
        }

        if(Input.GetMouseButton(0) && sensedObject){
            Debug.Log("sensed");
            Destroy(sensedObject.gameObject);
            sensedObject = null;
        }

        Debug.Log(currentMouseDelta.x);

    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        //Smooth movement
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        //Check grounded
        if(controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        //Headbob
        if(canUseHeadBob){
            if(Mathf.Abs(currentDir.x)>0.1f || Mathf.Abs(currentDir.y)>0.1f)
            {
                timer += Time.deltaTime * walkBobSpeed;
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * walkBobAmount,
                    playerCamera.transform.localPosition.z);
            }
        }

        //Handle Footstep sounds
        //footstepTimer -= Time.deltaTime * walkBobSpeed;
    
        if(velocity.magnitude >= 1){
            footstepTimer += Time.deltaTime;

            float seconds = footstepTimer % 60;
            if(seconds >= 0.8){
                FootstepAudioSource.PlayOneShot(concreteClips[Random.Range(0,concreteClips.Length - 1)]);
                footstepTimer = 0;
            }
        }
    }
}
