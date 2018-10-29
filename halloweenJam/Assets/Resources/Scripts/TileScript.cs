using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class TileScript : MonoBehaviour
    {
        public Color defaultColor;
        public Color canPlaceDownColor;
        public Color canNotPlaceDownColor;

        Material myMat;

        int rowNum;
        int colNum;

        int timer = 0;

        [HideInInspector]
        public bool spaceInUse = false;

        // Use this for initialization
        void Start()
        {
            myMat = GetComponent<Renderer>().material;
            DefaultTile();
            this.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (!spaceInUse)
            {
                if (timer == 0)
                {
                    DefaultTile();
                }
                else
                {
                    timer--;
                }

            }
            else
            {
                TurnOffTile();
            }
            
        }
        public void SetRowCol(int r, int c)
        {
            rowNum = r;
            colNum = c;
        }
        public int GetRowNum()
        {
            return rowNum;
        }
        public int GetColNum()
        {
            return colNum;
        }

        public void TurnOnTile()
        {
            myMat.color = canPlaceDownColor;
            timer = 2;
        }
        public void TurnOffTile()
        {
            myMat.color = canNotPlaceDownColor;
            timer = 2;
        }
        public void DefaultTile()
        {
            myMat.color = defaultColor;
        }
    }
}
