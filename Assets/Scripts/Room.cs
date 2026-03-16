using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    //public bool openWhenEnemiesCleared { get; private set; }
    public GameObject[] doors;
    [SerializeField]
    //private List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public bool roomActive;

    public GameObject mapHider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CameraController.instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                StartCoroutine(CloseDoorsAfterDelay());
            }

            roomActive = true;

            mapHider.SetActive(false);
        }
    }

    private IEnumerator CloseDoorsAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            roomActive = false;
        }
    }
}
