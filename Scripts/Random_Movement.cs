using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Random_Movement : MonoBehaviour
{
    public NavMeshAgent agent; // Necesito un NavMeshAgent
    public float range; // Radio de la esfera en la que se moverá el agente

    public Transform centerPoint; //Centro del área en la que se moverá el agente
    // Este puede ser un transform del agente en vez del centro del área si no te interesa que patrulle algo en específico

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Instanciar al agente
    }

    void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) // Termina de recorrer el camino
        {
            Vector3 point; // Se crea un nuevo punto vacío
            if (RandomPoint(centerPoint.position, range, out point)) // Se le da el centro y el rango y nos regresa unas coordenadas en el área correspondiente
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // Usamos el RayCast para ver a dónde se moverá el agente. El rayo lo lanzaos hacia arriba.
                agent.SetDestination(point); // Le decimos al agente que su próximo destino sean las nuevas coordenadas que le dimos.
            }
            // Solo camina a un nuevi punto si randomPoint fue true.
        }

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) // Bool porque nos dice si encontró un punto válido o no
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; // Elige un punto aleatorio dentro la esfera
        NavMeshHit hit; 
        if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas)) // Documentación: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            // En este caso, buscamos en un radio de 5 alrededor del punto aleatorio a ver si hay un área transitable (NavMesh.AllAreas)
            // El X.0F es la distancia máxima del punto aleatorio a un punto en el NavMesh
            // También se puede agregar un for
            result = hit.position; // Guardamos la posición válida y la regresamos
            return true; // Regresamos que sí encontramos un punto válido
        }
        result = Vector3.zero; // Ponemos un Vector en (0, 0, 0) si no encontramos un punto válido, es solo como placeholder
        return false; // Decimos que no encontramos un punto, y en el siguiente Update buscamos un nuevo punto
    }
}

// Tutorial de JonDevTutorials en: https://www.youtube.com/watch?v=dYs0WRzzoRc
// Código de JonDevTutoials en: https://github.com/JonDevTutorial/RandomNavMeshMovement