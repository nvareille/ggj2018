using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Configuration")]
    public DoorController TeleportTo;
    public Vector3 Spawn;

    private int CharacterLayer;
    private Camera Camera;

    public void Awake()
    {
        CharacterLayer = LayerMask.NameToLayer("Character");
        Camera = FindObjectOfType<Camera>();
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == CharacterLayer && Input.GetButtonDown("Interact"))
        {
            CameraCharacterController chara = collider.gameObject.GetComponent<CameraCharacterController>();

            /*collider.gameObject.transform.position = new Vector3(TeleportTo.transform.position.x + TeleportTo.Spawn.x, 
                                                                 TeleportTo.transform.position.y + TeleportTo.Spawn.y, 
                                                                 collider.gameObject.transform.position.z);*/


            chara.MayMove = false;
            chara.transform.rotation = Quaternion.Euler(0, 0, 0);

            Camera.DOFieldOfView(1, 1).OnComplete(() =>
            {
                chara.transform.position = TeleportTo.GetSpawn(chara);
                Camera.DOFieldOfView(60, 1).OnComplete(() =>
                {
                    chara.MayMove = true;
                    chara.transform.rotation = Quaternion.Euler(0, 180, 0);
                });
            });

        }
    }

    public Vector3 GetSpawn(CameraCharacterController chara)
    {
        return (new Vector3(transform.position.x + Spawn.x, transform.position.y + Spawn.y, chara.transform.position.z));
    }
}