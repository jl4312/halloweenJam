using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class GroundManager : MonoBehaviour
    {

        public GameObject gridTiles;
        public int gridWidth = 2;
        public int gridHeight = 2;
        public float heightAboveGround = 2;

        List<Vector2> listOfBlockedOffAreas;
        TileScript[] grid;
        int numOfTiles;

        float tileXScale;
        float tileZScale;
        float planeDefaultSize = 10;


        // Use this for initialization
        void Start()
        {
            numOfTiles = gridWidth * gridHeight;
            tileXScale = (transform.localScale.x / gridWidth) * planeDefaultSize;
            tileZScale = (transform.localScale.y / gridHeight) * planeDefaultSize;

            listOfBlockedOffAreas = new List<Vector2>();

            grid = new TileScript[numOfTiles];
            SetupTiles();

        }

        // Update is called once per frame
        void Update()
        {

        }
        void SetupTiles()
        {
            float xPos = (transform.localScale.x / -2) * planeDefaultSize + (tileXScale / 2);
            float zPos = (transform.localScale.z / 2) * planeDefaultSize - (tileZScale / 2);

            int rowCounter = 0;
            int colCounter = 0;

            for (int i= 0; i < numOfTiles; i++)
            {
                TileScript tile = gridTiles.transform.GetChild(i).GetComponent<TileScript>();

                //tile.gameObject.SetActive(true);

                tile.transform.localScale = new Vector3(tileXScale, tile.transform.localScale.y, tileZScale);
                tile.transform.position = transform.position + new Vector3(xPos, transform.position.y + heightAboveGround, zPos);

                tile.SetRowCol(rowCounter, colCounter);

                xPos += tileXScale;
                rowCounter++; 

                if (rowCounter >= gridWidth)
                {
                    rowCounter = 0;
                    colCounter++;
                    xPos = (transform.localScale.x / -2) * planeDefaultSize + (tileXScale / 2);
                    zPos -= tileZScale;
                }

                grid[i] = tile;
            }

        }
        public void ExposeTileArea(TileScript tile)
        {
            tile.TurnOnTile();
            for (int i = 0; i < listOfBlockedOffAreas.Count; i++)
            {
                int row = (int)listOfBlockedOffAreas[i].x + tile.GetRowNum();
                int col = (int)listOfBlockedOffAreas[i].y + tile.GetColNum();

                TileScript newTile = GrabTile(row, col);

                if (newTile != null)
                {
                    newTile.TurnOffTile();
                }                
            }
        }
        public void SetTileSpace(TileScript tile)
        {
            tile.spaceInUse = true;
            for (int i = 0; i < listOfBlockedOffAreas.Count; i++)
            {
                int row = (int)listOfBlockedOffAreas[i].x + tile.GetRowNum();
                int col = (int)listOfBlockedOffAreas[i].y + tile.GetColNum();

                TileScript newTile = GrabTile(row, col);

                if (newTile != null)
                {
                    newTile.spaceInUse = true;
                }
            }
        }
        public TileScript GrabTile(int row, int col)
        {
            TileScript tile = null;

            if ((row < gridWidth && col < gridHeight) && (row >= 0 && col >= 0))
            {
                tile = grid[(gridWidth * col) + row];
            }

            return tile;
        }

        public void TurnOnTiles(List<Vector2> blockedOffAreas)
        {
            listOfBlockedOffAreas = blockedOffAreas;

            for (int i= 0; i < numOfTiles; i++)
            {
                grid[i].gameObject.SetActive(true);
            }
        }
        public void TurnOffTiles()
        {
            for (int i = 0; i < numOfTiles; i++)
            {
                grid[i].DefaultTile();
                grid[i].gameObject.SetActive(false);
            }
        }

        Vector2 WorldPos2TilePos(Vector3 worldPos)
        {
            Vector2 tilePos = new Vector2(1,1);

            return tilePos;
        }
    }
}
