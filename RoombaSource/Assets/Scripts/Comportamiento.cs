using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comportamiento : MonoBehaviour
{
    public GameObject markvisited;
    public GameObject markfree;
    public GameObject markwall;
    public int hay_pared;
    public float vel_rotacion;
    public float vel_movimiento;
    public float tiempo_entre_pared;
    public int mapSize;
    int[][] map;
    int nx, nz, ax, az;

    Vector3 _pos_inicial;

    Actuadores _actuadores;
    Sensores _sensores;
    
    private bool hay_camino;

    //Sensores _sensores;

    // Use this for initialization
    void Start()
    {
        nx = nz = ax = az = 0;
        hay_camino = false;
        _pos_inicial = transform.position;
        hay_pared = 0;
        map = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                map[i][j] = -1;
                
            }
        }

        _actuadores = GetComponent<Actuadores>();
        _sensores = GetComponent<Sensores>();
        tiempo_entre_pared /= vel_movimiento;
       // StartCoroutine(MarcarPiso());


    }

    internal void MeEstanque()
    {
        print("Estancado");
    }

    private IEnumerator MarcarPiso()
    {

        yield return new WaitForSeconds(tiempo_entre_pared);
        //_actuadores.PonerPared(pared);
        StartCoroutine(MarcarPiso());



    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (hay_pared == 0)
        {
        */
           
            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x)*0.6f);// + mapSize / 2;
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z)*0.6f);// + mapSize / 2;


            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if(map[nx][nz] != 1 && map[nx][nz] != 2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 2 || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 97))
            {
                
                //int ax, az;
                int auxax = Mathf.RoundToInt(transform.position.x /*- Mathf.RoundToInt(transform.forward.x) * 0.5f*/);// + mapSize / 2;
                int auxaz = Mathf.RoundToInt(transform.position.z /*- Mathf.RoundToInt(transform.forward.z) * 0.5f*/);// + mapSize / 2;

                if (ax != auxax || az != auxaz)
                {
                    print("me movi un lugar");
                    _sensores.VerLados();
                    Instantiate(markvisited, new Vector3(auxax, 5, auxaz), Quaternion.identity);
                    map[ax][az] = 1;
                    ax = auxax;
                    az = auxaz;
             
                    
                }
                
                _actuadores.Avanzar(vel_movimiento);
                if (!hay_camino)
                {
                    _actuadores.Alinear();
                    hay_camino = true;
                }
            }
            else
            {
            //print("Auxilio");
            hay_camino = false;
            _actuadores.RotarIzquierda(vel_rotacion);
            }
       /* }
            
        
        else
        {

            hay_camino = false;
            _actuadores.RotarIzquierda(vel_rotacion);
        }*/
    }

    internal void ViEspacioVacio(Vector3 aux)
    {
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);
        Instantiate(markfree, new Vector3(x, 3, z), Quaternion.identity);
        if (map[x][z] == -1)
        {
            print("Espacio");
            print(x);
            print(z);
            map[x][z] = 0;
        }

    }

    internal void ViPared(Vector3 aux)
    {

        
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);
        Instantiate(markwall, new Vector3(x, 5, z), Quaternion.identity);
        if (map[x][z] == -1)
        {
            print("Pared");
            print(x);
            print(z);
            map[x][z] = 2;
        }
    }
}
