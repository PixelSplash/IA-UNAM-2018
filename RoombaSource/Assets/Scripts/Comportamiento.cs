using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comportamiento : MonoBehaviour
{
    private bool searching;
    public GameObject markvisited;
    public GameObject markfree;
    public GameObject markwall;
    public float vel_rotacion;
    public float vel_movimiento;
    public int mapSize;
    int[][] map;
    int nx, nz, ax, az;

    Actuadores _actuadores;
    Sensores _sensores;
    
    private bool hay_camino;
    private int pathcount;
    public float normalizacionDePosicion;


    // Use this for initialization
    void Start()
    {
        pathcount = 1;
        searching = true;
        espacios = new Stack<int[]>();
        camino = new Stack<int[]>();
        nx = nz = ax = az = 0;
        hay_camino = false;
        map = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            map[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                map[i][j] = -1;
            }
        }
        map[1][0] = -2;

        _actuadores = GetComponent<Actuadores>();
        _sensores = GetComponent<Sensores>();
        tiempo_entre_pared /= vel_movimiento;



    }

    internal void MeEstanque()
    {
        print("Me estanque");
        pathcount += 1;
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        searching = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        if (searching)
        {
        
           
            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x)* normalizacionDePosicion);
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z)* normalizacionDePosicion);


            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if(map[nx][nz] <=0 && map[nx][nz] != -2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5 || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {
               
                int auxax = Mathf.RoundToInt(transform.position.x);
                int auxaz = Mathf.RoundToInt(transform.position.z);

                if (ax != auxax || az != auxaz)
                {

                    _sensores.VerLados();
                    Instantiate(markvisited, new Vector3(auxax, 5, auxaz), Quaternion.identity);

                    map[ax][az] = pathcount;
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
            hay_camino = false;
            _actuadores.RotarIzquierda(vel_rotacion);
            }
        }
            
        
        else
        {

            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * normalizacionDePosicion);// + mapSize / 2;
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * normalizacionDePosicion);// + mapSize / 2;

            int rauxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);// + mapSize / 2;
            int rauxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);// + mapSize / 2;
            int lauxnx = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);// + mapSize / 2;
            int lauxnz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);// + mapSize / 2;

            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if (map[nx][nz] < pathcount && map[nx][nz] != -2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5.0f || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {
                _sensores.VerLados();

                int auxax = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.forward.x) * 0.4f);
                int auxaz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.forward.z) * 0.4f);

                if (ax != auxax || az != auxaz)
                {

                    map[ax][az] = pathcount + 2;
                    ax = auxax;
                    az = auxaz;

                    if(map[rauxnx][rauxnz] == 0 || map[lauxnx][lauxnz] == 0 || map[auxnx][auxnz] == 0)
                    {
                        transform.position = new Vector3(ax, transform.position.y, az);
                        searching = true;
                        nx = -1;
                        nz = -1;
                    }


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
 
                hay_camino = false;
                _actuadores.RotarIzquierda(vel_rotacion);
            }
        }
    }

    internal void ViEspacioVacio(Vector3 aux)
    {
        
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);

        if(x >= 0 && x < mapSize && z >= 0 && z < mapSize)
        {

            if (map[x][z] == -1)
            {

                map[x][z] = 0;
            }
        }
        

    }

    internal void ViPared(Vector3 aux)
    {

        
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);
        if (x >= 0 && x < mapSize && z >= 0 && z < mapSize)
        {
            Instantiate(markwall, new Vector3(x, 5, z), Quaternion.identity);
            if (map[x][z] == -1)
            {
                
                map[x][z] = -2;
            }
        }
    }
}
