using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float velocityBeforePhysicsUpdate;
    public bool shot = false;
    int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void FixedUpdate(){
        velocityBeforePhysicsUpdate = gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
        if (shot){
                if(timer < 60)
                timer++;
                if (timer >= 60){
                    if(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude < 1){
                        gameObject.GetComponent<Collider2D>().enabled = false;
                    }
                }
            }
        if(!GetComponent<Renderer>().isVisible){
            GameObject.Destroy(gameObject, 1);
        }
    }

    public void DebugVelocity(){
        Debug.Log("a " +  velocityBeforePhysicsUpdate);
    }
}
