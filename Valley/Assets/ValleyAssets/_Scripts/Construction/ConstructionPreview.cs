using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ConstructionPreview : MonoBehaviour
{
    [SerializeField] protected Construction realConstruction;

    protected List<GameObject> objectBlockingPose = new List<GameObject>();

    [SerializeField] private MeshRenderer mesh;

    [SerializeField] protected Material availableMaterial;
    [SerializeField] protected Material unavailableMaterial;

    protected bool availabilityState = true;

    public Construction RealConstruction => realConstruction;

    protected abstract bool OnCanPlaceObject(Vector3 position);

    public bool CanPlaceObject(Vector3 position)
    {
        bool toReturn = true;

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(position, out hit, .5f, NavMesh.AllAreas)) //Check si on est sur un terrain praticable
        {
            toReturn = false;
        }

        if(objectBlockingPose.Count > 0)
        {
            toReturn = false;
        }

        return toReturn && OnCanPlaceObject(position);
    }

    public void SpawnObject(Vector3 position)
    {

    }

    public void DespawnObject()
    {
        Destroy(gameObject);
    }

    public void CheckAvailability()
    {
        if(CanPlaceObject(transform.position) != availabilityState)
        {
            availabilityState = CanPlaceObject(transform.position);
            if(availabilityState)
            {
                mesh.material = availableMaterial;
            }
            else
            {
                mesh.material = unavailableMaterial;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NavMeshObstacle>() != null && !objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Remove(other.gameObject);
        }
    }
}
