using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField]
    private float waitForAnyKey = 2f;
    [SerializeField]
    private GameObject anyKeyText;
    [SerializeField]
    private string mainMenuScene;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        Destroy(PlayerController.instance.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
