using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [SerializeField]
    public GameObject lobbyControls;

    [SerializeField]
    public CameraFollow camera;
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
        lobbyControls.SetActive(true);
        camera.isFollowingPlayer = true;
    }
}
