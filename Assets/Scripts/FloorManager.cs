using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    [SerializeField]
    public GroundPrefabManager groundPrefabManager;

    [HideInInspector]

    public List<RoomManager> rooms;

    [HideInInspector]
    public FloorStats floorStats;

    public delegate void OnFloorFinishedDelegate();

    public event OnFloorFinishedDelegate onFloorFinishedDelegate;

    private GameObject[,] roomImageGrid = new GameObject[3, 3] {
        {null,null,null},
        {null,null,null},
        {null,null,null}
    };

    private RoomManager[,] roomGrid = new RoomManager[3, 3]{
        {null,null,null},
        {null,null,null},
        {null,null,null}
    };

    private Vector3[,] roomPositions = new Vector3[3, 3]{
        {new Vector3(-74.47f,-2,0),new Vector3(0,-2,0), new Vector3(74.7f,-2,0)},
        {new Vector3(-74.7f,-2,-74.7f),new Vector3(0,-2,-74.7f), new Vector3(74.7f,-2,-74.47f)},
        {new Vector3(-74.7f,-2,-148.94f),new Vector3(0,-2,-148.94f), new Vector3(74.7f,-2,-148.94f)}
    };

    private PlayerController player;

    private Vector3 playerDefaultPosition = new Vector3(-74.7f, 0, 0);

    private int[] activeGridPos = { 0, 0 };



    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        var map = GameObject.Find("Map");

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                roomImageGrid[r, c] = map.transform.Find($@"{r}-{c}").gameObject;
                roomImageGrid[r, c].SetActive(false);
            }
        }
    }
    // void Update()
    // {

    // }

    public void GenerateRooms(FloorStats stats)
    {
        floorStats = stats;

        Reset();

        int r = 0, c = 0;

        var roomToInstantiate = groundPrefabManager.GetRandomRoom(floorStats.level);

        string previousOppChoice = null;

        RoomManager currentRoom = Instantiate(roomToInstantiate, roomPositions[r, c], Quaternion.identity).GetComponent<RoomManager>();
        currentRoom.gridPos = new int[2] { r, c };

        currentRoom = AssignUIMap(currentRoom, roomImageGrid[r, c]);
        currentRoom.roomStats = floorStats.rooms[0];
        currentRoom.onRoomFinished += OnRoomFinished;
        currentRoom.onRoomStarted += OnRoomStarted;
        currentRoom.onFloorFinished += FloorFinished;

        rooms.Add(currentRoom);

        do
        {
            List<string> choices = new List<string> { "up", "down", "left", "right" };

            if (previousOppChoice != null)
            {
                choices.RemoveAt(choices.IndexOf(previousOppChoice));
            }

            if (r == 0)
            {
                choices.RemoveAt(choices.IndexOf("up"));
            }

            if (c == 0)
            {
                choices.RemoveAt(choices.IndexOf("left"));
            }

            if (r == 2)
            {
                choices.RemoveAt(choices.IndexOf("down"));
            }

            if (c == 2)
            {
                choices.RemoveAt(choices.IndexOf("right"));
            }

            int choiceIndex = Random.Range(0, choices.Count);
            string choice = choices[choiceIndex];
            previousOppChoice = GetOppositeChoice(choice);

            switch (choice)
            {
                case "up":
                    r--;
                    break;
                case "left":
                    c--;
                    break;
                case "right":
                    c++;
                    break;
                case "down":
                    r++;
                    break;
            }

            currentRoom.pathsToOpen.Add(choice);


            if (roomGrid[r, c] == null)
            {

                var groundToInstantiate = groundPrefabManager.GetRandomRoom(floorStats.level);

                currentRoom = Instantiate(groundToInstantiate, roomPositions[r, c], Quaternion.identity).GetComponent<RoomManager>();
                currentRoom.gridPos = new int[2] { r, c };
                currentRoom = AssignUIMap(currentRoom, roomImageGrid[r, c]);
                currentRoom.pathsToOpen.Add(previousOppChoice);

                currentRoom.onRoomFinished += OnRoomFinished;
                currentRoom.onRoomStarted += OnRoomStarted;
                currentRoom.onFloorFinished += FloorFinished;
                currentRoom.roomStats = floorStats.rooms[rooms.Count()];
                currentRoom.isLastRoom = rooms.Count() + 1 == floorStats.rooms.Count();
                roomGrid[r, c] = currentRoom;

                rooms.Add(currentRoom);
            }
            else
            {
                currentRoom = roomGrid[r, c];
                currentRoom.pathsToOpen.Add(previousOppChoice);
            }

        } while (rooms.Count() < floorStats.rooms.Count());

        foreach (var room in rooms)
        {
            room.ShowActivePathInMaps();
        }

    }

    public void Reset()
    {
        ResetRooms();
        ResetPlayerPosition();
    }

    public void ResetRooms()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        rooms.Clear();
    }

    public void ResetPlayerPosition()
    {
        player.transform.position.Set(playerDefaultPosition.x, playerDefaultPosition.y, playerDefaultPosition.z);
    }

    public void FloorFinished()
    {
        onFloorFinishedDelegate();
    }

    private void OnRoomFinished(int[] gridPos, bool isLastRoom)
    {
        foreach (var room in rooms)
        {
            room.OpenPaths();
        }
    }

    private void OnRoomStarted(int[] gridPos)
    {
        var hasRoomChanged = gridPos.ToString() != activeGridPos.ToString();

        activeGridPos = hasRoomChanged ? gridPos : activeGridPos;

        foreach (var room in rooms)
        {
            if (hasRoomChanged && room.gridPos.ToString() != activeGridPos.ToString())
            {
                room.HideProps();
                room.isActive = false;
            }
            room.ClosePaths();
        }
    }

    private string GetOppositeChoice(string choice)
    {
        switch (choice)
        {
            case "up":
                return "down";
            case "left":
                return "right";
            case "right":
                return "left";
            case "down":
                return "up";
            default:
                return "down";
        }
    }

    private RoomManager AssignUIMap(RoomManager room, GameObject uiMap)
    {
        uiMap.SetActive(true);
        room.uiMap = uiMap;
        room.leftImagePath = uiMap.transform.Find("left").gameObject;
        room.rightImagePath = uiMap.transform.Find("right").gameObject;
        room.topImagePath = uiMap.transform.Find("top").gameObject;
        room.bottomImagePath = uiMap.transform.Find("bottom").gameObject;
        return room;
    }

}
