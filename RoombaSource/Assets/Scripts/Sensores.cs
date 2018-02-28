using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensores : MonoBehaviour {
    Comportamiento _comportamiento;
    float _tiempo_quieto;
    Vector3 pos;
    public float tiempo_estancado;
    public float rango_de_vision;

    // Use this for initialization
    void Start () {
        pos = transform.position;
        _tiempo_quieto = 0;
        _comportamiento = GetComponent<Comportamiento>();
    }
	
	// Update is called once per frame
	void Update () {
		if(pos == transform.position)
        {
            _tiempo_quieto++;
        }
        else
        {
            
           _tiempo_quieto = 0;
           pos = transform.position;
        }
        if(_tiempo_quieto > tiempo_estancado)
        {
            _tiempo_quieto = 0;
            _comportamiento.MeEstanque();
        }
	}
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pared") _comportamiento.hay_pared++;
        if (other.tag == "Basura") Destroy(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Pared") _comportamiento.hay_pared--;
    }
    */
    public void VerLados()
    {
        RaycastHit hit;
        Vector3 aux;

        //Sensor de proximidad derecho

        aux = transform.position + transform.right * rango_de_vision;// * 3;
        if (Physics.Raycast(transform.position + transform.up, transform.right, out hit, rango_de_vision))
        {
            
            if(hit.transform.tag == "Pared")
            {
                //print(hit.point);
                _comportamiento.ViPared(aux);
            }
            
        }
        else
        {
            _comportamiento.ViEspacioVacio(aux);
        }

        //Sensor de proximidad izquierdo

        aux = transform.position - transform.right * rango_de_vision;// * 3;
        if (Physics.Raycast(transform.position + transform.up, -transform.right, out hit, rango_de_vision))
        {
            
            if (hit.transform.tag == "Pared")
            {
                //print(hit.point);
                _comportamiento.ViPared(aux);
            }
            
        }
        else
        {
            _comportamiento.ViEspacioVacio(aux);
        }

        //Sensor de proximidad frontal

        aux = transform.position + transform.forward * rango_de_vision;
        if (Physics.Raycast(transform.position + transform.up, transform.forward, out hit, rango_de_vision))
        {

            if (hit.transform.tag == "Pared")
            {
                //print(hit.point);
                _comportamiento.ViPared(aux);
            }

        }
        else
        {
            //_comportamiento.ViEspacioVacio(aux);
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        
        Vector3 aux;
        aux = transform.position - transform.right * rango_de_vision * 3;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, aux);

        aux = transform.position + transform.right * rango_de_vision * 3;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, aux);

        aux = transform.position + transform.forward * rango_de_vision;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, aux);
    }
}
