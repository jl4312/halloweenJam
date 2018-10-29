using System.Collections;
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
            if (structureToPlace != null)
            {
                bool hitGround = false;

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
                }
	
                if (hitGround)
                {
                    structureToPlace.transform.position = pointOnGround;
                }
                else
                {
                    structureToPlace.transform.position = new Vector3(10000,0,0);
                }

                if (Input.GetMouseButtonDown(0) && hitGround)
                {

                    PlaceStructure();
                }
            }
           
        }
        void PlaceStructure()
        {
            structureToPlace.isActive = true;

			if (currentMoney >= structureToPlace.cost) {
				currentMoney -= structureToPlace.cost;
				moneyText.text = "" +  currentMoney;

				structureToPlace.ResetStructure();
				structureToPlace.Build();
				
				structureToPlace = null;
			}
			else
				Debug.Log ("Not Enough Money" + currentMoney + "/" + structureToPlace.cost);
        }

        public void LoadTorrentProjectileStructure(GameObject pool)
        {
            for (int i = 0; i < pool.transform.childCount; i++)
            {
				if (!pool.transform.GetChild(i).gameObject.activeInHierarchy)
                {
					pool.transform.GetChild(i).gameObject.SetActive(true);
					structureToPlace = pool.transform.GetChild(i).GetComponent<StructureBase>();
                    structureToPlace.isActive = false;
                    return;
                }
            }

            Debug.Log("Got no structure");
            structureToPlace = null;
        }


        public void SpawnEnemy(GameObject ene)
        {
            int ranNum = Random.Range(0, numOfSpawns);

            Transform spawnPoint = enemySpawnPoints.transform.GetChild(ranNum);
            ene.transform.position = spawnPoint.position;

            ene.SetActive(true);
        }
    }
}
