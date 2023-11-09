using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TestScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        //if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        //{
        //    Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //}
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        //{
        //    animator.Play("Breath");
        //}
    }

    public async void Attack()
    {
        System.Threading.Tasks.Task task = CheckAnim(animator, "Attack");
        animator.Play("Attack");
        await task;
        Debug.Log(4);
        animator.Play("Breath");
    }

    public async System.Threading.Tasks.Task CheckAnim(Animator animator, string aniName)
    {
        Debug.Log(0);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(aniName))
        {
            Debug.Log(1);
            await System.Threading.Tasks.Task.Delay((int)(Time.deltaTime*1000));
        }
        while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            Debug.Log(2);
            await System.Threading.Tasks.Task.Delay((int)(Time.deltaTime * 1000));
        }
        Debug.Log(3);
    }
}
