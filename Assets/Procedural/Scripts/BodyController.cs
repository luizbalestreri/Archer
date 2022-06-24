using System.Collections; 
using System. Collections. Generic; 
using UnityEngine; 


public class BodyController : MonoBehaviour 
{ 
[SerializeField]
WormController wormController;
public int length; 
public Vector3[] segmentPoses;
private Vector3[] segmentV; 
public Transform targetDir;

public float targetDist;
public float smoothSpeed; 
public Transform[] bodyParts;
//public Transform tailEnd;
public float wiggle;
public bool wiggleSign = false;

    private void Start() { 
        segmentPoses = new Vector3[length]; 
        segmentV = new Vector3[length]; 
    } 
    private void Update() { 
        segmentPoses[0] = targetDir.position; 
        for (int i = 1; i < segmentPoses.Length; i++){ 
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist; 
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed); 
            bodyParts[i - 1].transform.position = segmentPoses[i];
        }
        RotatePart();
        Wiggle();
    }

    private void RotatePart(){
        bodyParts[0].right = (transform.position) - bodyParts[0].transform.position;
        bodyParts[0].Rotate(0,0,wiggle);
        for (int i = 1; i < bodyParts.Length; i++){
            bodyParts[i].right = bodyParts[i - 1].transform.position - bodyParts[i].transform.position;
            bodyParts[i].Rotate(0,0,wiggle);
        }
    }

    private void Wiggle(){
        if(Mathf.Abs(wiggle) > 7) wiggleSign = !wiggleSign;
        wiggle += wiggleSign? 0.3f : -0.3f;
    }
}