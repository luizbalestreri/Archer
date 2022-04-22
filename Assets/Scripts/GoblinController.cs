using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonsterController
{

    Animator _animator;
    void Awake(){
        _animator = gameObject.GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D col){
        //GameObject.Destroy(col.gameObject,0.5f);
        health -= col.gameObject.GetComponent<Arrow>().velocityBeforePhysicsUpdate;;
        col.transform.parent = gameObject.transform;
        col.collider.enabled = false;
        col.rigidbody.Sleep();
        Hitted(col);
        StartCoroutine("DamageEffect"); 
    }
    void Hitted(Collision2D col){
        float posX = Mathf.Clamp(col.transform.localPosition.x, -.25f, 0.25f);
        col.transform.localPosition = new Vector3(posX, -.25f, 0.1f);
    }

    new void Die(){
        _animator.SetBool("Dead", true);
        speed = 0;
        
        foreach (Transform c in gameObject.transform){
            GameObject.Destroy(c.gameObject, 0.1f);
        }
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
    IEnumerator DamageEffect(){
        float aux = speed;
        speed = 0;
        _animator.SetBool("Hit", true);
        Color original = spriteRenderer.color;
        Color[] colorArray = new Color[]{original, Color.red};
        for (int i = 0; i<3; i++){ 
            spriteRenderer.color = colorArray[i%2];
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = original;
        _animator.SetBool("Hit", false);
        speed = aux;
        if (health <= 0){
            Die();
        }
        yield return null;
    }
}
