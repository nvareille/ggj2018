using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAI : AI {

    [SerializeField]
    private Transform Player;
    [SerializeField]
    private SwordController Hand;

    private bool IsAttacking = true;
    public void SetAttack(bool val) { IsAttacking = val; }

    protected override void Start()
    {
        base.Start();

        Anim.GetBehaviour<RandomAtkAnimator>().golem = this;
    }

    protected override void Idle()
    {
        //Debug.Log("Idle");
    }

    protected override void Move()
    {
        //Debug.Log("Walk");
        transform.position = Vector3.MoveTowards(transform.position, Player.position, Stat.Speed * Time.deltaTime);
        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-90, 180, 0);
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
        if (Player.position.x > this.transform.position.x)
        {
            transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-90, 180, 0);
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
            Hand.Attack();
            Anim.SetTrigger("Attack");
            IsAttacking = true;
            AtkTimer = 0;
        }
    }


    public override void KillHim()
    {
        UpdateIndex = (int)EAIState.MOVE;
        Anim.SetBool("Walk", true);
    }

    public override void LostHim()
    {
        UpdateIndex = (int)EAIState.IDLE;
        Anim.SetBool("Walk", false);
    }
}
