using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = gameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("GameObject");
    }

    public void StartAnim()
    {
        //gameObject.AddComponent<SpriteRenderer>();
        GetComponent<Animator>().Play("Test");
        //animator.Play("Test");
    }
}
