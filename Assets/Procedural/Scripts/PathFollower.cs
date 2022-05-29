using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class PathFollower : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float speed;
    float dstTravelled = 15;
    void Start(){
        transform.position = pathCreator.path.GetPoint(0);
    }
    void Update()
    {
        dstTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        transform.rotation = Quaternion.FromToRotation(Vector3.up, pathCreator.path.GetNormalAtDistance(dstTravelled));
    }
}
