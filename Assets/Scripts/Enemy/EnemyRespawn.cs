using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour {

    private GameObject Enemy;
    public GameObject EnemyPref;

    void Start()
    {
        Enemy = Instantiate(EnemyPref, this.transform.position, EnemyPref.transform.rotation);
    }

    public void Reload()
    {
        GameObject.Destroy(Enemy);
        Enemy = Instantiate(EnemyPref, this.transform.position, EnemyPref.transform.rotation);
    }

}
