using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : AStats
{
    public Slider LifeBar;
    


    public void Awake()
    {
        
    }

    public void Update()
    {
        LifeBar.value = ((float)CurrentLife / (float)Life);
    }
}
