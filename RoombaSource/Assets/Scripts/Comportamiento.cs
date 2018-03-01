using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comportamiento : MonoBehaviour
{

    Stack<int[]> camino;
    private bool searching;
    Stack<int[]> espacios;
    public GameObject markvisited;
    public GameObject markfree;
    public GameObject markwall;
    public int hay_pared;
    public float vel_rotacion;
    public float vel_movimiento;
    public float tiempo_entre_pared;
    public int mapSize;
    int[][] map;
   // int[][] pathFindingMap;
    int nx, nz, ax, az;
    //int[] ultimoEspacioVacio;

    Vector3 _pos_inicial;

    Actuadores _actuadores;
    Sensores _sensores;
    
    private bool hay_camino;
    private int _xDestino;
    private int _zDestino;
    private int pathcount;
    public float normalizacionDePosicion;
    private int count;

    //Sensores _sensores;

    // Use this for initialization
    void Start()
    {
        pathcount = 1;
        searching = true;
        espacios = new Stack<int[]>();
        camino = new Stack<int[]>();
        //ultimoEspacioVacio = new int[2];
        nx = nz = ax = az = 0;
        hay_camino = false;
        _pos_inicial = transform.position;
        hay_pared = 0;
        map = new int[mapSize][];
       // pathFindingMap = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            map[i] = new int[mapSize];
           // pathFindingMap[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
               // if((i != 0 && i != mapSize - 1) || (j != 0 && j != mapSize - 1)) map[i][j] = - 1;
                //pathFindingMap[i][j] = -1;
               // else map[i][j] = -2;
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
        count = 0;
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
        
           
            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x)* normalizacionDePosicion);// + mapSize / 2;
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z)* normalizacionDePosicion);// + mapSize / 2;


            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if(map[nx][nz] <=0 && map[nx][nz] != -2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5 || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {
                
                //int ax, az;
                int auxax = Mathf.RoundToInt(transform.position.x /*- Mathf.RoundToInt(transform.forward.x) * 0.5f*/);// + mapSize / 2;
                int auxaz = Mathf.RoundToInt(transform.position.z /*- Mathf.RoundToInt(transform.forward.z) * 0.5f*/);// + mapSize / 2;

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
            //print("Auxilio");
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
                //if (count > 1) {
                _sensores.VerLados();
                //}
                //int ax, az;
                int auxax = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.forward.x) * 0.4f);
                int auxaz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.forward.z) * 0.4f);

                if (ax != auxax || az != auxaz)
                {
                    //count++;
                    //map[ax][az] = pathcount;
                    print("marque lugar");
                    map[ax][az] = pathcount;
                    ax = auxax;
                    az = auxaz;

                    if(map[rauxnx][rauxnz] == 0 || map[lauxnx][lauxnz] == 0 || map[auxnx][auxnz] == 0)
                    //if (map[rauxnx][rauxnz] == -1 || map[lauxnx][lauxnz] == -1 || map[auxnx][auxnz] == -1)
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
        //ultimoEspacioVacio[0] = x;
        //ultimoEspacioVacio[1] = z;
        
        if(x >= 0 && x < mapSize && z >= 0 && z < mapSize)
        {
            //Instantiate(markvisited, new Vector3(x, 3, z), Quaternion.identity);
            //Instantiate(markfree, new Vector3(x, 3, z), Quaternion.identity);
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
