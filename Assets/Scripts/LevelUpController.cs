using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpController : MonoBehaviour {
    [SerializeField]
    public GameObject lobbyControls;

    [SerializeField]
    public GameObject upgradeLife;

    [SerializeField]
    public GameObject upgradeMana;

    [SerializeField]
    public GameObject upgradeWeapon;

    [SerializeField]
    public GameObject upgradeCompanion;

    [SerializeField]
    public Text lifeGemText;

    [SerializeField]
    public Text manaGemText;

    [SerializeField]
    public Text weaponGemText;

    [SerializeField]
    public Text companionGemText;

    [SerializeField]
    public Text lifeText;

    [SerializeField]
    public Text gemText;

    [SerializeField]
    public Text manaText;

    [SerializeField]
    public List<RawImage> weaponSlotsUpgrade;

    [SerializeField]
    public List<RawImage> companionSlotsUpgrade;

    [SerializeField]
    public CameraFollow camera;
    public Animator animator;

    public AudioSource audioSource;

    private List<LifeUpgrade> lifeUpgrades = new List<LifeUpgrade> {
        new LifeUpgrade {
        life = 150,
        gems = 20
        },
        new LifeUpgrade {
        life = 200,
        gems = 30
        },
        new LifeUpgrade {
        life = 250,
        gems = 30
        },
        new LifeUpgrade {
        life = 350,
        gems = 50
        },
        new LifeUpgrade {
        life = 400,
        gems = 50
        },
        new LifeUpgrade {
        life = 450,
        gems = 50
        }
    };
    private List<ManaUpgrade> manaUpgrades = new List<ManaUpgrade> {
        new ManaUpgrade {
        mana = 200,
        gems = 20
        },
        new ManaUpgrade {
        mana = 250,
        gems = 30
        },
        new ManaUpgrade {
        mana = 300,
        gems = 30
        },
        new ManaUpgrade {
        mana = 400,
        gems = 50
        },
        new ManaUpgrade {
        mana = 450,
        gems = 50
        },
        new ManaUpgrade {
        mana = 500,
        gems = 50
        }
    };

    private List<int> weaponSlots = new List<int> { 0, 0, 20, 20, 20, 20, 50 };

    private List<int> companionSlots = new List<int> { 80, 100, 120 };

    private int weaponSlotUpgrdadeIndex = 2;

    private int lifeUpgradeSlotIndex = 0;

    private int manaUpgradeSlotIndex = 0;

    private int companionSlotUpgradeIndex = 0;

    private Color disabledColor = new Color (0.3f, 0.3f, 0.3f);
    private Color enabledColor = new Color (1, 0, 0.32f);
    void Start () {
        animator = GetComponent<Animator> ();
        audioSource = GetComponent<AudioSource> ();
    }

    void Update () {
        GetUpgradeIndex ();
        UpdateUI ();
    }

    public void PlayCloseAnimation () {
        animator.Play ("CloseUI");
    }

    public void PlayOpenAnimation () {
        animator.Play ("OpenUI");
    }

    public void Close () {
        lobbyControls.SetActive (true);
        camera.isFollowingPlayer = true;
        gameObject.SetActive (false);
    }

    public void UpgradeLife () {
        Debug.Log (lifeUpgradeSlotIndex + " " + PlayerDataState.gems.ToString ());

        if (lifeUpgradeSlotIndex != -1 && PlayerDataState.gems >= lifeUpgrades[lifeUpgradeSlotIndex].gems) {
            Debug.Log (lifeUpgradeSlotIndex);

            PlayerDataState.gems -= lifeUpgrades[lifeUpgradeSlotIndex].gems;
            PlayerDataState.life = lifeUpgrades[lifeUpgradeSlotIndex].life;

            audioSource.Play ();
            SaveSystem.SavePlayerData ();
        }
    }

    public void UpgradeMana () {
        if (manaUpgradeSlotIndex != -1 && PlayerDataState.gems >= manaUpgrades[manaUpgradeSlotIndex].gems) {
            PlayerDataState.gems -= manaUpgrades[manaUpgradeSlotIndex].gems;
            PlayerDataState.mana = manaUpgrades[manaUpgradeSlotIndex].mana;

            audioSource.Play ();
            SaveSystem.SavePlayerData ();
        }
    }

    public void UpgradeWeapon () {
        if (weaponSlotUpgrdadeIndex != -1 && PlayerDataState.gems >= weaponSlots[weaponSlotUpgrdadeIndex]) {
            PlayerDataState.gems -= weaponSlots[weaponSlotUpgrdadeIndex];
            PlayerDataState.maxWeaponSlots++;

            audioSource.Play ();
            SaveSystem.SavePlayerData ();
        }
    }

    public void UpgradeCompanion () {
        if (companionSlotUpgradeIndex != -1 && PlayerDataState.gems >= companionSlots[companionSlotUpgradeIndex]) {
            PlayerDataState.gems -= companionSlots[companionSlotUpgradeIndex];
            PlayerDataState.maxCompanionSlots++;

            audioSource.Play ();
            SaveSystem.SavePlayerData ();
        }
    }

    private void UpdateUI () {
        gemText.text = PlayerDataState.gems.ToString ();
        lifeText.text = PlayerDataState.life.ToString ();
        manaText.text = PlayerDataState.mana.ToString ();

        lifeGemText.text = lifeUpgradeSlotIndex != -1 ? lifeUpgrades[lifeUpgradeSlotIndex].gems.ToString () : "0";
        manaGemText.text = manaUpgradeSlotIndex != -1 ? manaUpgrades[manaUpgradeSlotIndex].gems.ToString () : "0";
        weaponGemText.text = weaponSlotUpgrdadeIndex != -1 ? weaponSlots[weaponSlotUpgrdadeIndex].ToString () : "0";
        companionGemText.text = companionSlotUpgradeIndex != -1 ? companionSlots[companionSlotUpgradeIndex].ToString () : "0";

        for (int i = 0; i < weaponSlots.Count (); i++) {
            if (i <= PlayerDataState.maxWeaponSlots - 1) {
                weaponSlotsUpgrade[i].color = new Color (1, 0, 0.32f);
            } else {
                weaponSlotsUpgrade[i].color = new Color (0, 0.913f, 1, 1);
            }
        }

        for (int i = 0; i < companionSlots.Count (); i++) {
            if (i <= PlayerDataState.maxCompanionSlots - 1) {
                companionSlotsUpgrade[i].color = new Color (1, 1, 1, 1);
            } else {
                companionSlotsUpgrade[i].color = new Color (0, 0, 0, 1);
            }
        }

        upgradeLife.GetComponent<RawImage> ().color = lifeUpgradeSlotIndex != -1 && PlayerDataState.gems >= lifeUpgrades[lifeUpgradeSlotIndex].gems ? new Color (enabledColor.r, enabledColor.g, enabledColor.b) : new Color (disabledColor.r, disabledColor.g, disabledColor.b);
        upgradeLife.GetComponentInChildren<Text> ().text = lifeUpgradeSlotIndex != -1 ? "UPGRADE" : "MAX";

        upgradeMana.GetComponent<RawImage> ().color = manaUpgradeSlotIndex != -1 && PlayerDataState.gems >= manaUpgrades[manaUpgradeSlotIndex].gems ? new Color (enabledColor.r, enabledColor.g, enabledColor.b) : new Color (disabledColor.r, disabledColor.g, disabledColor.b);
        upgradeMana.GetComponentInChildren<Text> ().text = manaUpgradeSlotIndex != -1 ? "UPGRADE" : "MAX";

        upgradeWeapon.GetComponent<RawImage> ().color = weaponSlotUpgrdadeIndex != -1 && PlayerDataState.gems >= weaponSlots[weaponSlotUpgrdadeIndex] ? new Color (enabledColor.r, enabledColor.g, enabledColor.b) : new Color (disabledColor.r, disabledColor.g, disabledColor.b);
        upgradeWeapon.GetComponentInChildren<Text> ().text = weaponSlotUpgrdadeIndex != -1 ? "UPGRADE" : "MAX";

        upgradeCompanion.GetComponent<RawImage> ().color = companionSlotUpgradeIndex != -1 && PlayerDataState.gems >= companionSlots[companionSlotUpgradeIndex] ? new Color (enabledColor.r, enabledColor.g, enabledColor.b) : new Color (disabledColor.r, disabledColor.g, disabledColor.b);
        upgradeCompanion.GetComponentInChildren<Text> ().text = companionSlotUpgradeIndex != -1 ? "UPGRADE" : "MAX";
    }
    private void GetUpgradeIndex () {
        lifeUpgradeSlotIndex = lifeUpgrades.FindIndex (lifeUpgrade => PlayerDataState.life < lifeUpgrade.life);
        manaUpgradeSlotIndex = manaUpgrades.FindIndex (manaUpgrade => PlayerDataState.mana < manaUpgrade.mana);
        weaponSlotUpgrdadeIndex = PlayerDataState.maxWeaponSlots > weaponSlots.Count () - 1 ? -1 : PlayerDataState.maxWeaponSlots;
        companionSlotUpgradeIndex = PlayerDataState.maxCompanionSlots > companionSlots.Count () - 1 ? -1 : PlayerDataState.maxCompanionSlots;
    }
}