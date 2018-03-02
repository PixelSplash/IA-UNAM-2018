using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensores : MonoBehaviour {
    Comportamiento _comportamiento;
    private float _tiempoQuieto; // Se utiliza como contador para saber si el agente esta estancado en un mismo sitio
    Vector3 _pos; // Se utiliza para comparar la posicion en cada instante
    public float tiempoEstancado; // Tiempo en el que se considera que se esta estancado
    public float rangoDeVision; // Es la longitud del rayo de los sensores


    // Use this for initialization
    void Start () {
        _pos = transform.position;
        _tiempoQuieto = 0;
        _comportamiento = GetComponent<Comportamiento>();
    }
	
	// Update is called once per frame
	void Update () {

        if (_comportamiento.bateria < 50) _comportamiento.bateriaBaja = true;

        if (_pos == transform.position)
        {
            _tiempoQuieto++; // Esta en el mismo lugar que el frame anterior
        }
        else
        {
           _tiempoQuieto = 0; // Cambio de lugar
           _pos = transform.position;
        }

        if(_tiempoQuieto > tiempoEstancado)
        {
            _tiempoQuieto = 0;
            _comportamiento.MeEstanque(); // Percibe que se atasco, manda mensaje al modulo de comportamiento
        }
	}
 
    public void VerLados()
    {
        RaycastHit hit;
        Vector3 aux;

        // Sensor de proximidad derecho trasero
        aux = transform.position - transform.forward + transform.right * rangoDeVision;
        if (Physics.Raycast(transform.position + transform.up - transform.forward, transform.right, out hit, rangoDeVision))
        {
            
            if(hit.transform.tag == "Pared")
            {
                _comportamiento.ViPared(aux);
            }
            
        }
        else
        {
            _comportamiento.ViEspacioVacio(aux);

            // Sensor de proximidad derecho trasero 2, solo se activa si no hay pared
            aux = transform.position - transform.forward + transform.right * rangoDeVision * 2;
            if (Physics.Raycast(transform.position + transform.up - transform.forward, transform.right, out hit, rangoDeVision * 2))
            {

                if (hit.transform.tag == "Pared")
                {
                    _comportamiento.ViPared(aux);
                }

            }
            else
            {
                _comportamiento.ViEspacioVacio(aux);


            }
        }

        // Sensor de proximidad izquierdo trasero

        aux = transform.position - transform.forward - transform.right * rangoDeVision;
        if (Physics.Raycast(transform.position + transform.up - transform.forward, -transform.right, out hit, rangoDeVision))
        {
            
            if (hit.transform.tag == "Pared")
            {
                _comportamiento.ViPared(aux);
            }
            
        }
        else
        {
            _comportamiento.ViEspacioVacio(aux);

            // Sensor de proximidad izquierdo trasero 2, , solo se activa si no hay pared

            aux = transform.position - transform.forward - transform.right * rangoDeVision * 2;
            if (Physics.Raycast(transform.position + transform.up - transform.forward, -transform.right, out hit, rangoDeVision * 2))
            {

                if (hit.transform.tag == "Pared")
                {
                    _comportamiento.ViPared(aux);
                }

            }
            else
            {
                _comportamiento.ViEspacioVacio(aux);
            }
        }

        // Sensor de proximidad derecho delantero

        aux = transform.position + transform.right * rangoDeVision;
        if (Physics.Raycast(transform.position + transform.up, transform.right, out hit, rangoDeVision))
        {

            if (hit.transform.tag == "Pared")
            {
                _comportamiento.ViPared(aux);
            }

        }

        // Sensor de proximidad izquierdo delantero

        aux = transform.position - transform.right * rangoDeVision;
        if (Physics.Raycast(transform.position + transform.up, -transform.right, out hit, rangoDeVision))
        {

            if (hit.transform.tag == "Pared")
            {
                _comportamiento.ViPared(aux);
            }

        }

        // Sensor de proximidad frontal

        aux = transform.position + transform.forward * rangoDeVision * 1.5f;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, rangoDeVision *1.5f))
        {

            if (hit.transform.tag == "Pared")
            {
                _comportamiento.ViPared(aux);
            }

        }
        
    }

    // Limpia la basura que encuentra
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Basura")
        {
            Destroy(other.gameObject);
        }
    }
}
