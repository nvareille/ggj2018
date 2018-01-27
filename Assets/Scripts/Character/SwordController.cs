using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    [Header("Configuration")]
    public float Delay = 0.6f;
    public float AtkDuration = 1f;
    public string Target = "Player";

    [Header("Components")]
    [SerializeField]
    private BoxCollider SwordCollider;
    [SerializeField]
    private MeshRenderer Rend;

    private int Damage = 3;
    public void SetDamage(int val) { Damage = val; }

    public void Attack()
    {
        Invoke("LateAttack", Delay);
        Invoke("RemoveCollider", AtkDuration + Delay);
    }

    private void LateAttack()
    {
        SwordCollider.enabled = true;
        if (Rend != null)
            Rend.enabled = true;
    }

    private void RemoveCollider()
    {
        SwordCollider.enabled = false;
        if (Rend != null)
            Rend.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Target)
        {
            other.GetComponent<AStats>().GetHit(Damage);
        }
    }
}
