using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAI : AI {

    [SerializeField]
    private SwordController Hand;

    protected override void Start()
    {
        base.Start();
        Hand.SetDamage(Stat.Damage);
        //Debug.Log("Idle");
    }

    protected override void Idle()
    {
        //Debug.Log("Idle");
    }

    protected override void Move()
    {
        //Debug.Log("Walk");
        if (AtkTimer <= Stat.AtkSpeed)
            AtkTimer += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Player.position, Stat.Speed * Time.deltaTime);
        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-90, 180, 0);
        }
        Vector3 tmp = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (Vector3.Distance(tmp, Player.position) < Stat.PlayerDist)
        {
            UpdateIndex = (int)EAIState.ATK;
            Anim.SetBool("Walk", false);
        }
    }

    protected override void Attack()
    {
        //si il est pu devant faut le pourchasser
        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-90, 180, 0);
        }
        Vector3 tmp = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        if (!IsAttacking && Vector3.Distance(tmp, Player.position) > Stat.PlayerDist + 0.2f)
        {
            UpdateIndex = (int)EAIState.MOVE;
            Anim.SetBool("Walk", true);
            return;
        }
        AtkTimer += Time.deltaTime;
        if (AtkTimer >= Stat.AtkSpeed)
        {
            Hand.Attack();
            Anim.SetTrigger("Attack");
            IsAttacking = true;
            AtkTimer = 0;
        }
    }

}
