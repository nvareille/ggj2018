using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : AI {

    [Header("Configuration")]
    public float Delay = 0.6f;
    public float AtkDuration = 1f;
    public Transform Target;
    public Transform SpawnSphere;
    public Transform Floor;
    public GameObject LavaPref;
    public GameObject SpherePref;

    private bool IsPhase2 = false;
    private bool IsMoving = false;

    protected override void Idle()
    {
        //Debug.Log("Idle");
    }

    protected override void Move()
    {
        //Debug.Log("Walk");
        UpdateIndex = (int)EAIState.ATK;
    }

    protected override void Attack()
    {
        //si il est pu devant faut le pourchasser
        if (IsMoving)
            return;
        AtkTimer += Time.deltaTime;
        if (AtkTimer >= Stat.AtkSpeed)
        {
            if (Random.Range(0,10) < 5)
            {
                Invoke("Sphere", Delay);
                Invoke("Sphere", Delay + 2);
                Anim.SetTrigger("Attack1");
            }
            else
            {
                Invoke("Lava", Delay);
                Anim.SetTrigger("Attack2");
            }
            
            AtkTimer = 0;
        }
        if (!IsPhase2 && Stat.GetLife() < (Stat.Life / 2))
        {
            StartCoroutine(GoingUp());
        }
    }

    private void Lava()
    {
        GameObject obj = Instantiate(LavaPref, new Vector3(Player.position.x, Floor.position.y, Player.position.z), Quaternion.Euler(-90, 0, 0));
        obj.GetComponent<SwordController>().SetDamage(Stat.Damage);
        obj.GetComponent<SwordController>().Attack();
    }

    private void Sphere()
    {
        Instantiate(SpherePref, SpawnSphere.position, Quaternion.identity);
    }

    IEnumerator GoingUp()
    {
        IsMoving = true;

        while (Vector3.Distance(transform.position, Target.position) > 0.2f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * Stat.Speed);
            yield return null;
        }

        IsMoving = false;
    }

    public override void KillHim()
    {
        UpdateIndex = (int)EAIState.ATK;
    }

    public void Reset()
    {
        UpdateIndex = (int)EAIState.IDLE;
        Stat.ResetLife();
    }

}
