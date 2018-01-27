using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    [Header("Configuration")]
    public float AtkTimer = 0.6f;

    [Header("Components")]
    [SerializeField]
    private BoxCollider SwordCollider;

    private int Damage = 3;
    public void SetDamage(int val) { Damage = val; }

    public void Attack()
    {
        SwordCollider.enabled = true;
        Invoke("RemoveCollider", AtkTimer);
    }

    public void RemoveCollider()
    {
        SwordCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyStats>().GetHit(Damage);
        }
    }
}
