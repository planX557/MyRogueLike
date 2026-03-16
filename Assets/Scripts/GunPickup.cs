using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;
    [SerializeField]
    private float waitToBeCollected = 0.5f;


    private void Update()
    {
        if (waitToBeCollected > 0)
            waitToBeCollected -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && waitToBeCollected <= 0)
        {
            bool hasGun = false;
            foreach (Gun gunToCheck in PlayerController.instance.availableGuns)
            {
                if (gunToCheck.weaponName == theGun.weaponName)
                    hasGun = true;
            }

            if (!hasGun)
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

            AudioManager.instance.PlaySFX(7);
            Destroy(gameObject);
        }
    }
}
