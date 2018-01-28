using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public int Damage = 3;
    public float Speed;

    private bool IsLeft = false;
    public Transform Target;


    void Start()
    {
        Target = FindObjectOfType<HeroStats>().transform;
        Vector3 tmp = new Vector3(Target.position.x, Target.position.y + 1, Target.position.z);
        transform.parent.LookAt(tmp);
        //transform.rotation = Quaternion.Euler(tmp.x, tmp.y, tmp.z);
    }

    public void Init(Transform targ)
    {

    }

    void Update()
    {
        transform.parent.Translate(Vector3.forward * Time.deltaTime * Speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<AStats>().GetHit(Damage);
            GameObject.Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            GameObject.Destroy(this.gameObject);
        }
        
    }
}
