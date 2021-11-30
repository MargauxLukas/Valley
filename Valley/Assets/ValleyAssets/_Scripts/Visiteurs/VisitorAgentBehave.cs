using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorAgentBehave : MonoBehaviour
{
    [Header("Dev datas")]
    [SerializeField] private VisitorData datas = new VisitorData();

    private PathPoint spawnPoint;
    private bool isWalking;

    public Vector2 GetPosition => new Vector2(transform.position.x, transform.position.z);

    public NavMeshAgent Agent => datas.agent;

    public VisitorData Data => datas;

    public ValleyArea currentArea;

    private bool doesAlreadyInteract;

    // Update is called once per frame
    void Update()
    {
        if (datas.agent.enabled && !datas.agent.pathPending && isWalking)
        {
            if (datas.agent.remainingDistance <= datas.agent.stoppingDistance)
            {
                if (!datas.agent.hasPath || datas.agent.velocity.sqrMagnitude == 0f) //CODE REVIEW : Voir pour simplifier les "if" et les rassembler
                {
                    ReachDestination();
                }
            }
        }
    }

    public void SetVisitor(PathPoint nSpawnPoint)
    {
        Valley_PathData wantedPath = VisitorManager.ChoosePath(nSpawnPoint);

        if (wantedPath != null)
        {
            spawnPoint = nSpawnPoint;

            datas.path = wantedPath;
            datas.lastPoint = null;
            datas.currentPoint = nSpawnPoint;

            transform.position = nSpawnPoint.Position;

            gameObject.SetActive(true);


            AskToWalk();

            if (datas.currentPoint == nSpawnPoint)
            {
                datas.currentPoint = nSpawnPoint.GetNextPathPoint(datas.lastPoint, datas.path);
                AskToWalk();
            }

            datas.currentPoint.OnPointDestroyed += UnsetVisitor;
        }
    }

    public void UnsetVisitor()
    {
        datas.currentPoint = null;

        gameObject.SetActive(false);
    }

    #region Deplacement
    private void AskToWalk()
    {
        if (enabled)
        {
            isWalking = VisitorManager.ChooseNextDestination(datas);
            if (isWalking)
            {
                datas.lastPoint.OnPointDestroyed -= UnsetVisitor;
                datas.currentPoint.OnPointDestroyed += UnsetVisitor;
            }
        }
    }

    private void ReachDestination()
    {
        isWalking = false;
        if (datas.currentPoint == spawnPoint)
        {
            VisitorManager.RemoveVisitor(this);
        }
        else
        {
            VisitorManager.MakeVisitorWait(Time.deltaTime, AskToWalk);
        }
    }
    #endregion

    #region Interactions
    public bool CanInteract => !doesAlreadyInteract;

    public void AskInteraction(InterestPoint point)
    {
        Debug.Log("Interact");
        if(point.IsUsable() && CanInteract)
        {
            StartInteraction();
            point.MakeVisitorInteract(this);
        }
    }

    public void StartInteraction()
    {
        if(!doesAlreadyInteract)
        {
            doesAlreadyInteract = true;
            datas.agent.isStopped = true;
            datas.agent.enabled = false;
        }
    }

    public void EndInteraction()
    {
        doesAlreadyInteract = false;
        datas.agent.enabled = true;
        datas.agent.isStopped = false;
    }
    #endregion

    private void OnDisable()
    {
        if (datas.currentPoint != null)
        {
            datas.currentPoint.OnPointDestroyed -= UnsetVisitor;
        }
    }

    private void OnDestroy()
    {
        if(datas.currentPoint != null)
        {
            datas.currentPoint.OnPointDestroyed -= UnsetVisitor;
        }
    }

    public void SetObstacle(bool isObstacle)
    {
        datas.agent.enabled = !isObstacle;
        datas.obstacle.enabled = isObstacle;
    }
}
