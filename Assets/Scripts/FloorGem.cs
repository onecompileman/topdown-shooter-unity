using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGem : MonoBehaviour
{
    [SerializeField]
    public float life;

    [SerializeField]
    public ParticleSystem deathEffect;

    [SerializeField]
    public MeshRenderer meshRenderer;

    public delegate void OnDeathDelegate();

    public event OnDeathDelegate onDeathDelegate;

    private Material material;

    private Color originalColor;

    void Start()
    {
        material = meshRenderer.materials[0];
        originalColor = new Color(material.color.r, material.color.g, material.color.b);
    }


    void Update()
    {
        if (life <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);

            onDeathDelegate();

            Destroy(gameObject);
        }
    }



    public IEnumerator TakeDamage()
    {
        material.color = new Color(155, 155, 155);
        yield return new WaitForSeconds(0.2f);
        material.color = originalColor;
    }
}
