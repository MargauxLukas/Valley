using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Valley_PathData
{
    public List<PathPoint> pathPoints = new List<PathPoint>(); // Liste des points du chemins.
    public Color colorPath;

    /// <summary>
    /// V�rifie si le chemin poss�de le point.
    /// </summary>
    /// <param name="toCheck">Le point � v�rifier.</param>
    /// <returns>Renvoi TRUE si le chemin poss�de le point. Sinon, renvoi FALSE.</returns>
    public bool ContainsPoint(PathPoint toCheck)
    {
        return pathPoints.Contains(toCheck);
    }

    public bool IsUsable(PathPoint toCheck)
    {
        return pathPoints.Count > 1 && pathPoints.Contains(toCheck);
    }
}
