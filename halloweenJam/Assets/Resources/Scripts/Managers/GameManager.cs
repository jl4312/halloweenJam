using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject torrentPool;

        Camera cam;
        StructureBase structureToPlace;
        GameObject enemySpawnPoints;
        GameObject ground;
        Vector3 distToGround;
        Vector3 mousePosWorld;
        Vector3 pointOnGround;
        float distToGroundMag;
        int numOfSpawns;

        // Use this for initialization
        void Start()
        {
            cam = mainCam.GetComponent<Camera>();
            enemySpawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints");
            ground = GameObject.FindGameObjectWithTag("Ground");

            distToGround = ground.transform.position - mainCam.transform.position;
            distToGroundMag = distToGround.magnitude;
            numOfSpawns = enemySpawnPoints.transform.childCount;
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
            structureToPlace.Build();

            structureToPlace = null;
        }
        public void LoadTorrentStructure()
        {
            for (int i = 0; i < torrentPool.transform.childCount; i++)
            {
                if (!torrentPool.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    torrentPool.transform.GetChild(i).gameObject.SetActive(true);
                    structureToPlace = torrentPool.transform.GetChild(i).GetComponent<StructureBase>();
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
