using System;
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
    public SelectionInterface SelectionInterface;
    
    [Header("Components")]
    public Camera FollowingCamera;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private SwordController Sword;

    [Header("RaycastCollisionDetection")]
    public RaycastDirection[] Directions;

    //[HideInInspector]
    public bool MayMove = true;

    private Rigidbody Rigidbody;
    private BoxCollider Collider;
    private bool MayJump;
    private float IdleTimer = 0;
    private int RoomLayer;
    private Vector3 SpawnPosition;

    private bool BlockSuicide;

    public void Awake()
    {
        //MayMove = false;
        SpawnPosition = transform.position;
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        RoomLayer = LayerMask.NameToLayer("Room");
    }

    public void Spawn()
    {
        MayMove = true;
        transform.position = SpawnPosition;
    }

    public void Update()
    {
        if (!BlockSuicide && Input.GetButton("Suicide1") && Input.GetButton("Suicide2"))
        {
            Die();
            Debug.Log("BOUM");
        }
        else
            BlockSuicide = false;
    }

    public void Die()
    {
        BlockSuicide = true;
        MayMove = false;
        SelectionInterface.Init(true);
        StartCoroutine(ResMob());
    }

    IEnumerator ResMob()
    {
        EnemyRespawn[] enemys = FindObjectsOfType<EnemyRespawn>();

        foreach(EnemyRespawn elem in enemys)
        {
            elem.Reload();
            yield return new WaitForFixedUpdate();
        }

        FindObjectOfType<BossAI>().Reset();
    }

    public void FixedUpdate()
    {
        float direction = (MayMove ? Input.GetAxis("Horizontal") : 0);

        direction = CheckCollision(direction);
        ComputeDirection(direction);
        
        transform.position += Vector3.right * direction * Speed;

        foreach (RaycastDirection d in Directions)
        {
            Vector3 point = Raycaster(d);

            if (point != Vector3.zero)
            {
                if (d.Freeze)
                {
                    Rigidbody.useGravity = !true;
                    MayJump = true;
                    _animator.SetBool("InTheAir", false);
                }
                else
                    Rigidbody.useGravity = !false;
            }
            else
                Rigidbody.useGravity = true;
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
            Rigidbody.useGravity = !false;
            //_animator.SetBool("InTheAir", true);
            Rigidbody.AddForce(new Vector3(0, JumpStrength, 0));
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

    public float CheckCollision(float direction)
    {
        float yMin = transform.position.y - Collider.bounds.min.y;
        float yMax = transform.position.y - Collider.bounds.max.y;
        float test = -yMax;
        float distance = 0.6f;

        while (test > -yMin)
        {
            Ray[] r = new[]
            {
                new Ray(transform.position + new Vector3(0, test, 0), Vector3.right),
                new Ray(transform.position + new Vector3(0, test, 0), Vector3.left)
            };
            
            foreach (RaycastHit hit in Physics.RaycastAll(r[0], distance))
            {
                if (hit.collider.gameObject.layer == RoomLayer && direction > 0)
                    return (0);
            }

            foreach (RaycastHit hit in Physics.RaycastAll(r[1], distance))
            {
                if (hit.collider.gameObject.layer == RoomLayer && direction < 0)
                    return (0);
            }

            test -= 0.1f;
        }

        return (direction);
    }

    public Vector3 Raycaster(RaycastDirection direction)
    {
        float xMin = transform.position.x - Collider.bounds.min.x;
        float xMax = transform.position.x - Collider.bounds.max.x;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(xMin, 0, 0), direction.Direction, out hit, direction.Length) ||
            Physics.Raycast(transform.position + new Vector3(xMax, 0, 0), direction.Direction, out hit, direction.Length) ||
            Physics.Raycast(transform.position, direction.Direction, out hit, direction.Length))
            return (hit.point - (direction.Direction * direction.Length));
        
        return (Vector3.zero);
    }
}