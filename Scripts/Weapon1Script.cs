using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1Script : MonoBehaviour {

    public float speed;
    public float lifeTime;

    public float damage;

	// Use this for initialization
	void Start () {
        Invoke("DestroyProjectile", lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
	}

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
            col.GetComponent<DestroyObject>().TakeDamage(damage);
        }

        if (col.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }

        if(col.gameObject.tag == "Boss")
        {
            Destroy(this.gameObject);
            col.GetComponent<Boss>().TakeDamage(damage);
        }
    }
}
