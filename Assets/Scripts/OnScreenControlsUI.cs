using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenControlsUI : MonoBehaviour
{

    [SerializeField]
    public PauseUIController pause;

    public void Pause()
    {
        pause.gameObject.SetActive(true);
        pause.PlayOpenAnimation();
        gameObject.SetActive(false);
    }
}
