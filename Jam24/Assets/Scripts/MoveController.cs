using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class MoveController : MonoBehaviourPunCallbacks
{
  public Animator animator;

  public PhotonView photonView;


  public float moveSpeed = 5f;
  private Rigidbody rb;
  private Vector3 inputVector;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!photonView.IsMine)
      return;

    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
    {
      animator.SetTrigger("Move");
    }

    if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
    {
      animator.SetTrigger("Idle");
    }

    {
      inputVector.x = Input.GetAxis("Horizontal");
      inputVector.z = Input.GetAxis("Vertical");

      if (inputVector != Vector3.zero)
      {
        transform.rotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.z));
      }
    }

    void FixedUpdate()
    {
      if (!photonView.IsMine)
        return;

      rb.velocity = new Vector3(inputVector.x * moveSpeed, rb.velocity.y, inputVector.z * moveSpeed);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
  }
}