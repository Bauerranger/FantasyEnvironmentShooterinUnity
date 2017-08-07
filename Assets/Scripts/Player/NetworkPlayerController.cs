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
    private float jumpHight = 100;
    private float smoothTime = 0.2f;
    private float cameraDistance = -15f;
    private float cameraOffsetX = 5.5f;
    private float cameraOffsetY = 5f;
    private float runSpeed = 6f;
    public float addForce = 50f;
    private bool menuOpen = false;
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
        EventManager.UpdateMethods += FetchInput;
        EventManager.UpdateMethods += Move;
        EventManager.UpdateMethods += CameraMovement;
        EventManager.UpdateMethods += Rotator;
        EventManager.UpdateMethods += ShotPointer;
        EventManager.UpdateMethods += UpdateAnimationState;
        EventManager.attackMethods += Cmd_Attack;
        EventManager.buttonLeftMethods += Cmd_MoveRight;
        EventManager.buttonRightMethods += Cmd_MoveLeft;
        EventManager.stopMethods += MoveStop;
        EventManager.buttonSpaceMethods += Cmd_MoveJump;
        ShotTargetPoint = GameObject.FindGameObjectWithTag("ShotTargetPoint").transform;
        menuCanvas = GameObject.FindGameObjectWithTag("main_Menu");
        inputIsActive = true;
        EventManager.dieMethods += ProxyCommandDie;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        EventManager.ObservePlayerStatus();
        if (dies && !isDead)
            EventManager.Die();
        EventManager.Menu();
    }

    void FetchInput()
    {
        if (isDead)
        {
            EventManager.UpdateMethods -= FetchInput;
        }
        if (inputIsActive && !isDead)
        {
            if (Input.GetKey(moveRightKey))
            {
                runs = true;
                EventManager.buttonLeftIsPressed();
            }

            if (Input.GetKey(moveLeftKey))
            {
                runs = true;
                EventManager.buttonRightIsPressed();
            }

            if (Input.GetKeyDown(moveJumpKey) && !jumps)
            {
                startJump = true;
                EventManager.buttonSpaceIsPressed();
            }

            if (!Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
            {
                EventManager.notLefnorRightIsPressed();
            }

            if (Input.GetMouseButton(0) && attackAnimationIsDone)
            {
                EventManager.Attack();
            }

            if (Input.GetMouseButtonUp(0))
            {
                attacks = false;
            }
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
    }

    private void Move()
    {
        Vector3 speed = new Vector3(addVelocity, characterRigidbody.velocity.y, 0);
        characterRigidbody.velocity = speed;
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
                ShotTargetPoint.position = hit.transform.position + new Vector3(0, 1.8f, 0);
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
        addVelocity = runSpeed;
    }


    [Command]
    void Cmd_MoveLeft()
    {
        addVelocity = -runSpeed;
    }

    [Command]
    void Cmd_MoveJump()
    {
        if (!jumps)
        {
            jumps = true;
            if (characterRigidbody != null)
                characterRigidbody.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }
    }

    void MoveStop()
    {
        addVelocity = 0;
    }

    void Rotator()
    {
        Vector2 playerScreenPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePositionOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(playerScreenPosition.y - mousePositionOnScreen.y, playerScreenPosition.x - mousePositionOnScreen.x) * Mathf.Rad2Deg;
        if (angle <= -90 || angle > 90)
            characterRigidbody.rotation = Quaternion.Lerp(characterRigidbody.rotation, Quaternion.AngleAxis(90, Vector3.up), 0.5f);
        if (angle >= -90 && angle < 90)
            characterRigidbody.rotation = Quaternion.Lerp(characterRigidbody.rotation, Quaternion.AngleAxis(-90, Vector3.up), 0.5f);
    }

    public void ProxyCommandDie()
    {

        inputIsActive = false;
        EventManager.UpdateMethods -= FetchInput;
        EventManager.UpdateMethods -= Move;
        EventManager.UpdateMethods -= Rotator;
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
        characterRigidbody.velocity = Vector3.zero;
        animator.SetTrigger("Die");
        isDead = true;
    }

    public void UpdateAnimationState()
    {
        animator.SetFloat("VelocityX", characterRigidbody.velocity.x);
        animator.SetFloat("VelocityY", characterRigidbody.velocity.y);
        if (characterRigidbody.velocity == Vector3.zero)
        {
            animator.SetBool("VelocityIsZero", true);
        }
        else
        {
            animator.SetBool("VelocityIsZero", false);
        }
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, down, out hit))
        {
            animator.SetFloat("DistanceToGround", hit.distance);
            if (hit.distance < 0.01f)
            {
                jumps = false;
            }

        }
    }

    private void OnDisable()
    {
        EventManager.UpdateMethods -= Cmd_MoveRight;
        EventManager.UpdateMethods -= Cmd_MoveLeft;
        EventManager.UpdateMethods -= Cmd_MoveJump;
        EventManager.UpdateMethods -= CameraMovement;
        EventManager.UpdateMethods -= Rotator;
        EventManager.UpdateMethods -= ShotPointer;
    }

    private void OnDestroy()
    {
        EventManager.UpdateMethods -= Cmd_MoveRight;
        EventManager.UpdateMethods -= Cmd_MoveLeft;
        EventManager.UpdateMethods -= Cmd_MoveJump;
        EventManager.UpdateMethods -= CameraMovement;
        EventManager.UpdateMethods -= Rotator;
        EventManager.UpdateMethods -= ShotPointer;
    }

}
