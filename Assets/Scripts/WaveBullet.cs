using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBullet : MonoBehaviour
{
    [SerializeField]
    public Vector3 velocity;

    [SerializeField]
    public float speed;

    [SerializeField]
    public float damage;

    [SerializeField]
    public float maxDistance;

    private List<int> collidedObjectIds = new List<int>();

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(velocity.z, velocity.x) * Mathf.Rad2Deg + 90, 0);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + (velocity * speed), Time.deltaTime);
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Enemy" && !collidedObjectIds.Contains(obj.gameObject.GetInstanceID()))
        {
            collidedObjectIds.Add(obj.gameObject.GetInstanceID());

            var enemyScript = obj.gameObject.GetComponent<EnemyController>();

            enemyScript.life -= damage;
            enemyScript.TakeDamageEffect();
        }
    }
}
