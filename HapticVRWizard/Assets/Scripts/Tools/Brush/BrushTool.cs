using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrushTool : MonoBehaviour, ITool
{
    public GameObject _brush;
    public BrushDraw _preview;
    // Should this be static?
    public List<GameObject> _allBrushs = new List<GameObject>();

    // Maybe reset tube? or pass color info
    public void StartStroke(Transform parent, Material mat)
    {
        _preview.transform.SetParent(parent, false);
        _preview.GetComponent<Renderer>().material = mat;
    }

    public void UpdateStroke(Vector3 point, Quaternion rotation, float scale)
    {
        _preview.AddPoint(point, rotation, scale);
    }

    public void AddBrush(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent, Material mat)
    {
        GameObject newBrush = (GameObject)Instantiate(_brush);
        newBrush.GetComponent<BrushDraw>().GenerateFrom(verts, tris, uvs);
        newBrush.GetComponent<BrushDraw>().Id = id;
        newBrush.transform.SetParent(parent, false);
        newBrush.GetComponent<Renderer>().material = mat;
        _allBrushs.Add(newBrush);
    }

    public void RemoveBrush(string id)
    {
        GameObject delBrush = _allBrushs.Find(t => t.GetComponent<BrushDraw>().Id == id);
        _allBrushs.Remove(delBrush);
        Destroy(delBrush);
    }

    public ICommand EndStroke(Transform parent, Material mat)
    {
        string BrushId = System.Guid.NewGuid().ToString();
        // Reset Preview and Pass it's data to the new tube
        BrushCommand rc = new BrushCommand(BrushId, this, _preview.Vertices, _preview.Tris, _preview.Uvs, parent, mat);
        _preview.Reset();
        rc.Execute();

        return rc;
    }
}
