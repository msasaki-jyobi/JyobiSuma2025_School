using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineData", menuName = "LineCast / LineData", order = 0)]
public class LineData : ScriptableObject
{
    public List<Vector3> StartPosition;
    public List<Vector3> EndPosition;
    public LayerMask LineLayer;
    public Color LineColor = Color.red;
}