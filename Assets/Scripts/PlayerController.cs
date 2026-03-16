using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 moveInput;

    public Transform gunArm;

    private Rigidbody2D rb;
    //private Camera theCam;
    private Animator anim;
    public SpriteRenderer bodySR;

    //[SerializeField]
    //private GameObject bulletToFire;
    //[SerializeField]
    //private Transform firePoint;
    //[SerializeField]
    //private float timeBetweenShots = 0.2f;
    //private float shotCounter;

    [SerializeField]
    private float activeMoveSpeed;
    [SerializeField]
    private float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvinciblity = 0.5f;
    public float dashCounter;
    private float dashCoolCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果实例已存在且不是当前对象，销毁当前对象
            if (instance != this)
            {
                Debug.LogWarning("检测到重复的PlayerController实例，正在销毁新实例");
                Destroy(gameObject);
                return;
            }
        }


        gameObject.SetActive(true);

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            rb.velocity = moveInput.normalized * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                gunArm.localScale = new Vector3(-1, -1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
            //    AudioManager.instance.PlaySFX(11);
            //    shotCounter = timeBetweenShots;
            //}
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
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (availableGuns.Count > 0)
                {
                    currentGun++;

                    if (currentGun >= availableGuns.Count)
                        currentGun = 0;

                    SwitchGun();
                }
                else
                {
                    Debug.LogWarning("Player has no guns!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                anim.SetTrigger("dash");

                AudioManager.instance.PlaySFX(8);

                PlayerHealthController.instance.MakeInvencible(dashInvinciblity);
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    public void SwitchGun()
    {
        foreach (Gun gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }

        availableGuns[currentGun].gameObject.SetActive(true);

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
