using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{


    [SerializeField]
    public GameObject leftPath;

    [SerializeField]
    public GameObject rightPath;

    [SerializeField]
    public GameObject topPath;

    [SerializeField]
    public GameObject bottomPath;


    [SerializeField]
    public GameObject leftImagePath;

    [SerializeField]
    public GameObject rightImagePath;

    [SerializeField]
    public GameObject topImagePath;

    [SerializeField]
    public GameObject bottomImagePath;

    [SerializeField]
    public GameObject floorGemPrefab;

    [SerializeField]
    public GameObject chestPrefab;

    [HideInInspector]
    public FloorGem floorGem;

    [HideInInspector]
    public GameObject uiMap;

    [HideInInspector]
    public RectTransform playerPosImage;

    [HideInInspector]
    public GameObject propsContainer;


    [HideInInspector]
    public List<string> pathsToOpen;

    [HideInInspector]
    public int[] gridPos;

    [HideInInspector]
    public bool isStarted = false;

    [HideInInspector]
    public bool isFinished = false;

    [HideInInspector]
    public bool isLastRoom = false;

    [HideInInspector]
    public bool isOpened = false;

    [HideInInspector]
    public RoomStats roomStats;


    [HideInInspector]
    public bool isActive = false;



    public delegate void OnRoomStarted(int[] gridPos);
    public event OnRoomStarted onRoomStarted;

    public delegate void OnRoomFinished(int[] gridPos, bool isLastRoom);
    public event OnRoomFinished onRoomFinished;

    public delegate void OnFloorFinished();
    public event OnFloorFinished onFloorFinished;

    [HideInInspector]
    public List<EnemyController> enemies = new List<EnemyController>();


    private EnemyPrefabManager enemyPrefabManager;

    private NotificationUIManager notificationUIManager;

    private Text waveText;


    private PlayerController player;

    private int currentEnemyWaveIndex = 0;


    void Start()
    {
        enemyPrefabManager = GameObject.Find("EnemyPrefabManager").GetComponent<EnemyPrefabManager>();
        notificationUIManager = GameObject.Find("InGameNotification").GetComponent<NotificationUIManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        propsContainer = transform.Find("Props").gameObject;
        playerPosImage = GameObject.Find("PlayerPosImage").GetComponent<RectTransform>();
        waveText = GameObject.Find("WaveText").GetComponent<Text>();
    }


    void LateUpdate()
    {
        CheckPlayerIsWithinRoomBounds();
        TrackOfPlayerPosition();

        GenerateEnemyWaves();
    }

    public void GenerateEnemyWaves()
    {
        if (roomStats != null)
        {

            if (isActive && !isFinished && currentEnemyWaveIndex < roomStats.enemyWaveGroups.GetLength(0) && isStarted && enemies.Count == 0)
            {
                var activeEnemyWaves = roomStats.enemyWaveGroups[currentEnemyWaveIndex++].enemyWaves;

                waveText.text = $"Wave: {currentEnemyWaveIndex} / {roomStats.enemyWaveGroups.GetLength(0)}";

                foreach (var enemyWave in activeEnemyWaves)
                {
                    for (var i = 0; i < enemyWave.enemyCount; i++)
                    {
                        GenerateEnemy(enemyWave.enemyType);
                    }
                }
            }

            if (!isFinished && currentEnemyWaveIndex == roomStats.enemyWaveGroups.GetLength(0) && enemies.Count == 0 && isStarted)
            {
                onRoomFinished(gridPos, isLastRoom);
                notificationUIManager.ShowNotification("RoomCleared");
                isFinished = true;

                if (isLastRoom)
                {
                    var floorGemObj = Instantiate(floorGemPrefab, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
                    floorGem = floorGemObj.GetComponent<FloorGem>();
                    floorGem.onDeathDelegate += OnFloorGemDestroyed;
                }

                if (roomStats.hasChest)
                {
                    var chest = Instantiate(chestPrefab, new Vector3(transform.position.x, 5, transform.position.z + 5), Quaternion.identity);
                    var chestScript = chest.GetComponent<Chest>();

                    chestScript.noOfGems = roomStats.chestGem;
                    chestScript.noOfCoins = roomStats.chestCoin;

                    chest.transform.SetParent(transform);
                }

                // floorGem.onDeathDelegate
            }
        }
    }

    public void CheckPlayerIsWithinRoomBounds()
    {
        var player2dPos = new Vector2(player.transform.position.x, player.transform.position.z);
        var room2dPos = new Vector2(transform.position.x, transform.position.z);

        var roomOffset = 12f;
        var roomSizeHalf = (74.47f / 2) - roomOffset;


        if (player2dPos.x >= room2dPos.x - roomSizeHalf && player2dPos.x <= room2dPos.x + roomSizeHalf && player2dPos.y >= room2dPos.y - roomSizeHalf && player2dPos.y <= room2dPos.y + roomSizeHalf)
        {
            isActive = true;
            ShowProps();

            if (!isStarted)
            {
                onRoomStarted(gridPos);
                isStarted = true;
            }
        }

    }

    public void HideProps()
    {
        propsContainer.SetActive(false);
    }

    public void ShowProps()
    {
        propsContainer.SetActive(true);
    }

    public void TrackOfPlayerPosition()
    {
        if (isActive)
        {
            var mapSizeHalf = 22 / 2;
            var roomSizeHalf = 74.47f / 2;

            var xPlayerImage = Map(player.transform.position.x, transform.position.x - roomSizeHalf, transform.position.x + roomSizeHalf, uiMap.transform.position.x - mapSizeHalf, uiMap.transform.position.x + mapSizeHalf);
            var yPlayerImage = Map(player.transform.position.z, transform.position.z - roomSizeHalf, transform.position.z + roomSizeHalf, uiMap.transform.position.y - mapSizeHalf, uiMap.transform.position.y + mapSizeHalf);

            playerPosImage.position = new Vector3(xPlayerImage, yPlayerImage, 0);
        }
    }

    public void OnFloorGemDestroyed()
    {
        onFloorFinished();
    }


    public void OpenPaths()
    {
        if (pathsToOpen.Contains("left"))
        {
            leftPath.SetActive(false);
            // StartCoroutine("OpenPath", leftPath);
        }
        if (pathsToOpen.Contains("right"))
        {
            rightPath.SetActive(false);
            // StartCoroutine("OpenPath", rightPath);
        }
        if (pathsToOpen.Contains("up"))
        {
            topPath.SetActive(false);
            // StartCoroutine("OpenPath", topPath);
        }
        if (pathsToOpen.Contains("down"))
        {
            // StartCoroutine("OpenPath", bottomPath);
            bottomPath.SetActive(false);
        }
    }

    public void ShowActivePathInMaps()
    {
        leftImagePath.SetActive(false);
        rightImagePath.SetActive(false);
        topImagePath.SetActive(false);
        bottomImagePath.SetActive(false);

        if (pathsToOpen.Contains("left"))
        {
            leftImagePath.SetActive(true);
        }
        if (pathsToOpen.Contains("right"))
        {
            rightImagePath.SetActive(true);
        }
        if (pathsToOpen.Contains("up"))
        {
            topImagePath.SetActive(true);
        }
        if (pathsToOpen.Contains("down"))
        {
            bottomImagePath.SetActive(true);
        }
    }

    public void ClosePaths()
    {
        if (pathsToOpen.Contains("left"))
        {
            leftPath.SetActive(true);
            // StartCoroutine("ClosePath", leftPath);
        }
        if (pathsToOpen.Contains("right"))
        {
            rightPath.SetActive(true);
            // StartCoroutine("ClosePath", rightPath);
        }
        if (pathsToOpen.Contains("up"))
        {
            topPath.SetActive(true);
            // StartCoroutine("ClosePath", topPath);
        }
        if (pathsToOpen.Contains("down"))
        {
            // StartCoroutine("ClosePath", bottomPath);
            bottomPath.SetActive(true);
        }
    }

    private IEnumerator OpenPath(GameObject path)
    {
        var time = 1f / 60;
        var targetDestination = new Vector3(path.transform.position.x, path.transform.position.y - 100, path.transform.position.z - 10);
        var vel = targetDestination - path.transform.position;
        vel = vel * time;


        for (var i = 0; i < 60; i++)
        {
            path.transform.position = path.transform.position + vel;
            yield return new WaitForSeconds(time);
        }

        path.SetActive(false);

    }

    private IEnumerator ClosePath(GameObject path)
    {

        var time = 1f / 60;
        var targetDestination = new Vector3(path.transform.position.x, path.transform.position.y + 100, path.transform.position.z);
        var vel = targetDestination - path.transform.position;
        vel = vel * time;


        for (var i = 0; i < 60; i++)
        {
            path.transform.position = path.transform.position + vel;
            yield return new WaitForSeconds(time);
        }
    }

    private void GenerateEnemy(int type)
    {
        var randomEnemyPosition = Vector3.zero;

        var enemyGenerateDistance = 10f;

        var roomHalfSize = 74.47f / 2;

        var roomOffset = 5f;

        var space = roomHalfSize - roomOffset;

        do
        {
            var randomXPos = Random.Range(-space + transform.position.x, space + transform.position.x);
            var randomZPos = Random.Range(-space + transform.position.z, space + transform.position.z);

            randomEnemyPosition.Set(randomXPos, 0, randomZPos);
        } while (Vector3.Distance(randomEnemyPosition, player.transform.position) < enemyGenerateDistance);

        var enemyPrefab = enemyPrefabManager.GetEnemy(type);

        var enemy = Instantiate(enemyPrefab, randomEnemyPosition, Quaternion.identity);
        var enemyScript = enemy.GetComponent<EnemyController>();

        enemyScript.enemyRoomIndex = enemies.Count();
        enemyScript.onDeathDelegate += RemoveEnemy;

        enemies.Add(enemyScript);
    }

    public void RemoveEnemy(int index)
    {
        var enemyIndex = enemies.Select((enemy, i) => new { enemy, i })
                                .Where(enemy => enemy.enemy.enemyRoomIndex == index)
                                .Select(enemy => enemy.i)
                                .FirstOrDefault();
        enemies.RemoveAt(enemyIndex);
    }

    private float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
