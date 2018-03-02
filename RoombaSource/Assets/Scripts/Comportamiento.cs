using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Comportamiento : MonoBehaviour
{
    public GameObject imagenBateria; // Imagen que representa en pantalla la energia actual
    public float bateria; // Energia almacenada en la bateria
    public float rapidezDeUsoBateria;
    private int _estado; // Estado de busqueda libre o estado de busqueda direccionada
    public GameObject marcaVisitado; // Prefab para marcar espacios ya visitados
    public GameObject marcaPared; // Prefab para marcar paredes
    public float velocidadRotacion;
    public float velocidadMovimiento;
    public int tamanoMapa;
    private int[][] _mapa; // Matriz que representa el escenario, -1 espacio no visitado, -2 pared, 0 espacio vacio, 1+ espacio ya visitado
    private int[][] _pathfinding; // Matriz que guarda el camino descubierto por el algoritmo de pathfinding
    private int[][] _visitados; // Matriz para marcar espacios ya visitados en el algoritmo de pathfinding
    private bool _hayCamino; // Variable utilizada para saber el primer momento que se encuentra un camino, esto para alinear al agente
    private int _marcadorVisitados; // numero a utilizar para marcar espacios visitados
    public float normalizacionDePosicion; // distancia que se agrega al pivote del agente para hacer calculos
    public bool bateriaBaja;
    private int _xObjetivo; // coordenada x del objetivo para el estado de movimiento direccionado
    private int _zObjetivo; // coordenada z del objetivo para el estado de movimiento direccionado
    private bool _loEncontre; // Variable utilizada en el algoritmo de pathfinding
    private Actuadores _actuadores;
    private Sensores _sensores;
    // Variables utilizadas para hacer la transicion de punto en el espacio con representacion en el mapa/ matriz
    private int _xPosicionAdelante, _zPosicionAdelante, ax, az;
    private int _xPosicionDerecha;
    private int _zPosicionDerecha;
    private int _xPosicionIzquierda;
    private int _zPosicionIzquierda;

    // Inicializacion de variables
    void Start()
    {
        Screen.SetResolution(500, 500, true);
        bateriaBaja = false;
        _hayCamino = false;
        bateria = 100;
        _marcadorVisitados = 1;
        _estado = 1; // Estado de busqueda libre
        _xPosicionAdelante = _zPosicionAdelante = ax = az = 0;
        
        _mapa = new int[tamanoMapa][];
        _pathfinding = new int[tamanoMapa][];
        _visitados = new int[tamanoMapa][];

        for (int i = 0; i < tamanoMapa; i++)
        {
            _mapa[i] = new int[tamanoMapa];
            _pathfinding[i] = new int[tamanoMapa];
            _visitados[i] = new int[tamanoMapa];

            for (int j = 0; j < tamanoMapa; j++)
            {
                _mapa[i][j] = -1;
                _pathfinding[i][j] = 0;
                _visitados[i][j] = 0;
            }
        }

        _actuadores = GetComponent<Actuadores>();
        _sensores = GetComponent<Sensores>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Reiniciar nivel
        {
            SceneManager.LoadScene("Escena1");
        }
        if(bateria > rapidezDeUsoBateria)bateria -= rapidezDeUsoBateria; // Utilizar energia
        imagenBateria.transform.localScale = new Vector3((bateria/100.0f)*3, imagenBateria.transform.localScale.y, imagenBateria.transform.localScale.z); // Actualizar escala de la imagen de la energia restante

    }

    internal void BuscarEspacioVacio()
    {
        // Se inicializan en -1 para luego saber si no hay camino
        _xObjetivo = -1;
        _zObjetivo = -1;

        _loEncontre = false; // Se utiliza para parar la busqueda al encontrar el primer camino
        _estado = 2; // Se pasa al estado direccionado

        // Se limpian las matrices necesarias
        for (int i = 0; i < tamanoMapa; i++)
        {
            for (int j = 0; j < tamanoMapa; j++)
            {
                _pathfinding[i][j] = 0;
                _visitados[i][j] = 0;
            }
        }

        // Se marca el espacio en donde se encuentra el agente y se llama a la funcion de pathfinding
        int actualx, actualz;
        actualx = Mathf.RoundToInt(transform.position.x);
        actualz = Mathf.RoundToInt(transform.position.z);
        _mapa[actualx][actualz] = _marcadorVisitados;
        bool res = PathFindingEspacios(actualx, actualz, tamanoMapa*tamanoMapa);

    }

    // Va marcando el camino a seguir con numeros y los va disminuyendo, asi el agente sabe por donde debe de pasar
    internal bool PathFindingEspacios(int x, int z, int count)
    {
        if (_loEncontre) return false; // Si ya se encontro camino para la ejecucion
        if (x < 0 || z < 0 || x >= tamanoMapa || z >= tamanoMapa) return false; // Parametros no validos

        if (_mapa[x][z] == 0) // Encontro espacio vacio
        {
            _loEncontre = true;
            _pathfinding[x][z] = count;
            _xObjetivo = x;
            _zObjetivo = z;
            return true;
        }

        if (_visitados[x][z] == 1 || _mapa[x][z] < 0) return false; // Espacio es una pared o ya se visito

        _visitados[x][z] = 1; // Se marca como visitado

        // Se manda a buscar camino a las 4 direcciones
        bool res = false;
        if (PathFindingEspacios(x + 1, z, count - 1)) res = true;
        if (PathFindingEspacios(x - 1, z, count - 1)) res = true;
        if (PathFindingEspacios(x, z + 1, count - 1)) res = true;
        if (PathFindingEspacios(x, z - 1, count - 1)) res = true;

        if (res) // Si existe algun camino por este espacio, se marca
        {
            _pathfinding[x][z] = count;
        }
        return res;
    }

    internal void BuscarBase()
    {
        _estado = 2; // Se pasa al estado direccionado

        // Se limpian las matrices necesarias
        for (int i = 0; i < tamanoMapa; i++)
        {
            for (int j = 0; j < tamanoMapa; j++)
            {
                _pathfinding[i][j] = 0;
                _visitados[i][j] = 0;
            }
        }

        // Se llama a la funcion de pathfinding
        int actualx, actualz;
        actualx = Mathf.RoundToInt(transform.position.x);
        actualz = Mathf.RoundToInt(transform.position.z);
        bool res = PathFindingBase(1,1, actualx, actualz, 1);

        // se define la base de carga como objetivo
        _xObjetivo = 1;
        _zObjetivo = 1;

    }

    // Va marcando el camino a seguir con numeros y los va aumentando, ya que empieza desde la base, asi el agente sabe por donde debe de pasar
    internal bool PathFindingBase(int x, int z, int fx, int fz, int count)
    {
        if (x == fx && z == fz) // Encontro el objetivo
        {
            _pathfinding[x][z] = count;
            return true;
        }

        if (x < 0 || z < 0 || x >= tamanoMapa || z >= tamanoMapa) return false; // Parametros no validos
        if (_visitados[x][z] == 1 || _mapa[x][z] <= 0) return false; // Espacio es una pared o ya se visito

        _visitados[x][z] = 1; // Se marca como visitado

        // Se manda a buscar camino a las 4 direcciones
        bool res = false;
        if (PathFindingBase(x + 1, z, fx, fz, count + 1)) res = true;
        if (PathFindingBase(x - 1, z, fx, fz, count + 1)) res = true;
        if (PathFindingBase(x, z + 1, fx, fz, count + 1)) res = true;
        if (PathFindingBase(x, z - 1, fx, fz, count + 1)) res = true;

        if (res) // Si existe algun camino por este espacio, se marca
        {
            _pathfinding[x][z] = count;
        }
        return res;
    }

    // Se utiliza Fixed Update para mas precision en los calculos y uso de fisicas
    void FixedUpdate()
    {
        if (Mathf.RoundToInt(transform.position.x) == 1 && Mathf.RoundToInt(transform.position.z) == 1) // Si pasa por la base se recarga instantaneamente
        {
            bateriaBaja = false;
            bateria = 100;
        }


        // Estado de busqueda libre, va avanzando por espacios no visitados
        if (_estado == 1)
        {

            // Calcula la posicion siguiente en indices para la matriz
            _xPosicionAdelante = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * normalizacionDePosicion);
            _zPosicionAdelante = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * normalizacionDePosicion);

            // Si la siguiente posicion esta vacia o es desconocida, siguen adelante, solo entra cuando se esta en angulo modulo 90
            if (_mapa[_xPosicionAdelante][_zPosicionAdelante] <= 0 && _mapa[_xPosicionAdelante][_zPosicionAdelante] != -2 && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5 || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {

                int auxax = Mathf.RoundToInt(transform.position.x);
                int auxaz = Mathf.RoundToInt(transform.position.z);

                // Ya se avanzo de posicion en la matriz
                if (ax != auxax || az != auxaz)
                {
                    if (bateriaBaja) BuscarBase(); // Bateria baja, tiene que ir a recargar
                    _sensores.VerLados();

                    Instantiate(marcaVisitado, new Vector3(auxax, 5, auxaz), Quaternion.identity);
                    _mapa[ax][az] = _marcadorVisitados;
                    ax = auxax;
                    az = auxaz;

                }

                _actuadores.Avanzar(velocidadMovimiento);

                if (!_hayCamino) // Si es el primer frame en el que encontro camino, se alinea, osea arregla su angulo y posicion
                {
                    _actuadores.Alinear();
                    _hayCamino = true;
                }
            }
            else // Si no hay camino al frente, rota
            {
                _hayCamino = false;
                _actuadores.RotarIzquierda(velocidadRotacion);
            }
        }

        //
        else if (_estado == 2)
        {
            if (_xObjetivo == -1 && _zObjetivo == -1)
            {
                BuscarBase();
            }


            _xPosicionAdelante = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.forward.x) * normalizacionDePosicion);
            _zPosicionAdelante = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.forward.z) * normalizacionDePosicion);
            _xPosicionDerecha = Mathf.RoundToInt(transform.position.x + Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);
            _zPosicionDerecha = Mathf.RoundToInt(transform.position.z + Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);
            _xPosicionIzquierda = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.right.x) * normalizacionDePosicion);
            _zPosicionIzquierda = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.right.z) * normalizacionDePosicion);

            if ((_pathfinding[_xPosicionAdelante][_zPosicionAdelante] <= _pathfinding[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] && _pathfinding[_xPosicionAdelante][_zPosicionAdelante] >= 1) && (Mathf.Abs(transform.eulerAngles.y) % 90.0f < 5.0f || Mathf.Abs(transform.eulerAngles.y) % 90.0f > 85.0f))
            {
                _sensores.VerLados();

                int auxax = Mathf.RoundToInt(transform.position.x - Mathf.RoundToInt(transform.forward.x) * 0.4f);
                int auxaz = Mathf.RoundToInt(transform.position.z - Mathf.RoundToInt(transform.forward.z) * 0.4f);


                if (Mathf.RoundToInt(transform.position.x) == _xObjetivo && Mathf.RoundToInt(transform.position.z) == _zObjetivo)
                {

                    _mapa[Mathf.RoundToInt(transform.position.x)][Mathf.RoundToInt(transform.position.z)] = 1;
                    _estado = 1;

                }

                if (ax != auxax || az != auxaz)
                {

                    ax = auxax;
                    az = auxaz;

                    if ((_mapa[_xPosicionDerecha][_zPosicionDerecha] == 0 || _mapa[_xPosicionIzquierda][_zPosicionIzquierda] == 0 || _mapa[_xPosicionAdelante][_zPosicionAdelante] == 0) && _xObjetivo != 1 && _zObjetivo != 1)
                    {
                        transform.position = new Vector3(ax, transform.position.y, az);
                        _estado = 1;
                        _xPosicionAdelante = -1;
                        _zPosicionAdelante = -1;
                    }


                }


                _actuadores.Avanzar(velocidadMovimiento);

                if (!_hayCamino)
                {
                    _actuadores.Alinear();
                    _hayCamino = true;
                }
            }
            else
            {

                _hayCamino = false;
                _actuadores.RotarIzquierda(velocidadRotacion);
            }
        }
    }

    // Funcion ejecutada cuando los sensores persiben que se estanco el agente
    internal void MeEstanque()
    {
        BuscarEspacioVacio();   
    }

    // Marca el espacio vacio en el mapa del escenario, poniendo un 0 en la posicion de este
    internal void ViEspacioVacio(Vector3 aux)
    {
        
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);

        if(x >= 0 && x < tamanoMapa && z >= 0 && z < tamanoMapa)
        {

            if (_mapa[x][z] == -1)
            {

                _mapa[x][z] = 0;
            }
        }
        

    }

    // Marca la pared en el mapa del escenario, poniendo un -1 en la posicion de este e instancia el marcador rojo de pared
    internal void ViPared(Vector3 aux)
    {
        int x, z;
        x = Mathf.RoundToInt(aux.x);
        z = Mathf.RoundToInt(aux.z);
        if (x >= 0 && x < tamanoMapa && z >= 0 && z < tamanoMapa)
        {
            Instantiate(marcaPared, new Vector3(x, 5, z), Quaternion.identity);
            if (_mapa[x][z] == -1)
            {
                _mapa[x][z] = -2;
            }
        }
    }
}
