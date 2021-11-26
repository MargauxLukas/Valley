using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValleyArea
{
    public List<Transform> bordersTransform;
    public List<Vector2> borders;

    public List<VisitorAgentBehave> visitorInZone;

    public List<Vector2> GetBorders => borders;
}
