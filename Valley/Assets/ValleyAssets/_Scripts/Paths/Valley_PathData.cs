using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Valley_PathData
{
    //public List<PathPoint> pathPoints = new List<PathPoint>(); // Liste des points du chemins.
    public PathPoint startPoint;
    public List<PathFragmentData> pathFragment = new List<PathFragmentData>();
    public Color colorPath;

    /// <summary>
    /// V�rifie si le chemin poss�de le point.
    /// </summary>
    /// <param name="toCheck">Le point � v�rifier.</param>
    /// <returns>Renvoi TRUE si le chemin poss�de le point. Sinon, renvoi FALSE.</returns>
    public bool ContainsPoint(PathPoint toCheck)
    {
        for(int i = 0; i < pathFragment.Count; i++)
        {
            if(pathFragment[i].lastPoint == toCheck)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsUsable(PathPoint toCheck)
    {
        return startPoint != null && ContainsPoint(toCheck);
    }

    public PathFragmentData GetRandomDestination(PathPoint currentPoint, PathPoint lastPoint)
    {
        List<PathFragmentData> availablePaths = new List<PathFragmentData>();
        for(int i = 0; i < pathFragment.Count; i++)
        {
            if((pathFragment[i].startPoint == currentPoint && pathFragment[i].lastPoint != lastPoint))
            {
                availablePaths.Add(new PathFragmentData(pathFragment[i].startPoint, pathFragment[i].lastPoint, pathFragment[i].path));
            }
            else if ((pathFragment[i].startPoint != lastPoint && pathFragment[i].lastPoint == currentPoint))
            {
                availablePaths.Add(new PathFragmentData(pathFragment[i].lastPoint, pathFragment[i].startPoint, pathFragment[i].GetReversePath()));
            }
        }

        return availablePaths[UnityEngine.Random.Range(0, availablePaths.Count)];
    }

    public void RemoveFragment(PathPoint endPoint, PathPoint startPoint)
    {
        for(int i = pathFragment.Count-1; i >= 0 ; i--)
        {
            if(pathFragment[i].startPoint == startPoint && pathFragment[i].lastPoint == endPoint)
            {
                pathFragment.RemoveAt(i);
                break;
            }
        }
    }
}
