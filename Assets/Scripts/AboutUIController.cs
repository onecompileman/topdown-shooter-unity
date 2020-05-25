﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutUIController : MonoBehaviour
{

    [SerializeField]
    public Animator animator;

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
