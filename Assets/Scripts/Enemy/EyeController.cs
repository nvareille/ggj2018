using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeController : MonoBehaviour {

    [SerializeField]
    private AI Head;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Head.KillHim();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Head.LostHim();            
        }
    }
}
