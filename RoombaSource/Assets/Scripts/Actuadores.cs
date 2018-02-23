using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actuadores : MonoBehaviour {
    Rigidbody rig;

    // Use this for initialization
    void Start () {
        transform.eulerAngles = new Vector3(0, 0, 0);
        rig = transform.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Avanzar(float vel_movimiento)
    {
        rig.angularVelocity = Vector3.up * 0;
        rig.velocity = transform.forward * vel_movimiento;
    }

    public void RotarDerecha(float vel_rotacion)
    {
        rig.velocity = Vector3.forward * 0;
        rig.AddTorque(transform.up * -vel_rotacion);
    }

    public void RotarIzquierda(float vel_rotacion)
    {
        rig.velocity = Vector3.forward * 0;
        rig.angularVelocity = transform.up * vel_rotacion;
    }
    /*
    public void PonerPared(GameObject pared)
    {
        GameObject aux = Instantiate(pared);
        aux.transform.position = transform.position;
        aux.transform.Translate(transform.forward * -1.1f);
        aux.transform.rotation = transform.rotation;
    }
    */
    internal void Alinear()
    {
        float angle = transform.eulerAngles.y;
       // print("Antes");
       // print(transform.eulerAngles.y);
        if (angle % 90.0f > 45) transform.eulerAngles = new Vector3(0,angle + (90 - (angle % 90.0f)), 0);
        else transform.eulerAngles = new Vector3(0, angle - (angle % 90.0f), 0);
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),transform.position.y, Mathf.RoundToInt(transform.position.z));
      //  print("Despues");
      //  print(transform.eulerAngles.y);

    }
}
