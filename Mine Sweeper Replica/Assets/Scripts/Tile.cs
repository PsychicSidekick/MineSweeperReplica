using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int pos;
    public Button btn;
    public bool revealed;
    public bool mined;

    public void Reveal()
    {
        foreach (GameObject tileObj in GetAdjacentTiles(this))
        {
            Debug.Log(tileObj.GetComponent<Tile>().pos);
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
                if(i == 0 && j == 0)
                {
                    continue;
                }

                int xPos = tile.pos.x + i;
                int yPos = tile.pos.y + j;
                
                if(xPos < 0 || xPos > boardSize.x || yPos < 0 || yPos > boardSize.y)
                {
                    continue;
                }

                adjacentTiles.Add(tileBoard[xPos][yPos]);
            }
        }

        return adjacentTiles;
    }

    public void OnClick()
    {
        Reveal();
        btn.interactable = false;
    }
}
