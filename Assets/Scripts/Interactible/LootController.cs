using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {

    public bool IsBenefic;
    public bool IsInterupt;

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
            collider.GetComponent<HeroStats>().WinATrait(IsBenefic);
            IsUsed = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
