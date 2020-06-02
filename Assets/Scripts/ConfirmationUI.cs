using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationUI : MonoBehaviour
{

    [SerializeField]
    public Text messsageText;

    [SerializeField]
    public Animator animator;

    private Action callback;

    private Action confirmCallback;

    private Action cancelCallback;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowConfirmation(string message, Action confirm, Action cancel)
    {
        messsageText.text = message;
        confirmCallback = confirm;
        cancelCallback = cancel;
        PlayOpenAnimation();
    }

    public void PlayCloseAnimation()
    {
        animator.Play("CloseUI");
    }

    public void PlayOpenAnimation()
    {
        animator.Play("OpenUI");
    }

    public void Confirm()
    {
        callback = confirmCallback;
        PlayCloseAnimation();
    }

    public void Cancel()
    {
        callback = cancelCallback;
        PlayCloseAnimation();
    }

    public void Close()
    {
        if (callback != null)
        {
            callback();
        }
        gameObject.SetActive(false);
    }
}
