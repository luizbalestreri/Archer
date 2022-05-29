using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigController : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    bool visible = true;
    GameObject dust;
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D col){
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.tag == "Hole"){
            Debug.Log("oi");
            visible = !visible;
            transform.GetComponent<SpriteRenderer>().enabled = visible;
        }
    }
    void DigStart(){
        dust = GameObject.Instantiate(dust, transform.position, Quaternion.identity);
    }

    void DigEnd(){
        dust.GetComponent<DustController>().EndDust();
    }
}
