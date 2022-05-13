using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
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
    [SerializeField] AudioSource AudioSource = default;
    [SerializeField] AudioClip[] concreteClips = default;
    float footstepTimer = 0;

    [Header("Pickup system")]
    public float pickupDistance = 100.0f;
    public InteractableComp sensedObject = null;
    public Camera fpsCamera = null;
    [SerializeField] AudioClip[] PickupClips = default;
    [SerializeField] AudioClip MoneyClip = default;
    [SerializeField] AudioClip SushiClip = default;
    public float currentCanAmount = 0;
    public double currentMoneyAmount = 156.60;
    [SerializeField] double canValue = 156.71;
    public GameObject teleportTo;
    public GameObject foodComponent;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    bool beingTeleported = false;

    //tasks
    public float stageTask = 0;
    float taskTimer = 0;
    [SerializeField] AudioClip TaskChangeSound = default;

    float stage = 0;
    float timerToEnd = 0;

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
        //Update task text
        TaskManager();
        //Lock movement if we reach stage 1
        if(stage != 1){
            UpdateMovement();
        }
        UpdateMouseLook();

        //Teleport to dinner table
        if(stage==1){
            teleportToPosition();
            timerToEnd += Time.deltaTime;
            if(timerToEnd > 10){
                GetComponent<FadeScript>().ShowUI();
            }
            if(timerToEnd > 15){
                Application.Quit();
            }
        }
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

        //Define our camera and connect a screenpointtoray
        Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Check with raycast what object we are pointing to
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
        
        //Manage which object is being clicked
        if(Input.GetMouseButton(0) && sensedObject){
            if((sensedObject.pickupType.ToString() == "EPC_VendingMachine") && currentCanAmount > 0){
                currentMoneyAmount = currentMoneyAmount + (currentCanAmount * canValue);
                AudioSource.PlayOneShot(MoneyClip);
                currentCanAmount = 0;
            }
            if((sensedObject.pickupType.ToString() == "EPC_SushiMachine") && currentMoneyAmount > 750 && stageTask == 4){
                beingTeleported = true;
                currentMoneyAmount = currentMoneyAmount - 750;
                AudioSource.PlayOneShot(SushiClip);
                stage = 1;
                stageTask = 5;
                foodComponent.SetActive(true);
                AudioSource.PlayOneShot(TaskChangeSound);
            }
            if((sensedObject.pickupType.ToString() != "EPC_VendingMachine") && (sensedObject.pickupType.ToString() != "EPC_SushiMachine")){
                Debug.LogFormat("sensed {0} of Type {1} amount {2}",sensedObject.name,sensedObject.pickupType,sensedObject.amount );
                AudioSource.PlayOneShot(PickupClips[Random.Range(0,PickupClips.Length - 1)]);
                Destroy(sensedObject.gameObject);
                currentCanAmount += 1;
                sensedObject = null;
            }
        }


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
        
        if(!beingTeleported){
            controller.Move(velocity * Time.deltaTime);
        }

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
        if(velocity.magnitude >= 1){
            footstepTimer += Time.deltaTime;

            float seconds = footstepTimer % 60;
            if(seconds >= 0.8){
                AudioSource.PlayOneShot(concreteClips[Random.Range(0,concreteClips.Length - 1)]);
                footstepTimer = 0;
            }
        }
    }

    void teleportToPosition(){
        controller.transform.rotation = new Quaternion(0,(float)-0.7,0,(float)0.7);
        controller.transform.position = teleportTo.transform.position;
        //beingTeleported = false;
    }

    void TaskManager(){
        Debug.Log(stageTask);
        taskTimer += Time.deltaTime;
        if(taskTimer > 7 && stageTask == 0){
            stageTask = 1;
            AudioSource.PlayOneShot(TaskChangeSound);
        }
        if(currentCanAmount > 1 && stageTask == 1){
            stageTask = 2;
            AudioSource.PlayOneShot(TaskChangeSound);
        }
        if(currentCanAmount > 5 && stageTask == 2){
            stageTask = 3;
            AudioSource.PlayOneShot(TaskChangeSound);
        }
        if(currentMoneyAmount > 750 && stageTask == 3){
            stageTask = 4;
            AudioSource.PlayOneShot(TaskChangeSound);
        }
    }

    // private void OnTriggerEnter(Collider other){
    //     if(other.CompareTag("Player"))
    //         Vocals.instance.Say(clipToPlay);
    // }
}
