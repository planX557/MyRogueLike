using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletToFire;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float timeBetweenShots = 0.2f;
    private float shotCounter;

    public string weaponName;
    public Sprite gunUI;

    public int itemCost;
    public Sprite gunShopSprite;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if (shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }

            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    AudioManager.instance.PlaySFX(11);
                    shotCounter = timeBetweenShots;
                }
                //if (Input.GetMouseButton(0))
                //{
                //    shotCounter -= Time.deltaTime;
                //    if (shotCounter <= 0)
                //    {
                //        shotCounter = timeBetweenShots;
                //        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                //        AudioManager.instance.PlaySFX(11);
                //    }
                //}
            }
        }
    }
}
