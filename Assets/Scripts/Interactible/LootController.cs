using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    
    public bool IsInterupt;

    public TraitScriptableObject Trait;

    private int CharacterLayer;
    private bool IsUsed;

	// Use this for initialization
	void Start () {
        CharacterLayer = LayerMask.NameToLayer("Character");
	}

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == CharacterLayer && !IsInterupt && Input.GetButtonDown("Interact") && !IsUsed)
        {
            collider.GetComponent<HeroStats>().WinATrait(Trait);
            IsUsed = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
