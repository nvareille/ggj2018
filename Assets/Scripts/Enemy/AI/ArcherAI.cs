using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : AI {

    [SerializeField]
    private Transform Player;
    [SerializeField]
    private RangeWeaponController Weapon;

    protected override void Idle()
    {
        //Debug.Log("Idle");
    }

    protected override void Move()
    {
        Debug.Log("Walk");
        transform.position = Vector3.MoveTowards(transform.position, Player.position, Stat.Speed * Time.deltaTime);
        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        if (Vector3.Distance(transform.position, Player.position) < Stat.PlayerDist)
        {
            UpdateIndex = (int)EAIState.ATK;
            Anim.SetBool("Walk", false);
        }
    }

    protected override void Attack()
    {
        //si il est pu devant faut le pourchasser
        Debug.Log("Atk");

        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        if (!IsAttacking && Vector3.Distance(transform.position, Player.position) > Stat.PlayerDist + 0.2f)
        {
            UpdateIndex = (int)EAIState.MOVE;
            Anim.SetBool("Walk", true);
            return;
        }
        AtkTimer += Time.deltaTime;
        if (AtkTimer >= Stat.AtkSpeed)
        {
            Weapon.Attack();
            Anim.SetTrigger("Attack");
            IsAttacking = true;
            AtkTimer = 0;
        }
    }
}
