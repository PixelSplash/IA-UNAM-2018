using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comportamiento : MonoBehaviour
{
    public GameObject bateryImage;
    [SerializeField]
    private float batery;
    public float bateryUseRatio;
    private int state;
    public GameObject markvisited;
    public GameObject markfree;
    public GameObject markwall;
    public float vel_rotacion;
    public float vel_movimiento;
    public int mapSize;
    int[][] map;
    private int[][] pathfinding;
    private int[][] visitados;
    int nx, nz, ax, az;

    Actuadores _actuadores;
    Sensores _sensores;

    private bool hay_camino;
    private int pathcount;
    public float normalizacionDePosicion;
    private bool bateriaBaja;


    // Use this for initialization
    void Start()
    {
        bateriaBaja = false;
        batery = 100;
        pathcount = 1;
        state = 1;
        nx = nz = ax = az = 0;
        hay_camino = false;
        map = new int[mapSize][];
        pathfinding = new int[mapSize][];
        visitados = new int[mapSize][];
        for (int i = 0; i < mapSize; i++)
        {
            map[i] = new int[mapSize];
            pathfinding[i] = new int[mapSize];
            visitados[i] = new int[mapSize];
            for (int j = 0; j < mapSize; j++)
            {
                map[i][j] = -1;
                pathfinding[i][j] = 0;
                visitados[i][j] = 0;
            }
        }
        map[1][0] = -2;

        _actuadores = GetComponent<Actuadores>();
        _sensores = GetComponent<Sensores>();



    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuscarBase();
        }
        print(state);
        batery -= bateryUseRatio;
        if (batery < 50) bateriaBaja = true;
        bateryImage.transform.localScale = new Vector3((batery/100.0f)*3, bateryImage.transform.localScale.y, bateryImage.transform.localScale.z);

    }

    internal void BuscarBase()
    {
        state = 3;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                pathfinding[i][j] = 0;
                visitados[i][j] = 0;
            }
        }

        int actualx, actualz;
        actualx = Mathf.RoundToInt(transform.position.x);
        actualz = Mathf.RoundToInt(transform.position.z);
        bool res = PathFinding(1,1, actualx, actualz, 1);

    }

    internal bool PathFinding(int x, int z, int fx, int fz, int count)
    {
        if (x == fx && z == fz)
        {
            pathfinding[x][z] = count;
            return true;
        }
        if (x < 0 || z < 0 || x >= mapSize || z >= mapSize) return false;
        if (visitados[x][z] == 1 || map[x][z] <= 0) return false;
        
        visitados[x][z] = 1;
        bool res = false;
        if (PathFinding(x + 1, z, fx, fz, count + 1)) res = true;
        if (PathFinding(x - 1, z, fx, fz, count + 1)) res = true;
        if (PathFinding(x, z + 1, fx, fz, count + 1)) res = true;
        if (PathFinding(x, z - 1, fx, fz, count + 1)) res = true;
        if (res)
        {
            pathfinding[x][z] = count;
        }
        return res;
    }

    //Funcion ejecutada cuando los sensores persiben que se estanco el agente
    internal void MeEstanque()
    {
        pathcount += 1;
        int x = Mathf.RoundToInt(transform.position.x);
        int z = Mathf.RoundToInt(transform.position.z);
        if(state == 1)state = 2;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Caminando por espacios no visitados
        if (state == 1)
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
                    if (bateriaBaja) BuscarBase();
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
            
        //Caminando por espacios ya visitados, buscando por donde no ha estado
        else if(state == 2)
        {

            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * normalizacionDePosicion);
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * normalizacionDePosicion);

            int rauxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);
            int rauxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);
            int lauxnx = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);
            int lauxnz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);

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
                    if (bateriaBaja) BuscarBase();
                    //map[ax][az] = pathcount + 2;
                    map[ax][az] = pathcount;
                    ax = auxax;
                    az = auxaz;

                    if(map[rauxnx][rauxnz] == 0 || map[lauxnx][lauxnz] == 0 || map[auxnx][auxnz] == 0)
                    {
                        transform.position = new Vector3(ax, transform.position.y, az);
                        state = 1;
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
        else if (state == 3)
        {

            int auxnx = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * normalizacionDePosicion);
            int auxnz = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * normalizacionDePosicion);

            if (nx != auxnx || nz != auxnz)
            {
                nx = auxnx;
                nz = auxnz;
            }

            if ((pathfinding[nx][nz] <= pathfinding[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] && pathfinding[nx][nz]>= 1) && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5.0f || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {
                
                int auxax = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.forward.x) * 0.4f);
                int auxaz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.forward.z) * 0.4f);


                if (Mathf.RoundToInt(transform.position.x) == 1 && Mathf.RoundToInt(transform.position.z) == 1)
                {
                    batery = 100;
                    state = 1;
                    bateriaBaja = false;
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
