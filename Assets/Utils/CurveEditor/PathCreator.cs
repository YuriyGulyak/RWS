using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public Path path = null;
    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = 0.1f;
    public float controlDiameter = 0.05f;
    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new Path( transform.position );
    }

    void Reset()
    {
        CreatePath();
    }
}