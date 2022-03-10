using System;
using UnityEngine;

public class MovingData
{
    public Vector3 startPostion { get; set; }
    public Vector3 endPosition { get; set; }
    public float speed { get; set; }
    public Func<bool> endFunc { get; set; }
}