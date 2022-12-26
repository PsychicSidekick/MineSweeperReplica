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
    public int adjacentMineCount;
    public bool mined;
    public bool flagged = false;
    public List<GameObject> adjacentTileObjs;

    private void Start()
    {
        adjacentTileObjs = GetAdjacentTiles(this);
        adjacentMineCount = CountAdjacentMines(adjacentTileObjs);
    }

    public void Reveal()
    {
        if (mined)
        {
            return;
        }

        if (adjacentMineCount > 0)
        {
            txt.text = adjacentMineCount.ToString();
            revealed = true;
            img.color = Color.green;
            return;
        }

        revealed = true;
        img.color = Color.green;

        foreach (GameObject tileObj in adjacentTileObjs)
        {
            if (tileObj.GetComponent<Tile>().revealed)
            {
                continue;
            }
            tileObj.GetComponent<Tile>().Reveal();
        }
    }

    public void RevealAdjacent()
    {
        foreach (GameObject tileObj in adjacentTileObjs)
        {
            Tile tile = tileObj.GetComponent<Tile>();

            tile.Reveal();
        }
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
            img.color = Color.red;
        }
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

    public int CountAdjacentMines(List<GameObject> adjacentTiles)
    {
        int mineCount = 0;

        foreach (GameObject tile in adjacentTiles)
        {
            if (tile.GetComponent<Tile>().mined == true)
            {
                mineCount++;
            }
        }

        return mineCount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (revealed)
        {
            RevealAdjacent();
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Reveal();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            FlagMine();
        }
    }
}
