﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MisfitMakers
{
    public enum StructureType
    {
        Torrent
    }

    public class GameManager : MonoBehaviour
    {
        [Header("Game Manager Settings")]
        public GameObject mainCam;

        [Header("Structure Pools")]
        public GameObject torrentBulletPool;
		public GameObject torrentArcPool;

		Camera cam;
        StructureBase structureToPlace;
        GroundManager groundManager;
        GameObject enemySpawnPoints;
        GameObject ground;
        Vector3 distToGround;
        Vector3 mousePosWorld;
        Vector3 pointOnGround;
        float distToGroundMag;
        int numOfSpawns;


		//currency information
		[Header("Currency")]
		public TextMeshProUGUI moneyText;
		float currentMoney = 500;


        // Use this for initialization
        void Start()
        {
            cam = mainCam.GetComponent<Camera>();
            enemySpawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints");
            ground = GameObject.FindGameObjectWithTag("Ground");
            groundManager = ground.GetComponent<GroundManager>();

            distToGround = ground.transform.position - mainCam.transform.position;
            distToGroundMag = distToGround.magnitude;
            numOfSpawns = enemySpawnPoints.transform.childCount;

			moneyText.text = "" + currentMoney;

        }
        void OnGUI()
        {
            Event currentEvent = Event.current;

            Vector2 mousePos = new Vector2();

            mousePos.x = currentEvent.mousePosition.x;
            mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;


            mousePosWorld = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            
        }
        // Update is called once per frame
        void Update()
        {
			CameraPan();
            if (structureToPlace != null)
            {
                bool hitGround = false;
                bool hasHitATile = false;

                TileScript tile = new TileScript();

                Vector3 correctDir = mousePosWorld - mainCam.transform.position;

                RaycastHit[] hit;
                hit = Physics.RaycastAll(mousePosWorld, correctDir, cam.farClipPlane);

                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].transform.gameObject == structureToPlace.gameObject)
                    {
                        continue;
                    }

                    if (hit[i].transform.CompareTag("Ground"))
                    {
                        pointOnGround = hit[i].point;
                        hitGround = true;                       
                    }
                    else if(hit[i].transform.CompareTag("Structure"))
                    {
                        hitGround = false;
                        continue;
                    }
                    else if (hit[i].transform.CompareTag("Tile") && !hasHitATile)
                    {
                        hasHitATile = true;
                        tile =  hit[i].transform.GetComponent<TileScript>();
                        groundManager.ExposeTileArea(tile);

                        //if (!hitGround)
                        //{
                        //    tile.spaceInUse = true;
                        //} 
                    }
                }
	
                if (hitGround && hasHitATile)
                {
                    structureToPlace.transform.position = tile.transform.position;
                }
                else
                {
                    structureToPlace.transform.position = new Vector3(10000,0,0);
                }

                if (Input.GetMouseButtonDown(0) && hitGround)
                {
                    if (hasHitATile && !tile.spaceInUse)
                    {
                        PlaceStructure(tile);
                    }                   
                }
            }
           
        }
        void PlaceStructure(TileScript tile)
        {
			if (currentMoney >= structureToPlace.cost)
            {
                structureToPlace.isActive = true;

                currentMoney -= structureToPlace.cost;
				moneyText.text = "" +  currentMoney;

				structureToPlace.ResetStructure();
				structureToPlace.Build();
				
				structureToPlace = null;

                groundManager.SetTileSpace(tile);
                groundManager.TurnOffTiles();
            }
			else
				Debug.Log ("Not Enough Money" + currentMoney + "/" + structureToPlace.cost);
        }

        public void LoadTorrentBulletStructure()
        {
            GameObject child = GetInactiveChild(torrentBulletPool);
            if (child != null)
            {
                structureToPlace = child.GetComponent<StructureBase>();
                structureToPlace.isActive = false;
                groundManager.TurnOnTiles(structureToPlace.borderList);
            }
            
        }
        public void LoadTorrentArcStructure()
        {
            GameObject child = GetInactiveChild(torrentArcPool);
            if (child != null)
            {
                structureToPlace = child.GetComponent<StructureBase>();
                structureToPlace.isActive = false;
                groundManager.TurnOnTiles(structureToPlace.borderList);
            }

           
        }
        GameObject GetInactiveChild(GameObject parent)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (!parent.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(true);
                    return parent.transform.GetChild(i).gameObject;
                }
            }

            Debug.Log("Got no children");
            return null;
        }


        public void SpawnEnemy(GameObject ene)
        {
            int ranNum = Random.Range(0, numOfSpawns);

            Transform spawnPoint = enemySpawnPoints.transform.GetChild(ranNum);
            ene.transform.position = spawnPoint.position;

            ene.SetActive(true);
        }

		//Debug Camera Panning
		void CameraPan(){

			float offset = .2f;
			Vector3 translation = cam.transform.position ;
			if (Input.GetKey (KeyCode.W))
				translation.y += offset;
			if(Input.GetKey(KeyCode.S))
			   translation.y -= offset;
			if (Input.GetKey (KeyCode.A)) {
				translation.x += offset / 2;
				translation.z -= offset / 2;
			}
			if (Input.GetKey (KeyCode.D)) {
				translation.x -= offset / 2;
				translation.z += offset / 2;
			}

			if (Input.GetKey (KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") > 0f) {
				cam.orthographicSize -= offset;
			}
			if (Input.GetKey (KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") < 0f) {
				cam.orthographicSize += offset;
			}
			cam.transform.position = translation;
	
		}
    }
}
