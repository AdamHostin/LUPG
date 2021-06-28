using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetter : MonoBehaviour
{
    private void OnEnable()
    {
        Animator animator  = gameObject.GetComponent<Animator>();
        transform.parent.GetComponent<CharacterController2D>().SetAnimator(animator);

    }


}
