using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField]
    public GameObject lobbyControls;

    [SerializeField]
    public Text coinsText;

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

    void LateUpdate()
    {
        coinsText.text = PlayerDataState.coins.ToString();
    }
}
