using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField]
    public AboutUIController aboutUI;

    [SerializeField]
    public GameObject loading;

    void Start()
    {
    }

    public void Play()
    {
        loading.SetActive(true);
        loading.GetComponent<AboutUIController>().PlayOpenAnimation();

        SceneManager.LoadScene("Lobby");
    }

    public void OpenAboutUI()
    {
        StartCoroutine("OpenAbout");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator OpenAbout()
    {
        aboutUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        aboutUI.PlayOpenAnimation();
    }
}
