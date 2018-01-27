﻿using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SwordController Sword;

    [Header("RaycastCollisionDetection")]
    public RaycastDirection[] Directions;

    private Rigidbody Rigidbody;
    private bool MayJump;
    private float IdleTimer = 0;

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
                {
                    Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    MayJump = true;
                    _animator.SetBool("InTheAir", false);
                }
                else
                    Rigidbody.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
            }
        }

        TryJump();

        TryAtk();

        Vector3 positionToGo = new Vector3(transform.position.x, transform.position.y, 0) + PositionModifier;

        FollowingCamera.transform.position = Vector3.Lerp(FollowingCamera.transform.position, positionToGo, 0.2f);
    }

    public void ComputeDirection(float direction)
    {
        if (direction > 0)
        {
            IdleTimer = 0;
            _animator.SetBool("Walk", true);
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (direction < 0)
        {
            IdleTimer = 0;
            _animator.SetBool("Walk", true);
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (direction == 0)
        {//si il n'a pas touché au joystick depuis 0.05sec, on le considère en idle -> car quand on change de direction il repasse par le Zero/idle et c'est relou
            IdleTimer += Time.deltaTime;
            if (IdleTimer >= 0.05f)
                _animator.SetBool("Walk", false);
        }
    }

    public void TryJump()
    {
        if (Input.GetButtonDown("Fire1") && MayJump)
        {
            MayJump = false;
            _animator.SetBool("InTheAir", true);
            Rigidbody.AddForce(new Vector3(0, JumpStrength, 0));
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
        }
    }

    public void TryAtk()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            _animator.SetTrigger("Attack");
            Sword.Attack();
        }
    }

    public void RemoveCollider()
    {

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