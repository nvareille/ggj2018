using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heritage/Trait")]
public class TraitScriptableObject : ScriptableObject
{
    public Sprite Sprite;
    public int Life = 0;
    public int Damage = 0;
    public float Speed = 0;
    public float AtkSpeed = 0;
}
