using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
    public int tamanoMapa; // El mapa va a ser una matriz cuadrada de 1 metro x 1 metro
    int[][] _mapa;
    public GameObject pared; // Prefab para la pared
    public GameObject basura; // Prefab para la basura

    // Inicializacion del mapa
    void Start () {
        _mapa = new int[tamanoMapa][];
        for(int i = 0; i< tamanoMapa; i++)
        {
            _mapa[i] = new int[tamanoMapa];
            for (int j = 0; j < tamanoMapa; j++)
            {
                if (j == 0 || i == 0 || j == tamanoMapa - 1 || i == tamanoMapa - 1) _mapa[i][j] = -1; // Paredes de los extremos van en -1
                else _mapa[i][j] = Random.Range(-1,20); // Se le asigna una probabilidad para saber si hay pared

                if (_mapa[i][j] == -1 && !((i == 1 || i== 2) && (j == 1 || j == 2))) // Pone pared en los espacios con -1, evitando el area cerca del agente
                {
                    GameObject aux = Instantiate(pared);
                    aux.transform.position = new Vector3(i*aux.transform.lossyScale.x, 0, j * aux.transform.lossyScale.z);
                }

                // Random para poner basura
                if (Random.Range(-1, 20) > 10)
                {
                    GameObject aux = Instantiate(basura);
                    aux.transform.position = new Vector3(i, 0, j);
                }
            }
        }
	}
}
