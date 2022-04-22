using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowString : MonoBehaviour
{
    bool arrowEngaged = false;
    GameObject top;
    GameObject bottom;
    GameObject center;
    Vector2 centerReset;
    [SerializeField]
    GameObject arrowPrefab;
    GameObject arrow;
    float arrowOffset = 0.3f;
    LineRenderer topLineRenderer;
    LineRenderer bottomLineRenderer;
    Vector2 click;
    Vector2 centerPosition;
    // Start is called before the first frame update
    void Awake(){
        top = transform.GetChild(0).gameObject;
        bottom = transform.GetChild(1).gameObject;
        center = transform.GetChild(2).gameObject;
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
        click = center.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));   
        centerPosition = center.transform.localPosition;
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
            RotateObject(gameObject, clickPosition, 180, 80);
            RotateObject(arrow, gameObject.transform.position, 0, 80);
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

    void MoveString(Vector2 clickPosition, int speed){
        Vector3 centerPosition = center.transform.position;
        clickPosition = transform.InverseTransformPoint(clickPosition);   
        clickPosition.x = Mathf.Clamp(clickPosition.x, -.85f, -.2f);
        clickPosition.y = Mathf.Clamp(clickPosition.y, -1, 1);
        float distance = speed/Vector2.Distance(center.transform.localPosition, clickPosition);
        center.transform.localPosition = Vector2.Lerp(center.transform.localPosition, clickPosition, distance * Time.deltaTime);
        //center.transform.localPosition = clickPosition;
        bottomLineRenderer.SetPosition(1, centerPosition);
        topLineRenderer.SetPosition(1, centerPosition);
        MoveArrow();
    }

    void MoveArrow(){
        Vector3 centerPosition = center.transform.position;
        arrow.transform.position = centerPosition;
    }

    void RotateObject(GameObject obj, Vector2 position, int offset, int speed){
        Vector2 direction = position - (Vector2) obj.transform.position;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + offset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, rotation, speed * Time.deltaTime);
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

}
