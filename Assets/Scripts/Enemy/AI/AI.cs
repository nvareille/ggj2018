using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAIState
{
    IDLE = 0,
    MOVE,
    ATK,
}

public abstract class AI : MonoBehaviour {

    protected EnemyStats Stat;
    protected Animator Anim;
    protected delegate void UpdateState();
    protected UpdateState[] UpdateSM;
    protected int UpdateIndex;

    protected float AtkTimer;

    protected bool IsAttacking = true;
    public void SetAttack(bool val) { IsAttacking = val; }

    // Use this for initialization
    protected virtual void Start()
    {
        Stat = GetComponent<EnemyStats>();
        Anim = GetComponent<Animator>();
        UpdateSM = new UpdateState[]{
            Idle,
            Move,
            Attack
        };
        UpdateIndex = 0;
        AtkTimer = Stat.AtkSpeed;
        //pour le golem, l'archer
        Anim.GetBehaviour<RandomAtkAnimator>().unit = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSM[UpdateIndex]();
    }

    protected abstract void Idle();
    protected abstract void Move();
    protected abstract void Attack();

    public virtual void KillHim()
    {
        UpdateIndex = (int)EAIState.MOVE;
        Anim.SetBool("Walk", true);
    }

    public virtual void LostHim()
    {
        UpdateIndex = (int)EAIState.IDLE;
        Anim.SetBool("Walk", false);
    }


}
