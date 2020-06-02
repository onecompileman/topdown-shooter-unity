using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponItemShopDisplay : MonoBehaviour
{
    [SerializeField]
    public Image image;

    [SerializeField]
    public Text price;

    [SerializeField]
    public Text name;

    [SerializeField]
    public WeaponItemShop weaponItem;

    [SerializeField]
    public RawImage buyButton;

    [SerializeField]
    public ConfirmationUI confirmation;


    private bool disableBuy;

    private Color disabledColor;

    private Color enabledColor;

    private AudioSource audioSource;


    void Start()
    {
        image.sprite = weaponItem.image;
        price.text = weaponItem.price.ToString();
        name.text = weaponItem.name;
        disabledColor = new Color(0.3f, 0.3f, 0.3f);
        enabledColor = new Color(1, 0, 0.32f);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (PlayerDataState.weapons.Contains(weaponItem.name))
        {
            disableBuy = true;
            buyButton.color = new Color(disabledColor.r, disabledColor.g, disabledColor.b);
            buyButton.gameObject.GetComponentInChildren<Text>().text = "SOLD";
        }
        else if (PlayerDataState.coins < weaponItem.price)
        {
            disableBuy = true;
            buyButton.color = new Color(disabledColor.r, disabledColor.g, disabledColor.b);
            buyButton.gameObject.GetComponentInChildren<Text>().text = "BUY";
        }
        else
        {
            disableBuy = false;
            buyButton.color = new Color(enabledColor.r, enabledColor.g, enabledColor.b);
            buyButton.gameObject.GetComponentInChildren<Text>().text = "BUY";
        }
    }


    public void Buy()
    {
        if (!disableBuy)
        {
            confirmation.gameObject.SetActive(true);
            confirmation.ShowConfirmation($"Are you sure to buy {weaponItem.name} at {weaponItem.price} coins", () =>
            {
                PlayerDataState.weapons.Add(weaponItem.name);
                PlayerDataState.coins -= weaponItem.price;

                audioSource.Play();

                SaveSystem.SavePlayerData();
            }, () => { });
        }
    }


}
