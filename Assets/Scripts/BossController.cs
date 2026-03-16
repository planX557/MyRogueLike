using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    public BossAction[] actions;
    private int currentAction;
    private float actionCounter;

    private float shootCounter;
    private Vector2 moveDirection;
    [SerializeField]
    private Rigidbody2D theRB;

    [SerializeField]
    private int currentHealth;

    public GameObject deathEffect, hitEffect;
    [SerializeField]
    private GameObject levelExit;

    public BossSequence[] sequences;
    [SerializeField]
    private int currentSequence;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        actions = sequences[currentSequence].actions;

        actionCounter = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    void Update()
    {
        if (actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;

            moveDirection = Vector2.zero;

            if (actions[currentAction].shouldMove)
            {
                if (actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = (PlayerController.instance.transform.position - transform.position).normalized;
                }
                else if (actions[currentAction].moveToPoint && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > 0.5f)
                {
                    moveDirection = (actions[currentAction].pointToMoveTo.position - transform.position).normalized;
                }
            }

            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;

            if (actions[currentAction].shouldShoot)
            {
                shootCounter -= Time.deltaTime;

                if (shootCounter <= 0)
                {
                    shootCounter = actions[currentAction].timeBetweenShots;

                    foreach (Transform shootPoint in actions[currentAction].shootPoints)
                    {
                        Instantiate(actions[currentAction].itemToShoot, shootPoint.position, shootPoint.rotation);
                    }
                }
            }
        }
        else
        {
            currentAction++;

            if (currentAction >= actions.Length)
            {
                currentAction = 0;
            }
            actionCounter = actions[currentAction].actionLength;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathEffect, transform.position, transform.rotation);

            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        else
        {
            if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public float moveSpeed;
    public bool moveToPoint;
    public Transform pointToMoveTo;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shootPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequences")]
    public BossAction[] actions;

    public int endSequenceHealth;
}
