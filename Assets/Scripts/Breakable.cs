using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField]
    private GameObject[] brokenPieces;
    [SerializeField]
    private int maxPieces = 3;

    [SerializeField]
    private bool shouleDropItem;
    [SerializeField]
    private GameObject[] itemToDrop;
    [SerializeField]
    private float itemDropChance = 25f;
    [SerializeField]
    private int breakSound = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask otherLayer = other.gameObject.layer;
        if (otherLayer == LayerMask.NameToLayer("Player"))
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
        }
        if (otherLayer == LayerMask.NameToLayer("PlayerBullets"))
        {
            Smash();
        }
    }

    private void Smash()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(breakSound);

        int piecesToSpawn = Random.Range(1, maxPieces + 1);

        for (int i = 0; i < piecesToSpawn; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);
            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        if (shouleDropItem)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance <= itemDropChance)
            {
                int randomItem = Random.Range(0, itemToDrop.Length);
                Instantiate(itemToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
