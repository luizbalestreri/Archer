using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
    [SerializeField]
    internal Animator wormParentAnimator;

    [SerializeField]
    internal BodyController bodyController;
    [SerializeField]
    internal PathFollower pathFollower;
    [SerializeField]
    internal DigController digController;
    public enum state {walk, attack, dig, die, digOut};
    public state currentState;
    // Start is called before the first frame update
    void Start()
    {
        WalkState();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void WalkState(){
        if (currentState != state.walk){
            currentState = state.walk;
            pathFollower.walking = true;
        }
    }

    public void AttackState(){
        if (currentState != state.attack){
            currentState = state.attack;
        }
    }

    public void DigState(){
        if (currentState != state.dig){
            currentState = state.dig;
            digController.DigInStart();
        }
    }

    public void DigOutState(){
        if (currentState != state.digOut){
            currentState = state.digOut;
            digController.DigOutStart();
        }
    }

    void DieState(){
        if (currentState != state.die){
            currentState = state.die;
        }
    }
}
