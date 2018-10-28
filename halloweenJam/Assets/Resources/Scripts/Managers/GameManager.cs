using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisfitMakers
{
    public class GameManager : MonoBehaviour
    {
        GameObject enemySpawnPoints;

        int numOfSpawns;

        // Use this for initialization
        void Start()
        {
            enemySpawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints");

            numOfSpawns = enemySpawnPoints.transform.childCount;
        }

        // Update is called once per frame
        void Update()
        {

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
