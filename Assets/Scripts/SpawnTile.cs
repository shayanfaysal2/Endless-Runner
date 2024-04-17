using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnTile : MonoBehaviour
{
    public Transform player;
    private Vector3 endPos;
    public GameObject[] prefabs;
    public GameObject startPrefab;
    public float playerDistance;

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        prefabs = GameManager.instance.prefabs;
        SpawnPrefab(startPrefab.transform.Find("EndPos").position);
    }

    // Update is called once per frame
    void Update()
    {
        //spawn prefab only if player within certain distance of latest end position
        if (Vector3.Distance(player.position, endPos) < playerDistance && !gameOver)
        {
            SpawnPrefab(endPos);
        }
    }

    void SpawnPrefab(Vector3 pos)
    {
        //spawn a random prefab
        GameObject newPrefab = Instantiate(prefabs[Random.Range(0, prefabs.Length)], pos, Quaternion.identity);

        //genrate two random x positions (-6, 0, or 6) that are not the same
        int x1 = GetRandomXPos();
        int x2;
        do
        {
            x2 = GetRandomXPos();
        }
        while (x2 == x1);

        //coins
        Transform new3Coins = newPrefab.transform.Find("3Coins");
        if (new3Coins != null)
        {
            //put the coins on one of the x positions (lanes)
            new3Coins.position += new Vector3(x1, 0, 0);

            //50% chance to disable coins
            int rand = Random.Range(0, 2);
            if (rand == 0)
                new3Coins.gameObject.SetActive(false);
        }

        //obstacle
        Transform newObstacle = newPrefab.transform.Find("Obstacle");
        if (newObstacle != null)
        {
            //put the obstacle on the other x positions (lanes)
            newObstacle.position += new Vector3(x2, 0, 0);
        }

        //update the end position
        endPos = newPrefab.transform.Find("EndPos").position;
    }

    int GetRandomXPos()
    {
        int a = Random.Range(0, 3);
        if (a == 0)
            return -6;
        else if (a == 1)
            return 6;
        else
            return 0;
    }
}