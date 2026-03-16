using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Health UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Coin UI")]
    public TextMeshProUGUI coinText;

    [Header("Screen UI")]
    public GameObject deathScreen;
    public Image fadeScreen;
    [SerializeField]
    private float fadeSpeed = 2f;
    private bool fadeToBlack;
    private bool fadeFromBlack;
    [SerializeField]
    private string newGameScene, mainMenuScene;
    public GameObject pauseMenu, mapDisplay, bigMapText;

    [Header("Gun UI")]
    public Image currentGun;
    public TextMeshProUGUI gunText;

    public Slider bossHealthBar;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fadeFromBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;
        gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
                fadeFromBlack = false;
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
                fadeToBlack = false;
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeFromBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);

        PlayerController.instance.gameObject.SetActive(true);
        //Destroy(PlayerController.instance.gameObject);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);

        Destroy(PlayerController.instance.gameObject);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnPause();
    }
}
