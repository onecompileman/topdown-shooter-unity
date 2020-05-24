using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationUIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject roomClearedNotification;

    [SerializeField]
    public AudioClip roomClearedSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ShowNotification(string notificationType)
    {
        switch (notificationType)
        {
            case "RoomCleared":
                StartCoroutine(InstantiateNotification(roomClearedNotification, roomClearedSound));
                break;
        }
    }

    private IEnumerator InstantiateNotification(GameObject prefabNotification, AudioClip notificationSound)
    {

        yield return new WaitForSeconds(1);
        var notification = Instantiate(roomClearedNotification, transform.position, Quaternion.identity);
        notification.transform.SetParent(transform);

        audioSource.clip = notificationSound;
        audioSource.Play();
    }
}
