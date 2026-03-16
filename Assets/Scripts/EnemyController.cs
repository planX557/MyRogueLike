using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Variables")]
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float health = 100f;

    [Header("Chase/Run Away Details")]
    [SerializeField]
    private bool shouldChasePlayer;
    [SerializeField]
    private float rangeTochasePlayer = 5f;
    [SerializeField]
    private bool shouldRunAway;
    [SerializeField]
    private float rangeToRunAway;

    [Header("Wander Details")]
    [SerializeField]
    private bool shouldWander;
    [SerializeField]
    private float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrol Details")]
    [SerializeField]
    private bool shouldPatrol;
    [SerializeField]
    private Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("Effects")]
    [SerializeField]
    private GameObject[] deathSplatter;
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private bool shouleDropItem;
    [SerializeField]
    private GameObject[] itemToDrop;
    [SerializeField]
    private float itemDropChance = 25f;

    [Header("Attack Details")]
    [SerializeField]
    private bool shouldShoot = true;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float fireRate = 3f;
    [SerializeField]
    private float fireCounter;
    [SerializeField]
    private float shootRange = 5f;
    [SerializeField]
    private SpriteRenderer theBody;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeTochasePlayer && shouldChasePlayer)
            {
                moveDirection = (PlayerController.instance.transform.position - transform.position).normalized;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;
                        moveDirection = wanderDirection;
                    }
                    else
                    {
                        pauseCounter -= Time.deltaTime;
                        if (pauseCounter <= 0)
                        {
                            wanderCounter = wanderLength;
                            pauseCounter = pauseLength;
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                        }
                    }
                }

                if (shouldPatrol)
                {
                    moveDirection = (patrolPoints[currentPatrolPoint].position - transform.position).normalized;

                    if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.1f)
                    {
                        currentPatrolPoint++;
                        if (currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToRunAway && shouldRunAway)
            {
                moveDirection = (transform.position - PlayerController.instance.transform.position).normalized;
            }
            //else
            //{
            //    moveDirection = Vector3.zero;
            //}

            rb.velocity = moveDirection * moveSpeed;


            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(12);
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            wanderCounter = 0;
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySFX(2);
        Instantiate(hitEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(1);

        int selectedSplatter = Random.Range(0, deathSplatter.Length);
        int rotation = Random.Range(0, 4);
        Instantiate(deathSplatter[selectedSplatter], transform.position, Quaternion.Euler(0, 0, rotation * 90));

        if (shouleDropItem)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance <= itemDropChance)
            {
                int randomItem = Random.Range(0, itemToDrop.Length);
                Instantiate(itemToDrop[randomItem], transform.position, transform.rotation);
            }
        }
        //anim.SetTrigger("die");
        //rb.velocity = Vector2.zero;
        //GetComponent<Collider2D>().enabled = false;
        //this.enabled = false;
    }
}
