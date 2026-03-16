using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject layoutRoom;
    [SerializeField]
    private Color startColor, endColor, shopColor, chestColor;
    [SerializeField]
    private float distanceToEnd;
    [SerializeField]
    private bool includeShop;
    [SerializeField]
    private int minDistanceToShop, maxDistanceToShop;
    [SerializeField]
    private bool includeChest;
    [SerializeField]
    private int minDistanceToChest, maxDistanceToChest;
    [SerializeField]
    private Transform generatorPoint;

    public enum Direction { up, right, down, left };
    [SerializeField]
    private Direction selectedDirection;
    [SerializeField]
    private float xOffset = 18f, yOffset = 10f;
    [SerializeField]
    private LayerMask whatIsRoom;
    [SerializeField]
    private RoomCenter centerStart, centerEnd, centerShop, centerChest;
    [SerializeField]
    private RoomCenter[] potentialsCenters;
    private GameObject endRoom, shopRoom, chestRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    [SerializeField]
    private RoomPrefabs rooms;
    private List<GameObject> generatedOutlines = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            layoutRoomObjects.Add(newRoom);

            if (i + 1 == distanceToEnd)
            {
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);

                endRoom = newRoom;
                endRoom.GetComponent<SpriteRenderer>().color = endColor;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, 0.2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (includeShop)
        {
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop + 1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;

            if (includeChest && shopSelector < maxDistanceToChest)
            {
                // 调整chest的范围，因为少了一个房间
                maxDistanceToChest = Mathf.Min(maxDistanceToChest, layoutRoomObjects.Count - 1);
                if (shopSelector < minDistanceToChest)
                {
                    minDistanceToChest = Mathf.Max(0, minDistanceToChest - 1);
                }
            }
        }

        if (includeChest)
        {
            // 确保选择范围有效
            int maxChestIndex = Mathf.Min(maxDistanceToChest, layoutRoomObjects.Count - 1);
            int minChestIndex = Mathf.Min(minDistanceToChest, maxChestIndex);

            if (minChestIndex <= maxChestIndex && layoutRoomObjects.Count > 0)
            {
                int chestSelector = Random.Range(minChestIndex, maxChestIndex + 1);
                chestRoom = layoutRoomObjects[chestSelector];
                layoutRoomObjects.RemoveAt(chestSelector);
                chestRoom.GetComponent<SpriteRenderer>().color = chestColor;
            }
            else
            {
                Debug.LogWarning("Chest room range is invalid, skipping chest generation");
            }
        }

        //if (includeChest)
        //{
        //    int chestSelector = Random.Range(minDistanceToChest, maxDistanceToChest + 1);
        //    chestRoom = layoutRoomObjects[chestSelector];
        //    layoutRoomObjects.RemoveAt(chestSelector);
        //    chestRoom.GetComponent<SpriteRenderer>().color = chestColor;


        //}

        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        if (includeShop)
        {
            CreateRoomOutline(shopRoom.transform.position);
        }
        if (includeChest)
        {
            CreateRoomOutline(chestRoom.transform.position);
        }

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (includeShop)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (includeChest)
            {
                if (outline.transform.position == chestRoom.transform.position)
                {
                    Instantiate(centerChest, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialsCenters.Length);
                Instantiate(potentialsCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITYE_EDITOR
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    private void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
            directionCount++;
        if (roomBelow)
            directionCount++;
        if (roomLeft)
            directionCount++;
        if (roomRight)
            directionCount++;

        switch (directionCount)
        {
            case 0:
                Debug.Log("Didn't find a room!");
                break;
            case 1:
                if (roomAbove)
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                if (roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                if (roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                if (roomRight)
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                break;
            case 2:
                if (roomAbove && roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                if (roomAbove && roomRight)
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                if (roomAbove && roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                if (roomBelow && roomLeft)
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                if (roomBelow && roomRight)
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                if (roomLeft && roomRight)
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                break;
            case 3:
                if (roomLeft && roomRight && roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.tripleUp, roomPosition, transform.rotation));
                if (roomAbove && roomRight && roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.tripleLeft, roomPosition, transform.rotation));
                if (roomAbove && roomLeft && roomBelow)
                    generatedOutlines.Add(Instantiate(rooms.tripleRight, roomPosition, transform.rotation));
                if (roomAbove && roomLeft && roomRight)
                    generatedOutlines.Add(Instantiate(rooms.tripleDown, roomPosition, transform.rotation));
                break;
            case 4:
                if (roomAbove && roomBelow && roomLeft && roomRight)
                    generatedOutlines.Add(Instantiate(rooms.fourWay, roomPosition, transform.rotation));
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleLeft, singleRight, singleUp, singleDown,
        doubleLeftRight, doubleUpDown, doubleUpRight, doubleRightDown, doubleDownLeft, doubleUpLeft,
        tripleLeft, tripleRight, tripleUp, tripleDown,
        fourWay;
}
