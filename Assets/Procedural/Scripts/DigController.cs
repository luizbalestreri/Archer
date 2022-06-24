using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigController : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    bool visible = true;
    [SerializeField]
    GameObject dust;
    [SerializeField]
    GameObject hole;
    Animator animator;
    void Awake(){
        //animator = transform.GetComponent<Animator>();
        wormController.wormParentAnimator.enabled = false;
    }
    // Start is called before the first frame update
    // void OnCollisionEnter2D(Collision2D col){
    //     Debug.Log(col.gameObject.tag);
    //     if(col.gameObject.tag == "Hole"){
    //         Debug.Log("oi");
    //         visible = !visible;
    //         transform.GetComponent<SpriteRenderer>().enabled = visible;
    //     }
    // }
    public void DigInStart(){
        StartCoroutine("DigIn");
    }
    public void DigOutStart(){
        StartCoroutine("DigOut");
    }
    void DigEnd(){
        dust.GetComponent<DustController>().EndDust();
        hole.GetComponent<Animator>().enabled = true;
        // GameObject.Destroy(dust, 1);
        // GameObject.Destroy(hole, 1);
    }
    

    IEnumerator DigIn(){
        for (float i = wormController.pathFollower.speed; i > 0; i -= 0.1f){
            wormController.pathFollower.speed = i;
            yield return new WaitForSeconds(0.05f);
        }
        wormController.pathFollower.walking = false;
        yield return wormController.pathFollower.RearrangeBody(-90);
        wormController.wormParentAnimator.enabled = true;
        // wormController.wormParentAnimator.Play("Digging");
        yield return new WaitForSeconds(1.1f);
        Quaternion rotation = transform.GetChild(0).rotation; 
        rotation *= Quaternion.Euler(0, 0, 90);
        dust = GameObject.Instantiate(dust, transform.GetChild(0).position + transform.GetChild(0).right * 0.6f, rotation);
        hole = GameObject.Instantiate(hole, transform.GetChild(0).position + transform.GetChild(0).right * 0.6f, rotation);
        yield return new WaitForSeconds(0.5f);
        DigEnd();
        yield return new WaitForSeconds(0.2f);
        wormController.pathFollower.dstTravelled +=5;
        wormController.pathFollower.walking = true;
        wormController.pathFollower.speed = 3;
        wormController.wormParentAnimator.enabled = false;
    }

    IEnumerator DigOut(){
        wormController.pathFollower.walking = false;
        yield return wormController.pathFollower.RearrangeBody(90);
        Quaternion rotation = transform.GetChild(0).rotation; 
        rotation *= Quaternion.Euler(0, 0, -90);
        transform.position-= new Vector3(0, 3f, 0);
        dust = GameObject.Instantiate(dust, transform.GetChild(0).position + transform.GetChild(0).right * 0.6f, rotation);
        hole = GameObject.Instantiate(hole, transform.GetChild(0).position + transform.GetChild(0).right * 0.6f, rotation);
        wormController.wormParentAnimator.enabled = true;
        wormController.wormParentAnimator.Play("DigOut");
        yield return new WaitForSeconds(1.2f);
        // wormController.pathFollower.dstTravelled +=5;
        wormController.wormParentAnimator.enabled = false;
        yield return wormController.pathFollower.ReturnToPath();
        wormController.WalkState();
        DigEnd();
        //Criar path entre onde está e o dstTravel
    }


////////////////////////////////////////////////////////////////////////////
    const string animBaseLayer = "Base Layer";
    int digAnimHash = Animator.StringToHash(animBaseLayer + ".DigOut");
    int moveAnimHash = Animator.StringToHash(animBaseLayer + ".Move");
    int lookAnimHash = Animator.StringToHash(animBaseLayer + ".Look");

    public IEnumerator PlayAndWaitForAnim(Animator targetAnim, string stateName)
    {
        //Get hash of animation
        int animHash = 0;
        if (stateName == "DigOut")
            animHash = digAnimHash;
        else if (stateName == "Move")
            animHash = moveAnimHash;
        else if (stateName == "Look")
            animHash = lookAnimHash;

        //targetAnim.Play(stateName);
        targetAnim.CrossFadeInFixedTime(stateName, 0.6f);

        //Wait until we enter the current state
        while (targetAnim.GetCurrentAnimatorStateInfo(0).fullPathHash != animHash)
        {
            yield return null;
        }

        float counter = 0;
        float waitTime = targetAnim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Done playing. Do something below!
        Debug.Log("Done Playing");

    }
}
