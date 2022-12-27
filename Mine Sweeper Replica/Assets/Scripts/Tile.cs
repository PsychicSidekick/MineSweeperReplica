using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public Image img;
    public TMP_Text txt;

    public Vector2Int pos;
    public bool revealed = false;
    public int adjMineCount;
    public bool mined;
    public bool flagged = false;
    public int adjFlagCount = 0;
    public List<GameObject> adjTileObjs;

    private void Start()
    {
        adjTileObjs = GetAdjacentTiles(this);
        CountAdjMines(adjTileObjs);
    }

    public void Reveal()
    {
        if (mined)
        {
            return;
        }

        if (adjMineCount > 0)
        {
            txt.text = adjMineCount.ToString();
            revealed = true;
            img.color = Color.green;
            GameManager.instance.CheckWin();
            return;
        }

        revealed = true;
        img.color = Color.green;

        foreach (GameObject tileObj in adjTileObjs)
        {
            if (tileObj.GetComponent<Tile>().revealed)
            {
                continue;
            }
            tileObj.GetComponent<Tile>().Reveal();
        }

        GameManager.instance.CheckWin();
    }

    public void RevealAdjacent()
    {
        CountAdjMines(adjTileObjs);
        if (adjFlagCount != adjMineCount)
        {
            return;
        }

        foreach (GameObject tileObj in adjTileObjs)
        {
            Tile tile = tileObj.GetComponent<Tile>();
            if(!tile.flagged && tile.mined)
            {
                tile.Explode();
            }
            tile.Reveal();
        }

        GameManager.instance.CheckWin();
    }

    public void FlagMine()
    {
        if(flagged)
        {
            flagged = false;
            img.color = Color.white;
        }
        else
        {
            flagged = true;
            img.color = Color.yellow;
        }
    }

    public void Explode()
    {
        img.color = Color.red;
        Debug.Log("You Lose");
    }

    public List<GameObject> GetAdjacentTiles(Tile tile)
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        List<List<GameObject>> tileBoard = GameManager.instance.tileBoard;
        Vector2Int boardSize = GameManager.instance.size;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int xPos = tile.pos.x + i;
                int yPos = tile.pos.y + j;
                
                if (xPos < 0 || xPos >= boardSize.x || yPos < 0 || yPos >= boardSize.y)
                {
                    continue;
                }

                adjacentTiles.Add(tileBoard[xPos][yPos]);
            }
        }

        return adjacentTiles;
    }

    public void CountAdjMines(List<GameObject> adjacentTiles)
    {
        int mineCount = 0;
        int flagCount = 0;

        foreach (GameObject tile in adjacentTiles)
        {
            if (tile.GetComponent<Tile>().mined == true)
            {
                mineCount++;
            }

            if (tile.GetComponent<Tile>().flagged == true)
            {
                flagCount++;
            }
        }

        adjFlagCount = flagCount;
        adjMineCount = mineCount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !flagged)
        {
            if (revealed)
            {
                RevealAdjacent();
            }
            else if (mined)
            {
                Explode();
            }
            else
            {
                Reveal();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right && !revealed)
        {
            FlagMine();
        }
    }
}
