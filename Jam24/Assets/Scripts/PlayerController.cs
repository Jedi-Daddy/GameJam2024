using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
  public Animator anim;

  [Header("Movement")]
  public float moveSpeed;
  private Vector2 curMovementInput;
  public float jumpForce;
  public LayerMask groundLayerMask;

  [Header("Look")]
  public Transform cameraContainer;
  public float minXLook;
  public float maxXLook;
  private float camCurXRot;
  public float lookSensitivity;


  [Header("Components")]
  public Rigidbody rig;
  public MeshRenderer meshRenderer;
  public PhotonView photonView;
  public Photon.Realtime.Player photonPlayer;

  private Vector2 mouseDelta;

  [HideInInspector]
  public bool canLook = true;

  public int id;

  [PunRPC]
  public void Initialize(Photon.Realtime.Player player)
  {
    photonPlayer = player;
    id = player.ActorNumber;

    GameManager.instance.players[id - 1] = this;

    var inter = GetComponent<InteractionManager>();
    if (!player.IsLocal)
    {
      inter.InventoryHud.SetActive(false);
      inter.MainHUD.SetActive(false);
    }
    else
    {
      inter.InventoryHud.SetActive(true);
      inter.MainHUD.SetActive(true);
    }

    // if this isn't our local player, disable physics as that's
    // controlled by the user and synced to all other clients
    if (!photonView.IsMine)
      rig.isKinematic = true;
  }

  void Awake()
  {
    // get our components
    rig = GetComponent<Rigidbody>();
    photonView = GetComponent<PhotonView>();
  }

  void Update()
  {
    // only the client of the local player can control it
    if (photonView.IsMine)
      Move();
  }

  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
  { }


  void LateUpdate()
  {
    if (!photonView.IsMine)
      return;

    if (canLook == true)
      CameraLook();
  }

  private Vector3 inputVector;

  void Move()
  {
    if (!photonView.IsMine)
      return;

    /*
    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
    {
      anim.SetTrigger("Move");
    }

    if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
    {
      anim.SetTrigger("Idle");
    }
    */

    inputVector.x = Input.GetAxis("Horizontal");
    inputVector.z = Input.GetAxis("Vertical");

    if (inputVector != Vector3.zero)
      transform.rotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.z));
  }


  void FixedUpdate()
  {
    if (!photonView.IsMine)
      return;

    rig.velocity = new Vector3(inputVector.x * moveSpeed, rig.velocity.y, inputVector.z * moveSpeed);

    if (rig.velocity.magnitude > 0.2)
      anim.SetTrigger("Move");
    else
      anim.SetTrigger("Idle");
  }

  void CameraLook()
  {
    // rotate the camera container up and down
    camCurXRot += mouseDelta.y * lookSensitivity;
    camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

    // rotate the player left and right
    transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
  }

  // called when we move our mouse - managed by the Input System
  public void OnLookInput(InputAction.CallbackContext context)
  {
    mouseDelta = context.ReadValue<Vector2>();
  }

  // called when we press WASD - managed by the Input System
  public void OnMoveInput(InputAction.CallbackContext context)
  {
    // are we holding down a movement button?
    if (context.phase == InputActionPhase.Performed)
    {
      curMovementInput = context.ReadValue<Vector2>();
    }
    // have we let go of a movement button?
    else if (context.phase == InputActionPhase.Canceled)
    {
      curMovementInput = Vector2.zero;
    }
  }

  // called when we press down on the spacebar - managed by the Input System
  public void OnJumpInput(InputAction.CallbackContext context)
  {
    // is this the first frame we're pressing the button?
    if (context.phase == InputActionPhase.Started)
    {
      // are we standing on the ground?
      if (IsGrounded())
      {
        // add force updwards
        rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      }
    }
  }

  bool IsGrounded()
  {
    Ray[] rays = new Ray[4]
    {
            new Ray(transform.position + (transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (Vector3.up * 0.01f), Vector3.down)
    };

    for (int i = 0; i < rays.Length; i++)
    {
      if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
      {
        return true;
      }
    }

    return false;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;

    Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
    Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
    Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
    Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
  }
}