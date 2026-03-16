using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private Vector3 direction;


    void Start()
    {
        //direction = (PlayerController.instance.transform.position - transform.position).normalized;
        direction = transform.right; // Assuming the bullet is facing right when instantiated
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (!BossController.instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (otherLayer == LayerMask.NameToLayer("Enemy"))
        {
            return;
        }

        //if (impactEffect != null)
        //    Instantiate(impactEffect, transform.position, transform.rotation);

        if (otherLayer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealthController.instance.DamagePlayer(damage);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
