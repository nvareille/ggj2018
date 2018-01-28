using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponController : MonoBehaviour {
    [Header("Configuration")]
    public float Delay = 0.6f;

    [Header("Components")]
    [SerializeField]
    private GameObject PrefRange;

    public void Attack()
    {
        Invoke("LateAttack", Delay);
    }

    private void LateAttack()
    {
        Instantiate(PrefRange, this.transform.position, Quaternion.identity);
    }
}
