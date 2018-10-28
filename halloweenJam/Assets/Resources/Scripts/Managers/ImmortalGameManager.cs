using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class ImmortalGameManager
    {
        
        public static ImmortalGameManager GM;
        public bool firstStartedUp = true;

        bool hasAplayerLoaded = false;

        public int highScore = 0;
        // Use this for initialization
        void Start()
        {

        }
        static public ImmortalGameManager GetInstance()
        {
            //Debug.Log("Is Awake");
            if (GM == null)
            {
                GM = new ImmortalGameManager();
            }
            else
            {
                //GM = this;
                if (GM.firstStartedUp)
                {
                    GM.firstStartedUp = false;
                }
            }
            return GM;
        }



        //public static void SaveToPlayerFile(PlayerBase player)
        //{
        //    FileIO.InitWriteToFile("Data/PlayerInfo.txt", true);

        //    FileIO.WriteLineToFile(player.playerName);
        //    FileIO.WriteLineToFile("" + player.playerLvl);
        //    FileIO.WriteLineToFile("" + player.currency);
        //    FileIO.WriteLineToFile("" + player.currentExp);
        //    FileIO.WriteLineToFile("" + player.maxExp);
        //    FileIO.WriteLineToFile("" + player.personalHighscore);
        //    FileIO.WriteLineToFile("" + player.ownedShipsInfo.currentShip);
        //    FileIO.WriteLineToFile("" + player.ownedShipsInfo.ownedShipCount);

        //    FileIO.EndWriteToFile();

        //    FileIO.InitWriteToFile("Data/PlayerShipInfo.txt", true);
        //    int shipCount = player.ownedShipsInfo.ownedShips.Length;
        //    FileIO.WriteLineToFile("" + shipCount);

        //    for (int i = 0; i < shipCount; i++)
        //    {
        //        bool shipOwned = player.ownedShipsInfo.ownedShips[i];
        //        FileIO.WriteLineToFile("" + shipOwned);
        //    }
        //    FileIO.EndWriteToFile();

        //    SaveSettingsToFile();
        //}
        //public static void SaveSettingsToFile()
        //{
        //    FileIO.InitWriteToFile("Data/Settings.txt", true);
        //    FileIO.WriteLineToFile("" + GM.TutorialOn);
        //    FileIO.EndWriteToFile();
        //}

        //public static void LoadSettings()
        //{
        //    List<string> info = FileIO.ReadTextFile("Data/Settings.txt");

        //    if (info != null)
        //    {
        //        if (info[0] == "True")
        //        {
        //            TutorialON();
        //        }
        //        else
        //        {
        //            TutorialOFF();
        //        }
        //    }
        //}

        public static void SaveHighScore(int score)
        {
            //if (GM != null)
            //{

            //    if (GM.playerCopy != null && GM.playerCopy.personalHighscore < score)
            //    {
            //        GM.playerCopy.personalHighscore = score;
            //    }

            //    if (GM.highScore < score)
            //    {
            //        GM.highScore = score;
            //    }
                
            //}
        }
        public static int GetHighScore()
        {
            int score = 0;

            if(GM != null)
            {
                score = GM.highScore;
            }
            return score;
        }

        // Update is called once per frame
        void Update()
        {

        }

		public static void ResetPlayerFile()
		{
			FileIO.DeleteFile("Data/PlayerInfo.txt");			
			FileIO.DeleteFile("Data/PlayerShipInfo.txt");
			
			//SaveSettingsToFile();
		}
    }
}
