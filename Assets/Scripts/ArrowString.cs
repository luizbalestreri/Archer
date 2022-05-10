using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowString : MonoBehaviour
{   
    GameObject bow;
    bool arrowEngaged = false;
    private Touch theTouch, theTouch2;
    private int maxTapCount = 2;
    GameObject top, bottom, center, arrow;
    [SerializeField]
    GameObject arrowPrefab;
    float arrowOffset = 0.5f;
    LineRenderer topLineRenderer, bottomLineRenderer;
    Vector2 centerReset;
    // Start is called before the first frame update
    void Awake(){
        bow = transform.GetChild(0).gameObject;
        top = bow.transform.GetChild(0).gameObject;
        bottom = bow.transform.GetChild(1).gameObject;
        center = bow.transform.GetChild(2).gameObject;
        centerReset = center.transform.localPosition;
        topLineRenderer = top.GetComponent<LineRenderer>();
        bottomLineRenderer = bottom.GetComponent<LineRenderer>();
    }
    void Start()
    {
        ResetLineRenderer();
    }

    // Update is called once per frame
    void Update()
    {   
        HandleTouch();
    }


    void HandleTouch(){
        if (Input.touchCount > 0){
            theTouch = Input.GetTouch(0);
            if(theTouch.phase== TouchPhase.Ended && arrowEngaged){
                GameObject.Destroy(arrow);
                ResetLineRenderer();
                arrowEngaged = false;
            }
            if(theTouch.phase == TouchPhase.Moved){
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                transform.position = clickPosition;
                topLineRenderer.SetPosition(0, top.transform.position);
                bottomLineRenderer.SetPosition(0, bottom.transform.position);
                topLineRenderer.SetPosition(1, center.transform.position);
                bottomLineRenderer.SetPosition(1, center.transform.position);
                if(arrowEngaged && Input.touchCount > 1) {
                    MoveArrow();
                    RotateObject(gameObject, Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position), 90);
                    RotateObject(arrow, bow.transform.position, 0);
                }
            }
        }
        if(Input.touchCount > 1){
            if(Input.touchCount > 1){
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position);
                theTouch = Input.GetTouch(1);
                if(theTouch.phase == TouchPhase.Began && !arrowEngaged){
                    clickPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                    Vector2 centerPosition = center.transform.position;
                    if (Vector2.Distance(clickPosition, centerPosition) <= arrowOffset){
                        RespawnArrow();
                        arrowEngaged = true;
                    }
                }
                if(theTouch.phase == TouchPhase.Moved && arrowEngaged){
                    clickPosition = Camera.main.ScreenToWorldPoint(theTouch.position);
                    RotateObject(gameObject, clickPosition, 90);
                    RotateObject(arrow, bow.transform.position, 0);
                    MoveLineRenderer();
                    MoveString(clickPosition, 100);
                }
                if(theTouch.phase == TouchPhase.Ended && arrowEngaged){
                    arrowEngaged = false;
                    float distance = Vector2.Distance(centerReset, center.transform.localPosition);
                    if (distance > 0.01f){
                        arrow.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * distance * 3000);
                        arrow.GetComponent<Arrow>().shot = true;
                    } else {GameObject.Destroy(arrow);}
                    ResetLineRenderer();   
                }
            }
        }
    }

    void MoveString(Vector2 clickPosition, int speed){
        Vector3 centerPosition = center.transform.position;
        clickPosition = bow.transform.InverseTransformPoint(clickPosition);   
        clickPosition.x = Mathf.Clamp(clickPosition.x, -.85f, -.2f);
        clickPosition.y = Mathf.Clamp(clickPosition.y, -1, 1);
        float distance = speed/Vector2.Distance(center.transform.localPosition, clickPosition);
        center.transform.localPosition = Vector2.Lerp(center.transform.localPosition, clickPosition, distance * Time.deltaTime);
        bottomLineRenderer.SetPosition(1, centerPosition);
        topLineRenderer.SetPosition(1, centerPosition);
        MoveArrow();
    }

    void MoveArrow(){
        Vector3 centerPosition = center.transform.position;
        arrow.transform.position = centerPosition;
    }

    void RotateObject(GameObject obj, Vector2 position, int offset){
        Vector2 direction = position - (Vector2) obj.transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + offset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = rotation;
    }
    void ResetLineRenderer(){
        center.transform.localPosition = centerReset;
        topLineRenderer.SetPosition(0, top.transform.position);
        bottomLineRenderer.SetPosition(0, bottom.transform.position);
        topLineRenderer.SetPosition(1, center.transform.position);
        bottomLineRenderer.SetPosition(1, center.transform.position);
    }

    void MoveLineRenderer(){
        topLineRenderer.SetPosition(0, top.transform.position);
        bottomLineRenderer.SetPosition(0, bottom.transform.position);
    }

    void RespawnArrow(){
        //GameObject.Destroy(arrow);
        arrow = GameObject.Instantiate(arrowPrefab, center.transform.position, transform.rotation);
        arrowEngaged = true;
    }







    
    void TestMouse(){
        if (Input.GetMouseButton(0)){
            Debug.Log( transform.right);
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (clickPosition - (Vector2)transform.position);//.normalized;
            float distance = Vector3.Distance(transform.position , clickPosition);
            
            transform.position = (Vector2) clickPosition - (Vector2) transform.right;
             
            //Debug.Log(direction);
            //(Vector2) transform.position + (direction * distance)
        }
    }

    void HandleMouse(){
        if (Input.GetMouseButton(0) && !arrowEngaged){
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 centerPosition = center.transform.position;
            if (Vector2.Distance(clickPosition, centerPosition) <= arrowOffset){
                RespawnArrow();
                arrowEngaged = true;
            }
        }

        if (arrowEngaged){
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RotateObject(gameObject, clickPosition, 180);
            RotateObject(arrow, gameObject.transform.position, 0);
            MoveLineRenderer();
            MoveString(clickPosition, 100);
        }

        if (Input.GetMouseButtonUp(0) && arrowEngaged){
            arrowEngaged = false;
            float distance = Vector2.Distance(centerReset, center.transform.localPosition);
            if (distance > 0.01f){
                arrow.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * distance * 3000);
                arrow.GetComponent<Arrow>().shot = true;
            } else {GameObject.Destroy(arrow);}
            ResetLineRenderer();
        }     
    }

}
