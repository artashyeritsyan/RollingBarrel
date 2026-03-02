using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;




public class MenuController : MonoBehaviour
{
    public static event Action<int> ChangeAlcoholLevel;
    public static event Action OnGameReset;
    public static event Action<int> OverDrunked; // TODO: handle this function in PlayerController, off the controls on 5 seconds

    //[SerializeField] Transform Enemy;

    [Header("Game Areas")]
    [SerializeField] GameObject gameArea;

    [Header("Menu Panels")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameOverlayPanel;
    [SerializeField] GameObject gameWinPanel;

    [Header("Game Overlay Elements")]
    [SerializeField] TextMeshProUGUI beerCountText;
    [SerializeField] Image drunkedLine;
    [SerializeField] Image timeLine;
    [SerializeField] GameObject overdrunkText;


    [Header("Parameters")]
    [SerializeField] int gameTime = 180;
    [SerializeField] int neededBeerCount = 50;
    //[SerializeField] int overdrunkCooldown = 5;
    [SerializeField] int startingBeerFilledCount = 40;
    [SerializeField] int maxBeerLimit = 100;
    [SerializeField] int beerImpact = 10;
    [SerializeField] int snackImpact = 10;
    private float timeRemaining;
    private float beerCount = 0;
    private float beerFilledValue = 0;

    GameObject[] beers;
    GameObject[] snacks;
    GameObject[] walls;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beerCount = 0;
        timeRemaining = gameTime;
        beerFilledValue = startingBeerFilledCount;

        menuPanel.SetActive(true);
        gameOverlayPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);

        overdrunkText.SetActive(false);

        beers = GameObject.FindGameObjectsWithTag("Beer");
        snacks = GameObject.FindGameObjectsWithTag("Snack");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        gameArea.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            GameOver();
        }

        DrunkBarValueManager();
        UpdateTimerUI();
    }    

    void UpdateTimerUI()
    {
        timeLine.fillAmount = timeRemaining / gameTime;

        float t = timeRemaining / gameTime;
        timeLine.color = Color.Lerp(Color.red, Color.green, t);
        drunkedLine.fillAmount = beerFilledValue / maxBeerLimit;
        beerCountText.text = beerCount + "/" + neededBeerCount;
    }

    public float GetBeerFilledValue()
    {
        return beerFilledValue;
    }

    public void reduceBeerFilledValue(float value)
    {
        if (beerFilledValue >= value)
        {
            beerFilledValue -= value;
        }
    }

    void DrunkBarValueManager()
    {
        if (beerFilledValue > 0)
        {
            beerFilledValue -= Time.deltaTime / 2;
        }
        else
        {
            beerFilledValue = 0;
        }

        switch (beerFilledValue)
        {
            case 0:
                ChangeAlcoholLevel?.Invoke(0);
                break;
            case < 10:
                ChangeAlcoholLevel?.Invoke(1);
                break;
            case < 20:
                ChangeAlcoholLevel?.Invoke(2);
                break;
            case < 30:
                ChangeAlcoholLevel?.Invoke(3);
                break;
            case < 40:
                ChangeAlcoholLevel?.Invoke(4);
                break;
            case < 50:
                ChangeAlcoholLevel?.Invoke(5);
                break;

            default:
                ChangeAlcoholLevel?.Invoke(0);
                break;
        }
    }


    public void GameStart()
    {
        beerCount = 0;
        timeRemaining = gameTime;
        beerFilledValue = startingBeerFilledCount;

        menuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverlayPanel.SetActive(true);

        gameArea.SetActive(true);

        OnGameReset?.Invoke();
        ChangeAlcoholLevel?.Invoke(0);
        TurnOnAllItems();

        foreach (var wall in walls)
        {
            if (wall == null) continue;

            var spawner = wall.GetComponent<WallDecorationSpawner>();
            if (spawner == null) continue;


            spawner.GenerateWallDecorations();
            spawner.GenerateGroundDecorations();

        }

    }

    public void Menu()
    {
        // TODO: Change later
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        menuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverlayPanel.SetActive(false);

        gameArea.SetActive(false);
    }

    void GameOver()
    {
        gameOverlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameArea.SetActive(false);
    }

    void GameWin()
    {
        gameOverlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void Restart()
    {
        // TODO: Change later
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        beerCount = 0;
        timeRemaining = gameTime;
        beerFilledValue = startingBeerFilledCount;


        gameOverlayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameArea.SetActive(true);

        OnGameReset?.Invoke();
        ChangeAlcoholLevel?.Invoke(0);
        TurnOnAllItems();
        /// TODO: Call restart event for all
        /// 
        foreach (var wall in walls)
        {
            if (wall == null) continue;

            var spawner = wall.GetComponent<WallDecorationSpawner>();
            if (spawner == null) continue;


            spawner.GenerateWallDecorations();
            spawner.GenerateGroundDecorations();

        }

        

    }

    void TurnOnAllItems()
    {
        foreach (var beer in beers)
        {
            beer.SetActive(true);
        }
        foreach (var snack in snacks)
        {
            snack.SetActive(true);
        }
    }

    void OnEnable()
    {
        PlayerController.OnItemCollect += HandleItemCollection;
        PlayerController.OnPlayerKilled += PlayerKilled;
    }

    void OnDisable()
    {
        PlayerController.OnItemCollect -= HandleItemCollection;
    }

    void PlayerKilled()
    {
        GameOver();
    }

    void HandleItemCollection(bool isBeer)
    {
        if (isBeer)
        {
            beerCount++;
            if (beerCount >= neededBeerCount)
            {
                GameWin();
            }

            beerFilledValue += beerImpact;
            if (beerFilledValue > maxBeerLimit)
            {
                beerFilledValue = maxBeerLimit;
                //OverDrunked?.Invoke(overdrunkCooldown);
                //StartCoroutine(OverDrunkHandler(overdrunkCooldown));
            }
        }
        else
        {
            beerFilledValue -= snackImpact;
        }
    }

    IEnumerator OverDrunkHandler(int duration) {
        overdrunkText.SetActive(true);
        yield return new WaitForSeconds(duration);

        overdrunkText.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
