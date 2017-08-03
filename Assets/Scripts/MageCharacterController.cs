using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MageCharacterController : NetworkBehaviour
{
    [SerializeField]
    private Transform ShotStartingPoint;
    [SerializeField]
    private Transform ShortTargetPoint;
    [SerializeField]
    private float cursorDistance = 3.00f;

    public GameObject cameraSpawn;
    private string moveRightKey = "d";
    private string moveLeftKey = "a";
    private string moveJumpKey = "space";
    private float addVelocity = 6;
    private float jumpHight = 8;
    private float smoothTime = 0.2f;
    private float cameraDistance = -17f;
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

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            Instantiate(cameraSpawn, new Vector3(0f, 0f, 0f), Quaternion.identity);
            cameraMover = GameObject.FindGameObjectWithTag("CameraMover");
            cameraMover.transform.position = new Vector3(characterRigidbody.transform.position.x + cameraOffsetX, characterRigidbody.transform.position.y + cameraOffsetY, cameraDistance);
            cameraMover.transform.rotation = Quaternion.Euler(cameraAngle);
            Cursor.visible = false;
            EventManager.movementMethods += cameraMovement;
            EventManager.movementMethods += rotator;
            EventManager.movementMethods += shotPointer;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            fetchInput();
            updateMethods();
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            EventManager.Movement();
            EventManager.Attack();
            EventManager.Menu();
            moveStop();
        }
    }

    void updateMethods()
    {
        if (walksLeft && !walksRight)
        {
            EventManager.movementMethods += moveLeft;
        }

        if (walksRight && !walksLeft)
        {
            EventManager.movementMethods += moveRight;
        }

        if (startJump && !jumps)
        {
            EventManager.movementMethods += moveJump;
        }

        if (attacks && attackAnimationIsDone)
        {
            EventManager.attackMethods += attack;
        }
        if (!attacks)
        {
            EventManager.attackMethods -= attack;
        }
        if (!walksRight)
        {
            EventManager.movementMethods -= moveRight;
        }
        if (!walksLeft)
        {
            EventManager.movementMethods -= moveLeft;
        }

        if (jumps)
        {
            EventManager.movementMethods -= moveJump;
        }

        if (menuOpen)
        {
            EventManager.menuMethods += openMenu;
            EventManager.menuMethods -= closeMenu;
        }

        if (!menuOpen)
        {
            EventManager.menuMethods += closeMenu;
            EventManager.menuMethods -= openMenu;
        }
    }
    
    void fetchInput()
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
        if (Input.GetKeyDown("escape"))
        {
            if (menuOpen)
                menuOpen = false;
            if (!menuOpen)
                menuOpen = true;
        }
    }

    void cameraMovement()
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

    void attack()
    {
        if (attackAnimationIsDone)
        {
            attackAnimationIsDone = false;
            animator.SetTrigger("Melee Attack");
        }
    }

    void shotPointer()
    {
        Plane playerPlane = new Plane(transform.right, ShotStartingPoint.position);
        float outEnterPoint = 0.0f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        playerPlane.Raycast(ray, out outEnterPoint);
        Vector3 mouseWorldPosition = ray.GetPoint(outEnterPoint);

        Vector3 towardsMouse = (mouseWorldPosition - ShotStartingPoint.position).normalized;
        float angle = Mathf.Acos(Vector3.Dot(towardsMouse, transform.forward)) * Mathf.Rad2Deg;

        if (angle >= 90)
        {
            Vector3 cachedForward = transform.forward;
            Vector3.OrthoNormalize(ref cachedForward, ref towardsMouse);
        }

        ShortTargetPoint.position = ShotStartingPoint.position + towardsMouse * cursorDistance;
    }

    public void spawnShot()
    {
        if (isLocalPlayer)
        EventManager.Shoot();
    }

    public void animationEnds()
    {
        attackAnimationIsDone = true;
    }

    void moveRight()
    {
        Vector3 speed = new Vector3(addVelocity, characterRigidbody.velocity.y, 0);
        mover(speed);
        if (!jumps && runs)
            animator.SetBool("Run", true);
    }

    void moveLeft()
    {
        Vector3 speed = new Vector3(-addVelocity, characterRigidbody.velocity.y, 0);
        mover(speed);
        if (!jumps && runs)
            animator.SetBool("Run", true);
    }

    void moveJump()
    {
        if (!jumps)
        {
            animator.SetTrigger("Jump");
            jumps = true;
            Vector3 speed = new Vector3(0, jumpHight, 0);
            mover(speed);
        }
    }

    void moveStop()
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

    void mover(Vector3 speed)
    {
        characterRigidbody.velocity = speed;
    }

    void rotator()
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

    void openMenu()
    {
        Cursor.visible = true;
    }

    void closeMenu()
    {
        Cursor.visible = false;
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

    private void OnDisable()
    {
        EventManager.movementMethods -= moveRight;
        EventManager.movementMethods -= moveLeft;
        EventManager.movementMethods -= moveJump;
        EventManager.movementMethods -= cameraMovement;
        EventManager.movementMethods -= rotator;
        EventManager.movementMethods -= shotPointer;
    }

    private void OnDestroy()
    {
        EventManager.movementMethods -= moveRight;
        EventManager.movementMethods -= moveLeft;
        EventManager.movementMethods -= moveJump;
        EventManager.movementMethods -= cameraMovement;
        EventManager.movementMethods -= rotator;
        EventManager.movementMethods -= shotPointer;
    }

}
