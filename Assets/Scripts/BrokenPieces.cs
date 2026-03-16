using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float deceleration = 5f; 
    [SerializeField]
    private float lifetime = 3f;
    private Vector3 moveDirection;

    [SerializeField]
    private SpriteRenderer theSR;
    [SerializeField]
    private float fadeDuration = 2.5f;


    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();

        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }


    void Update()
    {
        transform.position += moveDirection * Time.deltaTime;
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.Lerp(theSR.color.a, 0f, fadeDuration * Time.deltaTime));

            if (theSR.color.a <= 0.01f)
                Destroy(gameObject);
        }
    }
}
