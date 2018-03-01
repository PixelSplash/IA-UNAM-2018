using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuadores : MonoBehaviour {
    Rigidbody _rig;

    void Start () {
        transform.eulerAngles = new Vector3(0, 0, 0);
        _rig = transform.GetComponent<Rigidbody>();
    }
	
    //Avanza hacia el frende del agente
    public void Avanzar(float vel_movimiento)
    {
        _rig.angularVelocity = Vector3.up * 0;
        _rig.velocity = transform.forward * vel_movimiento;
    }

    //Rota hacia la derecha
    public void RotarDerecha(float vel_rotacion)
    {
        _rig.velocity = Vector3.forward * 0;
        _rig.AddTorque(transform.up * -vel_rotacion);
    }

    //Rota hacia la izquierda
    public void RotarIzquierda(float vel_rotacion)
    {
        _rig.velocity = Vector3.forward * 0;
        _rig.angularVelocity = transform.up * vel_rotacion;
    }

    //Funcion que arregla la posicion del agente, lo pone en un angulo modulo 90 y lo coloca en el centro de la casilla
    internal void Alinear()
    {
        float angle = transform.eulerAngles.y;
        if (angle % 90.0f > 45) transform.eulerAngles = new Vector3(0,angle + (90 - (angle % 90.0f)), 0);
        else transform.eulerAngles = new Vector3(0, angle - (angle % 90.0f), 0);
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),transform.position.y, Mathf.RoundToInt(transform.position.z));

    }
}
