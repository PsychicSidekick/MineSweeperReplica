using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<List<GameObject>> tileBoard = new List<List<GameObject>>();

    public Vector2Int size;
    public GameObject tilePrefab;
    public Transform gamePanel;

    float tileSize;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        tileSize = tilePrefab.GetComponent<RectTransform>().rect.width;
        initiateTiles();
    }

    public void initiateTiles()
    {
        Vector2 firstTilePos = findFirstTilePos();

        for (int i = 0; i < size.x; i++)
        {
            List<GameObject> row = new List<GameObject>();

            for (int j = 0; j < size.y; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, gamePanel.transform);
                newTile.transform.localPosition = firstTilePos + new Vector2(i * tileSize, -j * tileSize);
                newTile.GetComponent<Tile>().pos = new Vector2Int(i, j); 
                row.Add(newTile);
            }

            tileBoard.Add(row);
        }
    }

    public Vector2 findFirstTilePos()
    {
        Vector2 firstTilePos = new Vector2(0, 0);

        float xOffSet = size.x % 2 == 0 ? tileSize/2 : 0;
        float yOffSet = size.y % 2 == 0 ? tileSize/2 : 0;

        firstTilePos.x = -Mathf.Floor(size.x / 2) * tileSize + xOffSet;
        firstTilePos.y = Mathf.Floor(size.y / 2) * tileSize - yOffSet;

        return firstTilePos;
    }
}
