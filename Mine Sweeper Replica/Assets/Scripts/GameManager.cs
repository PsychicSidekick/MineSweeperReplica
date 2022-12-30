using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<List<Tile>> tileBoard;

    public Vector2Int size;
    public int mineCount;
    public GameObject tilePrefab;

    public Transform gamePanel;
    public GameObject winPanel;
    public GameObject gameoverPanel;

    public Button startBtn;
    public Button returnTitleBtn1;
    public Button returnTitleBtn2;

    float tileSize;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Main")
        {
            Initiate();
        }
    }

    private void Initiate()
    {
        tileBoard = new List<List<Tile>>();

        gamePanel = GameObject.Find("Canvas/TilePanel").transform;
        winPanel = GameObject.Find("Canvas/WinPanel");
        gameoverPanel = GameObject.Find("Canvas/GameoverPanel");

        winPanel.SetActive(false);
        gameoverPanel.SetActive(false);

        tileSize = tilePrefab.GetComponent<RectTransform>().rect.width;
        InitiateTiles();
        AddRandomMines();
    }

    public void InitiateTiles()
    {
        Vector2 firstTilePos = FindFirstTilePos();

        for (int i = 0; i < size.x; i++)
        {
            List<Tile> row = new List<Tile>();

            for (int j = 0; j < size.y; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, gamePanel.transform);
                newTile.transform.localPosition = firstTilePos + new Vector2(i * tileSize, -j * tileSize);
                newTile.GetComponent<Tile>().pos = new Vector2Int(i, j);

                row.Add(newTile.GetComponent<Tile>());
            }

            tileBoard.Add(row);
        }
    }

    public void CheckWin()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Tile tile = tileBoard[i][j].GetComponent<Tile>();

                if (!tile.mined && !tile.revealed)
                {
                    return;
                }
            }
        }

        winPanel.SetActive(true);
    }

    public void AddRandomMines()
    {
        int remainingMines = mineCount;

        while (remainingMines > 0)
        {
            int xPos = Random.Range(0, size.x);
            int yPos = Random.Range(0, size.y);

            Tile selectedTile = tileBoard[xPos][yPos].GetComponent<Tile>();

            if (selectedTile.mined)
            {
                continue;
            }

            selectedTile.mined = true;
            remainingMines -= 1;
        }

    }

    public Vector2 FindFirstTilePos()
    {
        Vector2 firstTilePos = new Vector2(0, 0);

        float xOffSet = size.x % 2 == 0 ? tileSize/2 : 0;
        float yOffSet = size.y % 2 == 0 ? tileSize/2 : 0;

        firstTilePos.x = -Mathf.Floor(size.x / 2) * tileSize + xOffSet;
        firstTilePos.y = Mathf.Floor(size.y / 2) * tileSize - yOffSet;

        return firstTilePos;
    }
}
