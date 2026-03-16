using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private GameObject buyMessage;
    private bool inBuyZone;

    [SerializeField]
    private bool isHealthRestore, isHealthUpgrade, isWeapon;
    [SerializeField]
    private int itemCost;
    [SerializeField]
    private int healthUpgradeAmount;

    [SerializeField]
    private Gun[] potentialGuns;
    [SerializeField]
    private SpriteRenderer gunSprite;
    [SerializeField]
    private TextMeshProUGUI infoText;
    private Gun theGun;


    void Start()
    {
        if (isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];

            gunSprite.sprite = theGun.gunShopSprite;
            infoText.text = theGun.weaponName + "\n - " + theGun.itemCost + " Gold - ";
            itemCost = theGun.itemCost;
        }
    }

    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (LevelManager.instance.currentCoins >= itemCost)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }
                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                    }
                    if (isWeapon)
                    {
                        Gun gunClone = Instantiate(theGun);
                        gunClone.transform.parent = PlayerController.instance.gunArm;
                        gunClone.transform.position = PlayerController.instance.gunArm.position;
                        gunClone.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        gunClone.transform.localScale = new Vector3(1, 1, 1);

                        PlayerController.instance.availableGuns.Add(gunClone);
                        PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                        PlayerController.instance.SwitchGun();
                    }

                    gameObject.SetActive(false);
                    inBuyZone = false;

                    AudioManager.instance.PlaySFX(17);
                }
                else
                {
                    AudioManager.instance.PlaySFX(18);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            buyMessage.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            buyMessage.SetActive(false);

            inBuyZone = false;
        }
    }
}
