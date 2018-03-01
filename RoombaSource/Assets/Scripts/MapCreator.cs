using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    public int mapSize;
    int[][] map;
    public GameObject pared;
    public GameObject basura;
    public int numBasuras;

    // Use this for initialization
    void Start () {
        map = new int[mapSize][];
        for(int i = 0; i< mapSize; i++)
        {
            map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                if (j == 0 || i == 0 || j == mapSize - 1 || i == mapSize - 1) map[i][j] = -1;
                else map[i][j] = Random.Range(-1,20);

                if (map[i][j] == -1 && !((i == 1 || i== 2) && (j == 1 || j == 2)))
                {
                    GameObject aux = Instantiate(pared);
                    aux.transform.position = new Vector3(i*aux.transform.lossyScale.x, 0, j * aux.transform.lossyScale.z);
                }

                if (Random.Range(-1, 20) > 10 && numBasuras > 0)
                {
                    numBasuras--;
                    GameObject aux = Instantiate(basura);
                    aux.transform.position = new Vector3(i, 0, j);
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
