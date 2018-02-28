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
                map[i][j] = - 1;
                //pathFindingMap[i][j] = -1;


            }
        }
        map[1][0] = -2;

        _actuadores = GetComponent<Actuadores>();
        _sensores = GetComponent<Sensores>();
        tiempo_entre_pared /= vel_movimiento;



    }

    internal void MeEstanque()
    {
        pathcount += 1;
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        map[x][z] = pathcount;
        //print("me Estanque");
        //print(x);
        //print(z);
        /*
        int range = 50;
        bool encontrado = false;
        for(int i = 1; i < range; i++)
        {
            if (encontrado) break;

            for (int j = -i; j <= i; i++)
            {
                if (encontrado) break;



                if (x+i < mapSize && z + j > -1 && z + j < mapSize)
                {
                    if(map[x + i][z + j] == 0)
                    {
                        x = x + i;
                        z = z + j;
                        encontrado = true;
                    }
                    
                }
                if (x - i > -1 && z + j > -1 && z + j < mapSize)
                {
                    if (map[x - i][z + j] == 0) {
                        x = x - i;
                        z = z + j;
                        encontrado = true;
                    }
                }
                if (z + i < mapSize && x + j > -1 && x + j < mapSize && map[x + j][z + i] == 0)
                {
                    if (map[x - i][z + j] == 0)
                    {
                        x = x + j;
                        z = z + i;
                        encontrado = true;
                    }
                }
                if (z - i > -1 && x + j > -1 && x + j < mapSize && map[x + j][z - i] == 0)
                {
                    if (map[x - i][z + j] == 0)
                    {
                        x = x + j;
                        z = z - i;
                        encontrado = true;
                    }
                }
            }
        }
        if (encontrado)
        {
            print("Encontre camino");
            print(x);
            print(z);

            _xDestino = x;
            _zDestino = z;
            Pathfinding();
        }
        else print("Estancado");*/
        //Pathfinding();
        searching = false;
    }

    //Path Finding
    /*
    public void Pathfinding()
    {
        int x, z;
        x = Mathf.RoundToInt(transform.position.x);
        z = Mathf.RoundToInt(transform.position.z);

        //int[] aux = { x, z };
        //camino.Push(aux);
        int cam = AuxPathfinding(x,z);
        print("El camino mas corto mide");
        print(cam);
        
    }

    private int AuxPathfinding(int x,int z)
    {
        
        
        if (x < 0 || x > mapSize || z < 0 || z > mapSize || (map[x][z] != 1 && map[x][z] != 0) || pathFindingMap[x][z] == 1) return 1000;
        pathFindingMap[x][z] = 1;
        //print("pase por");
        //print(x);
        //print(z);
        if (map[x][z] == 0) return 1;
        int ret = 10000000;
        int[] min = new int[2];
       // min[0] = -1;
       // min[1] = -1;

        int aux = 1 + AuxPathfinding(x + 1, z);
        if (aux < ret)
        {
            ret = aux;
            min[0] = x + 1;
            min[1] = z;
        }
        aux = 1 + AuxPathfinding(x - 1, z);
        if (aux < ret)
        {
            ret = aux;
            min[0] = x - 1;
            min[1] = z;
        }
        aux = 1 + AuxPathfinding(x , z+1);
        if (aux < ret)
        {
            ret = aux;
            min[0] = x;
            min[1] = z + 1;
        }
        aux = 1 + AuxPathfinding(x , z-1);
        if (aux < ret)
        {
            ret = aux;
            min[0] = x;
            min[1] = z - 1;
        }

        //camino.Push(min);
        return ret;
        
    }
    */
    // Update is called once per frame
    void FixedUpdate()
    {

        
        if (searching)
        {
        
           
            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x)*0.6f);// + mapSize / 2;
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z)*0.6f);// + mapSize / 2;


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

            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * 0.6f);// + mapSize / 2;
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * 0.6f);// + mapSize / 2;

            int rauxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.right.x) * 0.6f);// + mapSize / 2;
            int rauxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.right.z) * 0.6f);// + mapSize / 2;
            int lauxnx = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.right.x) * 0.6f);// + mapSize / 2;
            int lauxnz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.right.z) * 0.6f);// + mapSize / 2;

            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if (map[nx][nz] < pathcount && map[nx][nz] != -2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 2 || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 97))
            {

                //int ax, az;
                int auxax = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.forward.x) * 0.3f);
                int auxaz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.forward.z) * 0.3f);

                if (ax != auxax || az != auxaz)
                {

                    map[ax][az] += pathcount;
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
        //ultimoEspacioVacio[0] = x;
        //ultimoEspacioVacio[1] = z;
        
        Instantiate(markfree, new Vector3(x, 3, z), Quaternion.identity);
        if (map[x][z] == -1)
        {
            //print("Espacio");
            //print(x);
            //print(z);
            //int[] espacio = new int[2];
            //espacio[0] = x;
            //espacio[1] = z;
            //print("Ultimo espacio que vi");
            //print(x);
            //print(z);
            //espacios.Push(espacio);

            camino.Clear();
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
            //print("Pared");
            //print(x);
            //print(z);
            map[x][z] = -2;
        }
    }
}
