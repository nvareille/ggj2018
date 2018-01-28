using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : AStats
{
    public Slider LifeBar;

    public Image[] BonusGUI;
    public Image[] MalusGUI;

    public TraitScriptableObject[] Bonus;
    public TraitScriptableObject[] Malus;

    public void Awake()
    {
        int count;

        count = 0;
        foreach (TraitScriptableObject o in Bonus)
        {
            BonusGUI[count].gameObject.SetActive(true);
            BonusGUI[count].sprite = o.Sprite;
            Life += o.Life;
            Damage += o.Damage;
            Speed += o.Speed;
            AtkSpeed += o.AtkSpeed;
            ++count;
        }

        count = 0;
        foreach (TraitScriptableObject o in Malus)
        {
            MalusGUI[count].gameObject.SetActive(true);
            MalusGUI[count].sprite = o.Sprite;
            Life += o.Life;
            Damage += o.Damage;
            Speed += o.Speed;
            AtkSpeed += o.AtkSpeed;
            ++count;
        }
    }

    public void Update()
    {
        LifeBar.value = ((float)CurrentLife / (float)Life);
    }
}
