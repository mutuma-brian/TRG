using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class PlayerAnimation : MonoBehaviour
{


    protected Animator animator;
    public Transform rightHandObj, leftHandObj = null;
    public Transform player;


    void Awake()
    {
        animator = player.GetComponent<Animator>();
    }
    
    void Update()
    {
        animator.SetFloat("Steer", transform.root.GetComponent<BuggyControl>().steer);
    }

    void OnAnimatorIK()
    {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);


            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
        
    }

}