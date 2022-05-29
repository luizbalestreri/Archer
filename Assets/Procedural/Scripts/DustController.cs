using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    SpriteRenderer spriteRender;
    Animator animator;
    void Awake(){
        animator = transform.GetComponent<Animator>();
        spriteRender = transform.GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    public void EndDust(){
        animator.Play("dustend");  
    }

    public void Update(){
        if (Input.GetKeyDown(KeyCode.Space)) EndDust();
    }

}
