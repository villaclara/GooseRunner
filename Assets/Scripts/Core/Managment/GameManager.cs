
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _walls;
    [SerializeField] private GameObject[] _ladders;
    [SerializeField] private GameObject[] _verticalWalls;
    [SerializeField] private GameObject[] _brickWalls;
    [SerializeField] private GameObject[] _BGWalls;
    [SerializeField] private GameObject[] _wallsBehindLadder;
    [SerializeField] private GameObject[] _FrontWalls;
    public Transform characterTransform;
    public GameObject SpeedUpOrb;
    public GameObject SlowDownOrb;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;
    public SpriteRenderer bgRenderer;
    public GameObject BackGround;
    private float _maxWidth, _minWidth;
    public SpriteRenderer wallSpriteRenderer;
    public SpriteRenderer ladderRenderer;
    public SpriteRenderer verticalWallRenderer;
    public SpriteRenderer wallBehindLadderRenderer;
    private SpriteRenderer _objRenderer;
    public BoxCollider2D wallBoxCollider2D;
    public BoxCollider2D wallBehindLadderCollider2D;
    private float _wallScale, _ladderScaleX, _verticalWallScale;//wall h64px w256px,  ladder h300px w100px 16x48
    private Vector2 _ladderPosition;
    //private float _fallSpeed = GlobalVariables.fallSpeed;
    private float lastVerticalWallPosX = -100f;
    private float lastLadderPosX1 = -100f, lastLadderPosX2 = -100f;
    public GameObject gem;
    public static int gemSpawnCooldown = 3;
    public static int orbSpawnCooldown = 10;
    private static int _currentGemSpawnCooldown = gemSpawnCooldown;
    private static int _currentOrbSpawnCooldown = orbSpawnCooldown;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gemScoreText;
    public TextMeshProUGUI TapToStartText;
    private Queue<GameObject> _rightWalls;
    private float _unitsPerPixel;
    public GameObject gameOverUI;
    public GameObject boulder;
    private float lastX = 0;//prevents boulders spawn on the same position
    public RectTransform AttentionSign;
    private Vector2 _spawnPosL;
    private Vector2 _spawnPosR;
    public static bool gameStarted;
    public MainCharacterMovement mainCharacterMovement;
    public AttentionSign attentionSign;
    public SceneController sceneController;
    public GameObject pauseButton;
    public GameObject restartButton;
    public GameObject bgGrass, bgLeftHouse, bgRightHouse;
    public RectTransform pausePannelRectTransform;
    public TextMeshProUGUI gameOverScreenScore;
    public TextMeshProUGUI gameOverScreenGemsCollected;
    public static int gemsCollected;
    private bool onGameOverScreen;
    public GameObject cloud;
    public Sprite cloud1, cloud2, cloud3, cloud4, cloud5, cloud6, cloud0;
    public GameObject lockerButton;
    private float fps;
    public TextMeshProUGUI FPSCounter;
    public float ratioH, ratioW;
    private float _screenWidthInUnits;
    public CanvasGroup canvasGroup;
    public GameObject enemy;
    public CanvasGroup speedUpSides, slowDownSides;
    private int _enemySpawnCooldown = 7;

    // Start is called before the first frame update

    /// <summary>
    /// IF YOU TAKE SIZE FROM SPRITE RENDERER THEN MULTYPLY IT BY OBJECTS SCALE
    /// IF YOU WANT TO CHANGE SIZE OF SPRITE RENDERER DIVIDE IT BE SCALE
    /// </summary>
    void Start()
    {
       
        StartCoroutine(sceneController.FadeOutScreen());
        

        InvokeRepeating("GetFPS", 0.5f, 0.5f);



        TapToStartText.gameObject.SetActive(true);
        

        gameStarted = false;
        GlobalVariables.fallSpeed = 0f;
        GlobalVariables.gameIsRunning = true;


        _unitsPerPixel = 2 * Camera.main.orthographicSize / Screen.height;

        ///////////////////////////////
        ///temporary
        Debug.Log(Screen.height);//2532
        Debug.Log(Screen.width);//1170
        Debug.Log(Camera.main.orthographicSize * 2 * Screen.width / Screen.height);
        _screenWidthInUnits = Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        _wallScale = _walls[0].transform.localScale.x;
        _ladderScaleX = _ladders[0].transform.localScale.x;
        _verticalWallScale = _verticalWalls[0].transform.localScale.y;


        Resize(_ladders);
        Resize(_wallsBehindLadder);

        spawnPointLeft.position = new Vector2(-1 * _unitsPerPixel * Screen.width / 2, Screen.height * _unitsPerPixel / 2 + 1.5f);
        spawnPointRight.position = new Vector2(_unitsPerPixel * Screen.width / 2, Screen.height * _unitsPerPixel / 2 + 1.5f);
        _spawnPosL = spawnPointLeft.transform.position;
        _spawnPosR = spawnPointRight.transform.position;
        float fullScreenWidth = Screen.width * _unitsPerPixel;
        //Debug.Log($"full screen width{fullScreenWidth}");
        float ladderWidth = ladderRenderer.size.x * _ladderScaleX;
        //Debug.Log($"ladder renderer size{ladderRenderer.size.x}");
        //Debug.Log($"ladder width {ladderWidth}");   
        _maxWidth = (fullScreenWidth - 1.5f * ladderWidth) * 2f;
        //Debug.Log($"max width{_maxWidth}"); 
        _minWidth = (fullScreenWidth - _maxWidth / 2f - ladderWidth) * 2f;
        //Debug.Log($"min width{_minWidth}");

        Vector2 bgSize = bgRenderer.size;//bg - backgorund also must be sized depending on screen size
        bgSize.x = fullScreenWidth * 1.1f;
        bgSize.y = Screen.height * _unitsPerPixel * 4f;
        bgRenderer.size = bgSize;
        BackGround.transform.position = new Vector2(0, bgSize.y / 2 - (Screen.height * _unitsPerPixel) / 2);

        SpriteRenderer bgRightHouseR = bgRightHouse.GetComponent<SpriteRenderer>();
        bgRightHouseR.size = new Vector2(fullScreenWidth * 0.4f, (Screen.height * _unitsPerPixel) * 1.3f);

        SpriteRenderer bgLeftHouseR = bgLeftHouse.GetComponent<SpriteRenderer>();
        bgLeftHouseR.size = new Vector2(fullScreenWidth * 0.3f, (Screen.height * _unitsPerPixel) * 1.1f);

        SpriteRenderer bgGrassR = bgGrass.GetComponent<SpriteRenderer>();
        bgGrassR.size = new Vector2(fullScreenWidth * 1.1f, (Screen.height * _unitsPerPixel) * 0.2f);





        Vector2 pausePannelSize = pausePannelRectTransform.sizeDelta;
        pausePannelSize.x = Screen.width;
        pausePannelRectTransform.sizeDelta = pausePannelSize;


        _rightWalls = new Queue<GameObject>();//stores references to right wall objects, use them to know when the layer is destroyed and need to spawn new

        int i = 0;
        GlobalVariables.ladderHeight = ladderRenderer.size.y * _ladders[0].transform.localScale.y;
        for (float currentY = -Screen.height * _unitsPerPixel/ 2; 
            currentY <= Screen.height * _unitsPerPixel/ 2 + GlobalVariables.ladderHeight; 
            currentY+= GlobalVariables.ladderHeight)//make starting layers
        {
            if (i % 3 == 0)
            {
                SpawnThreeWalls(currentY);
            }
            else
            {
                SpawnTwoWalls(currentY);
                lastLadderPosX2 = -100;
                lastVerticalWallPosX = -100;
            }
            i++;
            _currentOrbSpawnCooldown--;
            //Debug.Log($"current = {currentY}, {Screen.height * _unitsPerPixel / 2 + GlobalVariables.ladderHeight}");
            //Debug.Log(currentY + GlobalVariables.ladderHeight<=Screen.height* _unitsPerPixel / 2 + GlobalVariables.ladderHeight);
        }

		highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        gemScoreText.text = PlayerPrefs.GetInt("GemScore", 0).ToString();
        GlobalVariables.gems = PlayerPrefs.GetInt("GemScore", 0);
        gemsCollected = 0;


        onGameOverScreen = false;
    }
    public void SpawnBoulder()
    {
        StartCoroutine(SpawnBoulderCoroutine());
    }
    private IEnumerator SpawnBoulderCoroutine()
    {
        Vector2 boulderSpawnPos = new Vector2();
        float leftEdge = spawnPointLeft.transform.position.x + 0.3f;
        boulderSpawnPos.y = 8f;
        float randomX = Random.Range(leftEdge, -leftEdge);
        while (Mathf.Abs(randomX - lastX) < 1.5f)
        {
            randomX = Random.Range(leftEdge, -leftEdge);
        }
        lastX = randomX;
        boulderSpawnPos.x = randomX;

        Vector2 attentionSignPos = AttentionSign.anchoredPosition;
        attentionSignPos.x = boulderSpawnPos.x * 192f;
        Debug.Log(attentionSignPos.x);
        AttentionSign.anchoredPosition = attentionSignPos;

        GlobalVariables.timesSignShowed = 0;
        yield return StartCoroutine(attentionSign.ShowSignCoroutine());
        yield return new WaitUntil(() => !PauseMenu.isPaused);

        Debug.Log("boulder");
        Instantiate(boulder, boulderSpawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        if ((Input.touchCount > 0 || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !gameStarted)
        {
            Touch touch = Input.GetTouch(0);
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                lockerButton.SetActive(false);
                pauseButton.SetActive(true);
                gameStarted = true;
                GlobalVariables.fallSpeed = 0.4f;
                TapToStartText.gameObject.SetActive(false);
                InvokeRepeating("SpawnBoulder", 10f, 10f);
            }
        }

        scoreText.text = GlobalVariables.score.ToString();
        gameOverScreenScore.text = GlobalVariables.score.ToString();
        if (GlobalVariables.score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", GlobalVariables.score);
            highScoreText.text = GlobalVariables.score.ToString();
        }

        gemScoreText.text = GlobalVariables.gems.ToString();
        PlayerPrefs.SetInt("GemScore", GlobalVariables.gems);

        //_fallSpeed = GlobalVariables.fallSpeed;
        if ((_rightWalls.Count > 0) && !_rightWalls.Peek().activeSelf)
        {
            SpawnLayer();
            _rightWalls.Dequeue();
        }

        if (!GlobalVariables.gameIsRunning && !onGameOverScreen)
            GameOver();
    }

    public void SpawnCloud()
    {
        int randSpawn = Random.Range(1, 3);
        int randomCloudType = Random.Range(0, 7);
        float randHeight = Random.Range(Screen.height * _unitsPerPixel / 4f, Screen.height * _unitsPerPixel / 2f);
        Vector2 spawnPos;
        if (randSpawn == 1)
        {
            spawnPos = new Vector2(Screen.width * _unitsPerPixel, randHeight);
            Instantiate(cloud, spawnPos, Quaternion.identity);
        }
        else
        {
            spawnPos = new Vector2(-Screen.width * _unitsPerPixel, randHeight);
            Instantiate(cloud, spawnPos, Quaternion.identity);
        }

        SpriteRenderer spriteR = cloud.GetComponent<SpriteRenderer>();
        Sprite sprite = randomCloudType switch
        {
            0 => cloud0,
            1 => cloud1,
            2 => cloud2,
            3 => cloud3,
            4 => cloud4,
            5 => cloud5,
            6 => cloud6,
            _ => throw new System.NotImplementedException()
        };
        spriteR.sprite = sprite;

    }
    private void SpawnLayer()
    {
        float spawnPos = Screen.height * _unitsPerPixel / 2 + GlobalVariables.ladderHeight;

        int randV = Random.Range(1, 4);
        if (randV % 3 == 0)
        {
            SpawnThreeWalls(spawnPos);
        }
        else
        {
            SpawnTwoWalls(spawnPos);
            lastLadderPosX2 = -100f;
            lastVerticalWallPosX = -100f;
        }
        _currentGemSpawnCooldown--;
        _currentOrbSpawnCooldown--;



        if (GlobalVariables.score % 3 == 0 && GlobalVariables.fallSpeed <= 1.5f)
        {
            GlobalVariables.fallSpeed += 0.04f;//0.04
            Debug.Log($"fallSpeed{GlobalVariables.fallSpeed}");
            MainCharacterMovement.moveSpeed += 0.009f * _screenWidthInUnits / GlobalVariables.commonScreenWidthInUnits;
            Debug.Log($"characte speed{MainCharacterMovement.moveSpeed}");
        }
    }

    private void SpawnTwoWalls(float spawnPosY)
    {

        GameObject wall = _walls[FindUnusedObject(_walls)];
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
        wallBoxCollider2D = wall.GetComponent<BoxCollider2D>();
        Vector2 newSizeR = wallSpriteRenderer.size;
        wall.transform.position = new Vector2(
            _spawnPosR.x,
            spawnPosY
        );

        GameObject ladder = _ladders[FindUnusedObject(_ladders)];
        ladderRenderer = ladder.GetComponent<SpriteRenderer>();
        Vector2 ladderSize = ladderRenderer.size;
        
        int i = 0;
        do {
            if (i > 50)
                break;
            newSizeR.x = Random.Range(_minWidth, _maxWidth) / _wallScale;

            _ladderPosition.x = wall.transform.position.x - newSizeR.x * 0.5f * _wallScale - ladderSize.x * 0.5f * _ladderScaleX;
            i++;
        } while ((Mathf.Abs(_ladderPosition.x - lastLadderPosX1) < ladderSize.x * _ladderScaleX) || (Mathf.Abs(_ladderPosition.x - lastLadderPosX2) < ladderSize.x * _ladderScaleX)
                    || (Mathf.Abs(_ladderPosition.x - lastVerticalWallPosX) < ladderSize.x * _ladderScaleX) 
                    || (Mathf.Abs(_ladderPosition.x - characterTransform.position.x)< ladderSize.x * _ladderScaleX));
        
        wallSpriteRenderer.size = newSizeR;
        wallBoxCollider2D.size = newSizeR;
        wall.SetActive(true);

        _ladderPosition.y = wall.transform.position.y - 0.373f; //prew was 0.38f
        ladder.transform.position = _ladderPosition;
        ladder.SetActive(true);

        //ENEMY
        if (_enemySpawnCooldown == 0)
        {
            Vector2 a = new Vector2(0, ladderRenderer.size.y * 0.8f);
            Instantiate(enemy, _ladderPosition + a, Quaternion.identity);
            _enemySpawnCooldown = 7;
        }

        GameObject wallBehind = _wallsBehindLadder[FindUnusedObject(_wallsBehindLadder)];
        wallBehind.transform.position = new Vector2(_ladderPosition.x, spawnPosY);
        wallBehindLadderCollider2D = wallBehind.GetComponent<BoxCollider2D>();
        Vector2 wallBehindLadderSize = wallBehindLadderRenderer.size;
        wallBehindLadderCollider2D.size = wallBehindLadderSize;
        wallBehind.SetActive(true);

        _rightWalls.Enqueue(wall);
        lastLadderPosX1 = _ladderPosition.x;


        wall = _walls[FindUnusedObject(_walls)];
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
        wallBoxCollider2D = wall.GetComponent<BoxCollider2D>();
        Vector2 newSizeL = wallSpriteRenderer.size;
        newSizeL.x = (((Screen.width * _unitsPerPixel) - newSizeR.x * 0.5f * _wallScale - ladderSize.x * _ladderScaleX) * 2f)/_wallScale;
        //Debug.Log($"{newSizeL.x}, {(Screen.width * _unitsPerPixel)}, {newSizeR.x * 0.5f * _wallScale}, {ladderSize.x * _ladderScaleX}");

        wallSpriteRenderer.size = newSizeL;
        wallBoxCollider2D.size = newSizeL;
        wall.SetActive(true);
        wall.transform.position = new Vector2(
            _spawnPosL.x,
            spawnPosY
        );

        int randomBGSpawn = Random.Range(1, 5);
        if (randomBGSpawn == 1 || randomBGSpawn == 2)
        {

            GameObject brickWall = _brickWalls[FindUnusedObject(_brickWalls)];
            SpriteRenderer brickWallRenderer = brickWall.GetComponent<SpriteRenderer>();
            Vector2 brickWallSize = brickWallRenderer.size;
            brickWallSize.x = Random.Range(0.25f * Screen.width * _unitsPerPixel, 0.75f * Screen.width * _unitsPerPixel);
            brickWallRenderer.size = brickWallSize;

            brickWall.SetActive(true);
            brickWall.transform.position = new Vector2(_spawnPosR.x - brickWallSize.x, newSizeL.y * 0.5f + spawnPosY + brickWallSize.y * 0.5f);
        }
        if(randomBGSpawn == 3 || randomBGSpawn == 4)
        {
            if (randomBGSpawn == 3)
            {
                GameObject BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                float BGWallSpawnPosX = spawnPointLeft.transform.position.x + 0.5f;
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);

                BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                BGWallSpawnPosX = BGWallSpawnPosX + 1f;// one becouse it the width of the BGWall
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);

                BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                BGWallSpawnPosX = BGWallSpawnPosX + 1f;// one becouse it the width of the BGWall
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);
            }
            else
            {
                GameObject BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                float BGWallSpawnPosX = spawnPointRight.transform.position.x - 0.5f;
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);

                BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                BGWallSpawnPosX = BGWallSpawnPosX - 1f;// one becouse it the width of the BGWall
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);

                BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
                BGWallSpawnPosX = BGWallSpawnPosX - 1f;// one becouse it the width of the BGWall
                BGWall.transform.position = new Vector2(BGWallSpawnPosX, spawnPosY + newSizeR.y * 0.5f + 0.5f);
                BGWall.SetActive(true);
            }
        }



        ////
        //randomBGSpawn = Random.Range(1, 15);
        //if(randomBGSpawn == 13 || randomBGSpawn == 14)
        //{
        //    GameObject frontWall = _FrontWalls[FindUnusedObject(_FrontWalls)];
        //    float frontWallSpawnPosX = randomBGSpawn == 13 ? spawnPointRight.transform.position.x - 1.25f: spawnPointLeft.transform.position.x + 1.25f;//1.25 -width/2
        //    float frontWallSpawnPosY = spawnPosY - newSizeR.y * 0.5f * _wallScale + 1.25f * 0.5f; //1.25f * 0.5f height/2
        //    frontWall.transform.position = new Vector2(frontWallSpawnPosX, frontWallSpawnPosY);
        //    frontWall.SetActive(true);
        //}

        if ((_currentOrbSpawnCooldown == 0) && (_currentGemSpawnCooldown != 0))
        {
            SpawnOrb(wall, lastLadderPosX1, lastLadderPosX2, lastVerticalWallPosX);
            _currentOrbSpawnCooldown = orbSpawnCooldown;
        }

        if (_currentOrbSpawnCooldown == 0 && _currentGemSpawnCooldown == 0)
            _currentOrbSpawnCooldown++;

        if (_currentGemSpawnCooldown == 0)
        {
            SpawnGem(wall, lastLadderPosX1, lastLadderPosX2, lastVerticalWallPosX);
            _currentGemSpawnCooldown = gemSpawnCooldown;
        }

        WallBehindLadderScript wallBScript = ladder.GetComponent<WallBehindLadderScript>();
        wallBScript.wall = wallBehind;
        wallBScript.otherWall = null;

        _enemySpawnCooldown--;

    }

    private void SpawnThreeWalls(float spawnPosY)
    {
        GameObject wall = _walls[FindUnusedObject(_walls)];
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
        wallBoxCollider2D = wall.GetComponent<BoxCollider2D>();
        Vector2 newSizeR = wallSpriteRenderer.size;
        wall.transform.position = new Vector2(
            _spawnPosR.x,
            spawnPosY
        );

        GameObject ladder1 = _ladders[FindUnusedObject(_ladders)];
        ladderRenderer = ladder1.GetComponent<SpriteRenderer>();
        Vector2 ladderSize = ladderRenderer.size;

        int i = 0;
        do {
            if (i > 50)
                break;
            newSizeR.x = Random.Range(_minWidth/2, _maxWidth / 2.3f) / _wallScale;

            _ladderPosition.x = wall.transform.position.x - newSizeR.x * _wallScale * 0.5f - ladderSize.x * 0.5f * _ladderScaleX;
            i++;
        } while ((Mathf.Abs(_ladderPosition.x - lastLadderPosX1) < ladderSize.x * _ladderScaleX) || (Mathf.Abs(_ladderPosition.x - lastVerticalWallPosX) < ladderSize.x * _ladderScaleX)
                || (Mathf.Abs(_ladderPosition.x - characterTransform.position.x) < ladderSize.x * _ladderScaleX));

        wallSpriteRenderer.size = newSizeR;
        wallBoxCollider2D.size = newSizeR;
        wall.SetActive(true);
        _rightWalls.Enqueue(wall);

        _ladderPosition.y = wall.transform.position.y - 0.373f;
        ladder1.transform.position = _ladderPosition;
        ladder1.SetActive(true);



        GameObject wallBehind1 = _wallsBehindLadder[FindUnusedObject(_wallsBehindLadder)];
        wallBehind1.transform.position = new Vector2(_ladderPosition.x, spawnPosY);
        wallBehindLadderCollider2D = wallBehind1.GetComponent<BoxCollider2D>();
        Vector2 wallBehindLadderSize = wallBehindLadderRenderer.size;
        wallBehindLadderCollider2D.size = wallBehindLadderSize;
        wallBehind1.SetActive(true);
        ///////////////////////////////////////////////////////////////////////////////////////////



        wall = _walls[FindUnusedObject(_walls)];
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
        wallBoxCollider2D = wall.GetComponent<BoxCollider2D>();
        Vector2 newSizeL = wallSpriteRenderer.size;
        wall.transform.position = new Vector2(
            _spawnPosL.x,
            spawnPosY
        );

        GameObject ladder2 = _ladders[FindUnusedObject(_ladders)];
        ladderRenderer = ladder2.GetComponent<SpriteRenderer>();
        ladderSize = ladderRenderer.size;

        i = 0;
        do {
            if (i > 50)
                break;
            newSizeL.x = Random.Range(_minWidth/2, _maxWidth / 2.3f) / _wallScale;

            _ladderPosition.x = wall.transform.position.x + newSizeL.x * _wallScale * 0.5f + ladderSize.x * 0.5f * _ladderScaleX;
            i++;
        } while((Mathf.Abs(_ladderPosition.x - lastLadderPosX1) < ladderSize.x * _ladderScaleX) || (Mathf.Abs(_ladderPosition.x - lastLadderPosX2) < ladderSize.x * _ladderScaleX)
                    || (Mathf.Abs(_ladderPosition.x - lastVerticalWallPosX) < ladderSize.x * _ladderScaleX) 
                    || (Mathf.Abs(_ladderPosition.x - characterTransform.position.x) < ladderSize.x * _ladderScaleX));

        wallSpriteRenderer.size = newSizeL;
        wallBoxCollider2D.size = newSizeL;
        wall.SetActive(true);

        _ladderPosition.y = wall.transform.position.y - 0.373f;
        ladder2.transform.position = _ladderPosition;
        ladder2.SetActive(true);




        GameObject wallBehind2 = _wallsBehindLadder[FindUnusedObject(_wallsBehindLadder)];
        wallBehind2.transform.position = new Vector2(_ladderPosition.x, spawnPosY);
        wallBehindLadderCollider2D = wallBehind2.GetComponent<BoxCollider2D>();
        wallBehindLadderSize = wallBehindLadderRenderer.size;
        wallBehindLadderCollider2D.size = wallBehindLadderSize;
        wallBehind2.SetActive(true);

        lastLadderPosX2 = _ladderPosition.x;
        _ladderPosition = ladder1.transform.position;
        lastLadderPosX1 = _ladderPosition.x;
        //////////////////////////////////////////////////////////////////////////////

        wall = _walls[FindUnusedObject(_walls)];
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
        wallBoxCollider2D = wall.GetComponent<BoxCollider2D>();
        Vector2 newSizeM = wallSpriteRenderer.size;
        newSizeM.x = ((Screen.width * _unitsPerPixel) - (2f * ladderSize.x * _ladderScaleX) - newSizeL.x * 0.5f * _wallScale - newSizeR.x * 0.5f * _wallScale) / _wallScale;
        float spawnPosMx = _ladderPosition.x - ladderSize.x * 0.5f * _ladderScaleX - newSizeM.x * 0.5f * _wallScale;
        wallSpriteRenderer.size = newSizeM;
        wallBoxCollider2D.size = newSizeM;

        wall.SetActive(true);
        wall.transform.position = new Vector2(
            spawnPosMx,
            spawnPosY
        );
        ///////////////////////////////////////////////////////////////////

        GameObject verticalWall = _verticalWalls[FindUnusedObject(_verticalWalls)];
        verticalWall.SetActive(true);
        Vector2 Vsize = verticalWallRenderer.size;
        float VPosY = newSizeM.y * 0.5f * _verticalWallScale + Vsize.y * 0.5f * _verticalWallScale + spawnPosY;
        verticalWall.transform.position = new Vector2(
            spawnPosMx,
            VPosY
            );
        lastVerticalWallPosX = spawnPosMx;

        /////////////////
        int randomBGSpawn = Random.Range(1, 4);
        if (randomBGSpawn == 1)
        {
            GameObject BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
            float BGWallSpawnPosX = spawnPosMx + _verticalWallScale * Vsize.x * 0.5f;
            BGWall.transform.position = new Vector2(BGWallSpawnPosX, VPosY + 0.1f);
            BGWall.SetActive(true);

            BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
            BGWallSpawnPosX = BGWallSpawnPosX + 1f;// one becouse it the width of the BGWall
            BGWall.transform.position = new Vector2(BGWallSpawnPosX, VPosY + 0.1f);
            BGWall.SetActive(true);
        }
        else if(randomBGSpawn == 2)
        {
            GameObject BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
            float BGWallSpawnPosX = spawnPosMx - _verticalWallScale * Vsize.x * 0.5f - 0.5f;
            BGWall.transform.position = new Vector2(BGWallSpawnPosX, VPosY + 0.1f);
            BGWall.SetActive(true);

            BGWall = _BGWalls[FindUnusedObject(_BGWalls)];
            BGWallSpawnPosX = BGWallSpawnPosX - 1f;// one becouse it the width of the BGWall
            BGWall.transform.position = new Vector2(BGWallSpawnPosX, VPosY + 0.1f);
            BGWall.SetActive(true);
        }
        else if(randomBGSpawn == 3)
        {
            GameObject brickWall = _brickWalls[FindUnusedObject(_brickWalls)];
            SpriteRenderer brickWallRenderer = brickWall.GetComponent<SpriteRenderer>();
            Vector2 brickWallSize = brickWallRenderer.size;
            brickWallSize.x = Random.Range(0.25f * Screen.width * _unitsPerPixel, 0.75f * Screen.width * _unitsPerPixel);
            brickWallRenderer.size = brickWallSize;

            brickWall.SetActive(true);
            brickWall.transform.position = new Vector2(_spawnPosR.x - brickWallSize.x, newSizeL.y * 0.5f + spawnPosY + brickWallSize.y * 0.5f);
        }


        ////
        //randomBGSpawn = Random.Range(1, 15);
        //if (randomBGSpawn == 13 || randomBGSpawn == 14)
        //{
        //    GameObject frontWall = _FrontWalls[FindUnusedObject(_FrontWalls)];
        //    float frontWallSpawnPosX = randomBGSpawn == 13 ? spawnPointRight.transform.position.x - 1.25f : spawnPointLeft.transform.position.x + 1.25f;//1.25 -width/2
        //    float frontWallSpawnPosY = spawnPosY - newSizeR.y * 0.5f * _wallScale + 1.25f * 0.5f; //1.25f * 0.5f height/2
        //    frontWall.transform.position = new Vector2(frontWallSpawnPosX, frontWallSpawnPosY);
        //    frontWall.SetActive(true);
        //}

        if ((_currentOrbSpawnCooldown == 0) && (_currentGemSpawnCooldown != 0))
        {
            SpawnOrb(wall, lastLadderPosX1, lastLadderPosX2, lastVerticalWallPosX);
            _currentOrbSpawnCooldown = orbSpawnCooldown;
        }

        if (_currentOrbSpawnCooldown == 0 && _currentGemSpawnCooldown == 0)
            _currentOrbSpawnCooldown++;

        if (_currentGemSpawnCooldown == 0)
        {
            SpawnGem(wall, lastLadderPosX1, lastLadderPosX2, lastVerticalWallPosX);
            _currentGemSpawnCooldown = gemSpawnCooldown;
        }

        WallBehindLadderScript wallBScript1 = ladder1.GetComponent<WallBehindLadderScript>();
        WallBehindLadderScript wallBScript2 = ladder2.GetComponent<WallBehindLadderScript>();


        wallBScript1.wall = wallBehind1;
        wallBScript1.otherWall = wallBehind2;


        wallBScript2.wall = wallBehind2;
        wallBScript2.otherWall = wallBehind1;
    }

    private void SpawnGem(GameObject wall, float lastLadderPosX1, float lastLadderPosX2, float lastVerticalVallPosX)
    {
        float wallSizeY = wallSpriteRenderer.size.y;
        float wallPosY = wall.transform.position.y;
        Vector2 gemSpawnPos = spawnPointLeft.transform.position;
        
        gemSpawnPos.y = wallPosY - 1.5f * (wallSizeY * _wallScale);
        do
        {
            gemSpawnPos.x = Random.Range(spawnPointLeft.transform.position.x + 0.5f, spawnPointRight.transform.position.x - 0.5f);
        } while ((Mathf.Abs(gemSpawnPos.x - lastLadderPosX1) < 3f * 0.1258) || (Mathf.Abs(gemSpawnPos.x - lastLadderPosX2) < 3f * 0.1258)
                    || (Mathf.Abs(gemSpawnPos.x - lastVerticalWallPosX) < 3f * 0.1258));// 2.5f - gems size, 0.1258 - gems scale, havent added variables for it

        Instantiate(gem, gemSpawnPos, Quaternion.identity);
    }

    private void SpawnOrb(GameObject wall, float lastLadderPosX1, float lastLadderPosX2, float lastVerticalVallPosX)
    {
        SpawnCloud();
        float wallSizeY = wallSpriteRenderer.size.y;
        float wallPosY = wall.transform.position.y;
        Vector2 orbSpawnPos = spawnPointLeft.transform.position;

        orbSpawnPos.y = wallPosY - 1.5f * (wallSizeY * _wallScale);
        do
        {
            orbSpawnPos.x = Random.Range(spawnPointLeft.transform.position.x + 0.5f, spawnPointRight.transform.position.x - 0.5f);
        } while ((Mathf.Abs(orbSpawnPos.x - lastLadderPosX1) < 3f * 0.1258) || (Mathf.Abs(orbSpawnPos.x - lastLadderPosX2) < 3f * 0.1258)
                    || (Mathf.Abs(orbSpawnPos.x - lastVerticalWallPosX) < 3f * 0.1258));

        if (GlobalVariables.fallSpeed < 0.6f)
        {
            Instantiate(SpeedUpOrb, orbSpawnPos, Quaternion.identity);
        }
        else if (GlobalVariables.fallSpeed > 1.4f)
        {
            Instantiate(SlowDownOrb, orbSpawnPos, Quaternion.identity);

        }
        else
        {
            int randOrb = Random.Range(1, 3);
            if (randOrb == 1)
            {
                Instantiate(SpeedUpOrb, orbSpawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(SlowDownOrb, orbSpawnPos, Quaternion.identity);
            }
        }

    }

    private int FindUnusedObject(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (!obj[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Resize(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            _objRenderer = obj[i].GetComponent<SpriteRenderer>();
            Vector2 objSize = _objRenderer.size;
            objSize.x *= _screenWidthInUnits / GlobalVariables.commonScreenWidthInUnits;
            _objRenderer.size = objSize;
        }
    }
    public void GameOver()
    {
        onGameOverScreen = true;
        gameOverScreenGemsCollected.text += gemsCollected.ToString();
        scoreText.enabled = false;
        gameOverUI.SetActive(true);
        restartButton.SetActive(true);
        GlobalVariables.fallSpeed = 0;
        CancelInvoke("SpawnBoulder");
        _rightWalls.Clear();
        pauseButton.SetActive(false);
    }

    public void Restart()
    {
        StartCoroutine(sceneController.FadeInScreen());
        SceneManager.LoadScene(0);
        GlobalVariables.score = 0;
    }

    private void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounter.text = "FPS: " + fps.ToString();
    }
}
