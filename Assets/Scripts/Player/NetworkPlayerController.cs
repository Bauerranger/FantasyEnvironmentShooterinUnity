using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    [System.NonSerialized]
    public bool dies = false;
    [System.NonSerialized]
    public bool isDead = false;
    [SerializeField]
    private float cursorDistance = 3.00f;
    [SerializeField]
    private Transform ShotStartingPoint;
    private Transform ShotTargetPoint;
    private string moveRightKey = "d";
    private string moveLeftKey = "a";
    private string moveJumpKey = "space";
    private float addVelocity = 6;
    private float jumpHight = 8;
    private float smoothTime = 0.2f;
    private float cameraDistance = -15f;
    private float cameraOffsetX = 5.5f;
    private float cameraOffsetY = 5f;
    private bool menuOpen = false;
    private bool walksRight = false;
    private bool walksLeft = false;
    private bool runs = false;
    private bool startJump = false;
    private bool jumps = false;
    private bool attacks = false;
    private bool attackAnimationIsDone = true;
    private Rigidbody characterRigidbody;
    private GameObject cameraMover;
    private Animator animator;
    private Vector3 zeroVelocity = Vector3.zero;
    private Vector3 cameraAngle = new Vector3(23.663f, 0, 0);
    private string[] animationNames = new string[3] { "Run", "Jump", "Melee Attack" };
    private GameObject menuCanvas;
    public bool inputIsActive { get; set; }
    [System.NonSerialized]
    public bool takeDamage;

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }
        cameraMover = GameObject.FindGameObjectWithTag("CameraMover");
        cameraMover.transform.position = new Vector3(characterRigidbody.transform.position.x + cameraOffsetX, characterRigidbody.transform.position.y + cameraOffsetY, cameraDistance);
        cameraMover.transform.rotation = Quaternion.Euler(cameraAngle);
        Cursor.visible = false;
        EventManager.movementMethods += CameraMovement;
        EventManager.movementMethods += Rotator;
        EventManager.movementMethods += ShotPointer;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        menuCanvas = GameObject.FindGameObjectWithTag("main_Menu");
        inputIsActive = true;
        EventManager.dieMethods += ProxyCommandDie;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (inputIsActive && !isDead)
            FetchInput();
        UpdateMethods();
        if (isDead)
        {
            inputIsActive = false;
            characterRigidbody.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        EventManager.Movement();
        EventManager.Attack();
        EventManager.ObservePlayerStatus();
        if (dies && !isDead)
            EventManager.Die();
        MoveStop();
        EventManager.Menu();
    }

    void UpdateMethods()
    {
        if (walksLeft && !walksRight)
        {
            EventManager.movementMethods += Cmd_MoveLeft;
        }

        if (walksRight && !walksLeft)
        {
            EventManager.movementMethods += Cmd_MoveRight;
        }

        if (startJump && !jumps)
        {
            EventManager.movementMethods += Cmd_MoveJump;
        }

        if (attacks && attackAnimationIsDone)
        {
            EventManager.attackMethods += Cmd_Attack;
        }
        if (!attacks)
        {
            EventManager.attackMethods -= Cmd_Attack;
        }
        if (!walksRight)
        {
            EventManager.movementMethods -= Cmd_MoveRight;
        }
        if (!walksLeft)
        {
            EventManager.movementMethods -= Cmd_MoveLeft;
        }

        if (jumps)
        {
            EventManager.movementMethods -= Cmd_MoveJump;
        }
    }

    void FetchInput()
    {

        if (Input.GetKey(moveRightKey) && !walksLeft && !jumps)
        {
            walksRight = true;
            runs = true;
        }
        if (Input.GetKeyUp(moveRightKey))
        {
            walksRight = false;
        }

        if (Input.GetKey(moveLeftKey) && !walksRight && !jumps)
        {
            walksLeft = true;
            runs = true;
        }
        if (Input.GetKeyUp(moveLeftKey))
        {
            walksLeft = false;
        }

        if (Input.GetKeyDown(moveJumpKey) && !jumps)
        {
            startJump = true;
        }

        if (!Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
        {
            runs = false;
        }

        if (Input.GetMouseButton(0) && attackAnimationIsDone)
        {
            attacks = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            attacks = false;
        }
    }

    void OpenMenu()
    {
        if (menuCanvas.GetComponent<GUIManager>() != null)
            menuCanvas.GetComponent<GUIManager>().openMenu();
    }

    void CloseMenu()
    {
        if (menuCanvas.GetComponent<GUIManager>() != null)
            menuCanvas.GetComponent<GUIManager>().closeMenu();
    }

    void CameraMovement()
    {
        Vector3 cameraOffsetNeutral = new Vector3(characterRigidbody.transform.position.x, characterRigidbody.transform.position.y + cameraOffsetY, cameraDistance);
        Vector3 cameraOffsetPos = new Vector3(characterRigidbody.transform.position.x + cameraOffsetX, characterRigidbody.transform.position.y + cameraOffsetY, cameraDistance);
        Vector3 cameraOffsetNeg = new Vector3(characterRigidbody.transform.position.x - cameraOffsetX, characterRigidbody.transform.position.y + cameraOffsetY, cameraDistance);
        cameraMover.transform.position = Vector3.SmoothDamp(cameraMover.transform.position, cameraOffsetNeutral, ref zeroVelocity, smoothTime);
        if (walksRight && !walksLeft)
        {
            cameraMover.transform.position = Vector3.SmoothDamp(cameraMover.transform.position, cameraOffsetPos, ref zeroVelocity, smoothTime);
        }

        if (walksLeft && !walksRight)
        {
            cameraMover.transform.position = Vector3.SmoothDamp(cameraMover.transform.position, cameraOffsetNeg, ref zeroVelocity, smoothTime);
        }
    }

    [Command]
    void Cmd_Attack()
    {
        if (attackAnimationIsDone && !dies)
        {
            attackAnimationIsDone = false;
            animator.SetTrigger("Melee Attack");
        }
    }

    void ShotPointer()
    {
        Cursor.visible = true;
        Plane playerPlane = new Plane(transform.right, ShotStartingPoint.position);
        float outEnterPoint = 0.0f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool targetFound = false;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == "Enemy")
            {
                ShotTargetPoint.position = hit.transform.position + new Vector3(0,1.8f,0);
                targetFound = true;
            }
        }

        if (!targetFound)
        {
            playerPlane.Raycast(ray, out outEnterPoint);
            Vector3 mouseWorldPosition = ray.GetPoint(outEnterPoint);

            Vector3 towardsMouse = (mouseWorldPosition - ShotStartingPoint.position).normalized;
            float angle = Mathf.Acos(Vector3.Dot(towardsMouse, transform.forward)) * Mathf.Rad2Deg;

            if (angle >= 90)
            {
                Vector3 cachedForward = transform.forward;
                Vector3.OrthoNormalize(ref cachedForward, ref towardsMouse);
            }

            ShotTargetPoint.position = ShotStartingPoint.position + towardsMouse * cursorDistance;
        }
    }

    public void SpawnShot()
    {
        EventManager.Shoot();
    }

    public void AnimationEnds()
    {
        attackAnimationIsDone = true;
    }

    [Command]
    void Cmd_MoveRight()
    {
        Vector3 speed = new Vector3(addVelocity, characterRigidbody.velocity.y, 0);
        Mover(speed);
        if (!jumps && runs)
            animator.SetBool("Run", true);
    }


    [Command]
    void Cmd_MoveLeft()
    {
        Vector3 speed = new Vector3(-addVelocity, characterRigidbody.velocity.y, 0);
        Mover(speed);
        if (!jumps && runs)
            animator.SetBool("Run", true);
    }

    [Command]
    void Cmd_MoveJump()
    {
        if (!jumps)
        {
            animator.SetTrigger("Jump");
            jumps = true;
            Vector3 speed = new Vector3(0, jumpHight, 0);
            Mover(speed);
        }
    }

    void MoveStop()
    {
        if (!jumps && !runs)
        {
            Vector3 speed = new Vector3(0, characterRigidbody.velocity.y, 0);
            characterRigidbody.velocity = speed;
        }
        if (!runs)
        {
            animator.SetBool("Run", false);
        }
    }

    void Mover(Vector3 speed)
    {
        characterRigidbody.velocity = speed;
    }

    void Rotator()
    {
        Quaternion rotation;
        if (walksLeft && !walksRight)
        {
            rotation = new Quaternion(0, -1f, 0, 1);
        }
        else if (walksRight && !walksLeft)
        {
            rotation = new Quaternion(0, 1f, 0, 1);
        }
        else
        {
            rotation = new Quaternion(characterRigidbody.rotation.x, characterRigidbody.rotation.y, characterRigidbody.rotation.z, characterRigidbody.rotation.w);
        }
        characterRigidbody.rotation = Quaternion.Lerp(characterRigidbody.rotation, rotation, 0.1f);
    }

    public void ProxyCommandDie()
    {
        Cmd_Die();
    }

    [Command]
    public void Cmd_Die()
    {
        Rpc_Die();
    }

    [ClientRpc]
    public void Rpc_Die()
    {
        animator.SetTrigger("Die");
        isDead = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            if (jumps && runs)
            {
                animator.SetBool("Run", true);
            }
            jumps = false;
            startJump = false;
        }
    }

    enum Action { Run, Jump, Fall, Idle, Attack, TakeDamage};
    Action actions; 

    public void UpdateVelocityState()
    {
        if (characterRigidbody.velocity.x != 0)
        {
            actions = Action.Run;
        }
        if (characterRigidbody.velocity.y > 0)
        {
            actions = Action.Jump;
        }
        if (characterRigidbody.velocity.y < 0)
        {
            actions = Action.Fall;
        }
        if (attacks)
        {
            actions = Action.Attack;
        }
        if (takeDamage)
        {
            actions = Action.TakeDamage;
        }
    }

    private void DoAnimations()
    {
        switch (actions)
        {
            case Action.Run:
                break;
            case Action.Jump:
                break;
            case Action.Fall:
                break;
            case Action.Idle:
                break;
            case Action.Attack:
                break;
            case Action.TakeDamage:
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        EventManager.movementMethods -= Cmd_MoveRight;
        EventManager.movementMethods -= Cmd_MoveLeft;
        EventManager.movementMethods -= Cmd_MoveJump;
        EventManager.movementMethods -= CameraMovement;
        EventManager.movementMethods -= Rotator;
        EventManager.movementMethods -= ShotPointer;
    }

    private void OnDestroy()
    {
        EventManager.movementMethods -= Cmd_MoveRight;
        EventManager.movementMethods -= Cmd_MoveLeft;
        EventManager.movementMethods -= Cmd_MoveJump;
        EventManager.movementMethods -= CameraMovement;
        EventManager.movementMethods -= Rotator;
        EventManager.movementMethods -= ShotPointer;
    }

}
