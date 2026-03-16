using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 7.5f;
    [SerializeField]
    private int damage = 25;
    [SerializeField]
    private GameObject impactEffect;
    private Rigidbody2D rb;

    [SerializeField] private LayerMask playerLayer;      
    [SerializeField] private LayerMask enemyLayer;      
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int otherLayer = other.gameObject.layer;

        if (otherLayer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player hit by own bullet, ignoring.");
            return;
        }

        if (impactEffect != null)
            Instantiate(impactEffect, transform.position, transform.rotation);

        if (otherLayer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<EnemyController>()?.DamageEnemy(damage);
        }

        if (other.CompareTag("Boss"))
        {
            BossController.instance.TakeDamage(damage);

            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
