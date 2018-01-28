using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : AStats
{
    public Slider LifeBar;

    public List<TraitScriptableObject> UnlockedTraits;
    public List<TraitScriptableObject> Traits;

    public SwordController Sword;

    public Image[] TraitGUI;

    public void Awake()
    {
        if (UnlockedTraits == null)
            UnlockedTraits = new List<TraitScriptableObject>();
        Traits = new List<TraitScriptableObject>();

        foreach (Image image in TraitGUI)
        {
            image.gameObject.SetActive(false);
        }
        base.Awake();
        Sword.SetDamage(Damage);
    }

    public void WipeTraits()
    {
        Traits.Clear();
        foreach (Image image in TraitGUI)
        {
            image.gameObject.SetActive(false);
        }
        RestoreStats();
    }

    public void WinATrait(TraitScriptableObject trait)
    {
        if (!UnlockedTraits.Contains(trait))
        {
            UnlockedTraits.Add(trait);
            AddTrait(trait);
        }
    }

    public void AddTrait(TraitScriptableObject trait)
    {
        Traits.Add(trait);
        Life += trait.Life;
        Damage += trait.Damage;
        Speed += trait.Speed;
        AtkSpeed += trait.AtkSpeed;

        TraitGUI[Traits.Count - 1].gameObject.SetActive(true);
        TraitGUI[Traits.Count - 1].sprite = trait.Sprite;
        Sword.SetDamage(Damage);
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
