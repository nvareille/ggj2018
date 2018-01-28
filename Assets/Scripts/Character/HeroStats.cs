using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ETrait
{
    NONE = 0,
    COSTAUD,
    DEMON,
    VAMPIRE,
    VENTRIPOTENT,
}

public class HeroStats : AStats
{
    public Slider LifeBar;

    public Image[] BonusGUI;
    public Image[] MalusGUI;

    public TraitScriptableObject[] Bonus;
    public TraitScriptableObject[] Malus;

    private int BonusCount;
    private int MaluscCount;

    private List<ETrait> Traits;
    public bool IsHavingATrait(ETrait trait)
    {
        foreach (ETrait elem in Traits)
        {
            if (elem == trait)
                return true;
        }
        return false;
    }

    public void Awake()
    {
        Traits = new List<ETrait>();
        /*
        BonusCount = 0;
        foreach (TraitScriptableObject o in Bonus)
        {
            BonusGUI[BonusCount].gameObject.SetActive(true);
            BonusGUI[BonusCount].sprite = o.Sprite;
            Life += o.Life;
            Damage += o.Damage;
            Speed += o.Speed;
            AtkSpeed += o.AtkSpeed;
            ++BonusCount;
        }

        MaluscCount = 0;
        foreach (TraitScriptableObject o in Malus)
        {
            MalusGUI[MaluscCount].gameObject.SetActive(true);
            MalusGUI[MaluscCount].sprite = o.Sprite;
            Life += o.Life;
            Damage += o.Damage;
            Speed += o.Speed;
            AtkSpeed += o.AtkSpeed;
            ++MaluscCount;
        }*/
    }

    public void WinATrait(bool IsBenefic)
    {
        TraitScriptableObject trait;

        if (IsBenefic)
        {
            trait = Bonus[Random.Range(0, Bonus.Length)];
            BonusGUI[BonusCount].gameObject.SetActive(true);
            BonusGUI[BonusCount].sprite = trait.Sprite;
            ++BonusCount;
        }
        else
        {
            trait = Malus[Random.Range(0, Malus.Length)];
            MalusGUI[MaluscCount].gameObject.SetActive(true);
            MalusGUI[MaluscCount].sprite = trait.Sprite;
            ++MaluscCount;
        }
        Traits.Add(trait.TraitName);
        Life += trait.Life;
        Damage += trait.Damage;
        Speed += trait.Speed;
        AtkSpeed += trait.AtkSpeed;
    }

    public void Heal(int val)
    {
        CurrentLife += val;
    }

    public void Update()
    {
        LifeBar.value = ((float)CurrentLife / (float)Life);
    }
}
