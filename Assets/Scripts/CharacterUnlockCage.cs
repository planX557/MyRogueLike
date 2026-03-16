using UnityEngine;

public class CharacterUnlockCage : MonoBehaviour
{
    private bool canLock;
    [SerializeField]
    private GameObject message;

    public CharacterSelector[] charSelects;
    private CharacterSelector playerToUnlock;

    public SpriteRenderer cageSprite;


    // Start is called before the first frame update
    void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];

        cageSprite.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (canLock)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);

                Instantiate(playerToUnlock, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canLock = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canLock = false;
            message.SetActive(false);
        }
    }

    public bool GetUnlock()
    {
        return canLock;
    }
}
