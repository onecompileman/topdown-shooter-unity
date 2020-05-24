using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutUIController : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayCloseAnimation()
    {
        animator.Play("CloseUI");
    }

    public void PlayOpenAnimation()
    {
        animator.Play("OpenUI");
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }


}
