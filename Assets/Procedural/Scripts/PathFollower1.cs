using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class PathFollower1 : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    public bool walking = false;
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float speed;
    [Range(0, 100)]
    public float dstTravelled;
    void Start(){
        transform.position = pathCreator.path.GetPoint(0);
    }
    void Update()
    {
        if (walking){
            dstTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, pathCreator.path.GetNormalAtDistance(dstTravelled));
        }
        if(dstTravelled > 5){
            
            wormController.DigState();
        }
    }
}
