using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class RaycastDirection
{
    public float Length;
    public Vector3 Direction;
    public Vector3 PositionModifier;
    public bool Freeze;
}

public class CameraCharacterController : MonoBehaviour
{
    [Header("Configuration")]
    public float Speed = 1f;
    public float LerpStrength = 1f;
    public float JumpStrength = 100f;
    public Vector3 PositionModifier = new Vector3(0, 0, -10);

    [Header("Components")]
    public Camera FollowingCamera;

    [Header("RaycastCollisionDetection")]
    public RaycastDirection[] Directions;

    private Rigidbody Rigidbody;

    public void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        transform.position += Vector3.right * Input.GetAxis("Horizontal") * Speed;

        ComputeDirection(Input.GetAxis("Horizontal"));

        foreach (RaycastDirection direction in Directions)
        {
            Vector3 point = Raycaster(direction);

            if (point != Vector3.zero)
            {
                transform.position = new Vector3(point.x * direction.PositionModifier.x + transform.position.x * (1 - direction.PositionModifier.x),
                                                 point.y * direction.PositionModifier.y + transform.position.y * (1 - direction.PositionModifier.y),
                                                 point.z * direction.PositionModifier.z + transform.position.z * (1 - direction.PositionModifier.z));

                if (direction.Freeze)
                    Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                else
                    Rigidbody.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
            }
        }

        TryJump();

        Vector3 positionToGo = new Vector3(transform.position.x, transform.position.y, 0) + PositionModifier;

        FollowingCamera.transform.position = Vector3.Lerp(FollowingCamera.transform.position, positionToGo, 0.2f);
    }

    public void ComputeDirection(float direction)
    {
        if (direction > 0)
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        else if (direction < 0)
            transform.localRotation = Quaternion.Euler(0, -90, 0);
    }

    public void TryJump()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody.AddForce(new Vector3(0, JumpStrength, 0));
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
        }
    }

    public Vector3 Raycaster(RaycastDirection direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction.Direction, out hit, direction.Length))
        {
            Debug.Log(hit.point);
            Debug.Log("Hit");
            return (hit.point - (direction.Direction * direction.Length));
        }

        return (Vector3.zero);
    }
}