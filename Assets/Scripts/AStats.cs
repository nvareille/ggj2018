using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStats : MonoBehaviour
{

    [Header("Stats")]
    public int Life = 1;
    public int Damage = 1;
    public float Speed = 1;
    public float AtkSpeed = 1;

    [Header("Config")]
    public bool IsImportant;

    private int LifeSave;
    private int CurrentLifeSave;
    private int DamageSave;
    private float SpeedSave;
    private float AtkSpeedSave;
    
    protected int CurrentLife;
    protected bool IsDead;

    public int GetLife() { return CurrentLife; }
    public void ResetLife() { CurrentLife = Life; }

    public void Awake()
    {
        CurrentLife = Life;
        BackupStats();
    }
    
    public void BackupStats()
    {
        Debug.Log("BACK");
        LifeSave = Life;
        CurrentLifeSave = CurrentLife;
        DamageSave = Damage;
        SpeedSave = Speed;
        AtkSpeedSave = AtkSpeed;
    }

    public void RestoreStats()
    {
        Life = LifeSave;
        CurrentLife = CurrentLifeSave;
        Damage = DamageSave;
        Speed = SpeedSave;
        AtkSpeed = AtkSpeedSave;
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
            if (!IsImportant)
            {
                GameObject.Destroy(this.gameObject, 0.6f);
            }
            else
            {
                if (this.GetComponent<CameraCharacterController>() != null)
                {
                    GetComponent<CameraCharacterController>().Die();
                }
                else
                {
                    //Winnnn
                }
            }
        }
        Debug.Log(this.name + " take " + dmg + ", only " + CurrentLife + " left !");
    }
}
