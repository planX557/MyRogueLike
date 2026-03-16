using UnityEngine;

public class GunChest : MonoBehaviour
{
    [SerializeField]
    private GunPickup[] potentialGuns;

    [SerializeField]
    private SpriteRenderer theSR;
    [SerializeField]
    private Sprite openChest;

    [SerializeField]
    private GameObject notification;

    private bool canOpen, isOpen;
    [SerializeField]
    private Transform spawnPoint;
    private float scaleSpeed = 2f;    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            isOpen = true;
            theSR.sprite = openChest;
            int gunToSpawn = Random.Range(0, potentialGuns.Length);
            Instantiate(potentialGuns[gunToSpawn], spawnPoint.position, spawnPoint.rotation);
            AudioManager.instance.PlaySFX(6);

            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            notification.SetActive(true);

            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            notification.SetActive(false);

            canOpen = false;
        }
    }
}
