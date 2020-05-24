using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    [SerializeField]
    public GameObject shopKeeper;
    [SerializeField]
    public GameObject lobbyGem;

    [SerializeField]
    public AboutUIController loading;

    [SerializeField]
    public ShopUIController shop;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    public CameraFollow camera;

    [SerializeField]
    public float distanceRadius;

    [SerializeField]
    public Text interactText;

    [SerializeField]
    public GameObject lobbyControls;

    [HideInInspector]
    public string mode;


    // Update is called once per frame
    void FixedUpdate()
    {
        var playerPos = player.transform.position;
        if (Vector3.Distance(playerPos, shopKeeper.transform.position) <= distanceRadius)
        {
            mode = "shop";
            interactText.text = "Press to shop";
        }
        else if (Vector3.Distance(playerPos, lobbyGem.transform.position) <= distanceRadius)
        {
            mode = "play";
            interactText.text = "Press to play";
        }
        else
        {
            mode = "";
            interactText.text = "";
        }
    }

    public void Interact()
    {
        switch (mode)
        {
            case "shop":
                lobbyControls.SetActive(false);
                camera.MoveToShopKeeper(() =>
                {
                    shop.gameObject.SetActive(true);
                    shop.PlayOpenAnimation();
                });

                break;
            case "play":
                lobbyControls.SetActive(false);
                camera.MoveToFloorGem(() =>
                {
                    loading.gameObject.SetActive(true);
                    loading.PlayOpenAnimation();
                });
                break;
        }
    }
}
