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
    public bool mined = false;
    public bool flagged = false;
    public int adjMineCount = 0;
    public int adjFlagCount = 0;

    public List<Tile> adjTiles;

    private void Start()
    {
        adjTiles = GetAdjacentTiles(this);
        CountAdjMines(adjTiles);
    }

    public void Reveal()
    {
        if (mined)
        {
            return;
        }

        revealed = true;
        img.color = Color.green;

        if (adjMineCount > 0)
        {
            txt.text = adjMineCount.ToString();
            GameManager.instance.CheckWin();
            return;
        }

        foreach (Tile tile in adjTiles)
        {
            if (tile.revealed)
            {
                continue;
            }
            tile.Reveal();
        }

        GameManager.instance.CheckWin();
    }

    public void RevealAdjacent()
    {
        CountAdjMines(adjTiles);
        if (adjFlagCount != adjMineCount)
        {
            return;
        }

        foreach (Tile tile in adjTiles)
        {
            if (!tile.flagged && tile.mined)
            {
                tile.Explode();
            }
            tile.Reveal();
        }

        GameManager.instance.CheckWin();
    }

    public void FlagMine()
    {
        if (flagged)
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
        GameManager.instance.gameoverPanel.SetActive(true);
    }

    public List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> adjacentTiles = new List<Tile>();
        List<List<Tile>> tileBoard = GameManager.instance.tileBoard;
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

    public void CountAdjMines(List<Tile> adjacentTiles)
    {
        int mineCount = 0;
        int flagCount = 0;

        foreach (Tile tile in adjacentTiles)
        {
            if (tile.mined == true)
            {
                mineCount++;
            }

            if (tile.flagged == true)
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
