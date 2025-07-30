using System.Collections.Generic;
using Project.Core.Scripts;
using UnityEngine;

public class PaintManager : SingletonBehaviour<PaintManager>
{
    public List<Vector2> CurrentPoint { get; set; }
}
