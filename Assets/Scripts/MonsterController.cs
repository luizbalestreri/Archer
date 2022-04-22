using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    protected float health = 50;
    [SerializeField]
    protected float speed = 0.5f;
    protected SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = speed/Vector2.Distance(transform.position,player.transform.position);
        transform.position = Vector2.Lerp(transform.position, player.transform.position, distance * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col){
        //GameObject.Destroy(col.gameObject,0.5f);
        col.transform.parent = gameObject.transform;
        health -= col.rigidbody.velocity.magnitude;
        col.collider.enabled = false;
        col.rigidbody.Sleep();
        Hitted(col);
        StartCoroutine("DamageEffect");
        if (health <= 0){
            Die();
        }
    }

    void Hitted(Collision2D col){
        float posY = Mathf.Clamp(col.transform.localPosition.y, -.5f, 0);
        col.transform.localPosition = new Vector2(0, posY);
    }



    protected void Die(){
        speed = 0;
        gameObject.transform.Rotate(0, 0, -90);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    //fazer die() e tirar collider
}
