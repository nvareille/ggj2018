using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStats : MonoBehaviour {

    [Header("Stats")]
    public int Life = 1;
    public int Damage = 1;
    public float Speed = 1;

    protected int CurrentLife;
    protected bool IsDead;

    public void Start()
    {
        CurrentLife = Life;
    }

    public void GetHit(int dmg)
    {
        if (IsDead)
            return;
        CurrentLife -= dmg;
        if (CurrentLife < 0)
        {
            IsDead = true;
            CurrentLife = 0;
            //play anim de mort
        }
        Debug.Log(this.name + " take " + dmg + ", only " + CurrentLife + " left !");
    }
}
