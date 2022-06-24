using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class PathFollower : MonoBehaviour
{
    [SerializeField]
    WormController wormController;
    public bool walking = false;
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float speed;
    [Range(0, 100)]
    public float dstTravelled;
    [SerializeField]
    Transform[] bodyParts;
    
    void Awake(){
        bodyParts = GetComponentsInChildren<Transform>();
    }
    void Start(){
        Debug.Log(bodyParts[0].name);
        // Time.timeScale = 0.3f;
        // dstTravelled = new float[bodyParts.Length];
        for (int i = 0; i < bodyParts.Length; i++){
            bodyParts[i].position = pathCreator.path.GetPoint(i);
            // dstTravelled[i] = -(i * 0.6f);
        }
    }
    void Update()
    {
        if (walking){
            dstTravelled += speed * Time.deltaTime;
            bodyParts[0].position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
            bodyParts[0].rotation = Quaternion.FromToRotation(Vector3.up, pathCreator.path.GetNormalAtDistance(dstTravelled));
            for (int i = 1; i < bodyParts.Length; i++){
                bodyParts[i].position = pathCreator.path.GetPointAtDistance(dstTravelled -((i - 1) * 0.6f), end);
                bodyParts[i].rotation = Quaternion.FromToRotation(Vector3.up, pathCreator.path.GetNormalAtDistance(dstTravelled -((i - 1) * 0.6f)));
            }
        }
        if(dstTravelled > 2 && dstTravelled < 3){
            
             wormController.DigState();
        }

        if (dstTravelled > 15 && wormController.currentState == WormController.state.dig){
            wormController.DigOutState();
        }
    }

    public IEnumerator RearrangeBody(int degree){
        // float time = 0;
        Quaternion originalRotation = bodyParts[0].rotation; 
        Vector2[] positionArray = new Vector2[bodyParts.Length];
        Quaternion[] rotationArray = new Quaternion[bodyParts.Length];
        for (int i = 0; i < bodyParts.Length; i++){
            positionArray[i] = bodyParts[i].position;
            rotationArray[i] = bodyParts[i].rotation;
        }
        float t = 0.1f;
        for (float j = 0; j < t; j+= Time.deltaTime){
            bodyParts[0].rotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0,0,degree), j * (1/t));
            Vector2 target = bodyParts[1].position + bodyParts[1].right * 0.6f;
            for (int i = 2; i < bodyParts.Length; i++){
                bodyParts[i].position = Vector2.Lerp(a: positionArray[i], (target) - ((Vector2)bodyParts[1].right * i * 0.6f), j*(1/t));
                bodyParts[i].rotation = Quaternion.Lerp(a: rotationArray[i], bodyParts[0].rotation, t: j*(1/t));
                yield return null;
            }
            yield return null;
        }
    }

    public IEnumerator ReturnToPath(){
        walking = false;
        bool flag = false;
        bool flag2 = false;
        Vector3 destination = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        Debug.Log(destination);
        bool[] walkingArray = new bool[bodyParts.Length];
        for ( int i = 0; i < walkingArray.Length;i++ ) {
            walkingArray[i] = false;
        }
        while(!flag){
            if (flag2){
                dstTravelled += speed * Time.deltaTime;
            }
            for (int i = 1; i < bodyParts.Length; i++){
                if(!walkingArray[i]){
                    print(i + ": " + Vector2.Distance(bodyParts[i].position, destination) +  ", " + walkingArray[i]);
                    if(Vector2.Distance(bodyParts[i].position, destination) > 0.01f){
                        Vector3 direction = (destination - bodyParts[i].position).normalized;
                        bodyParts[i].position += direction * speed * Time.deltaTime;
                    } else {
                        walkingArray[i] = true;
                        flag2 = true;
                    }
                } else {
                    bodyParts[i].position = pathCreator.path.GetPointAtDistance(dstTravelled -((i - 1) * 0.6f), end);
                    bodyParts[i].rotation = Quaternion.FromToRotation(Vector3.up, pathCreator.path.GetNormalAtDistance(dstTravelled -((i - 1) * 0.6f)));
                } 
            } 
            for (int i = 1; i < walkingArray.Length; i++){
                flag = true;
                if(walkingArray[i]) continue;
                else{flag = false; 
                    for (int j = 1; j < bodyParts.Length; j++){
                        print(j + ": " + Vector2.Distance(bodyParts[j].position, destination) +  ", " + walkingArray[j]);
                    }
                    break;
                }
            }
            yield return null;
        } 
    }
}
