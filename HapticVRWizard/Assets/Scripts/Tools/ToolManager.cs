using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {
 public RibbonTool _ribbonTool;
    public TubeTool _tubeTool;
    public TriangleTool _triangleTool;
    public BrushTool _brushTool;
    private bool _isMidTriangle = false;
    public enum ToolTypes { Ribbon, Tube, Triangle, Brush };

    public static bool isBrushTool;

    public string _defaultMat;
    public string _defaultColor;

    public List<Material> _materials;

    public BrushCursorManager _cursor;

    // Undo Redo Datastructures
    private Stack<ICommand> _undoStack = new Stack<ICommand>();
    private Stack<ICommand> _redoStack = new Stack<ICommand>();

    private ITool _currentTool;
    public bool IsStrokeTool
    {
        get { return (typeof(RibbonTool) == _currentTool.GetType() || typeof(TubeTool) == _currentTool.GetType() || typeof(BrushTool) == _currentTool.GetType()); }
    }

    private string _mat;
    private string _color;

    private Material Mat
    {
        get { return _materials.Find(m => m.name == (_mat + _color)); }
    }


    // Use this for initialization
    void Start()
    {
        // Initial Tool Choice, Probs want to display this in the UI somehow
        _currentTool = _tubeTool;
        _mat = _defaultMat;
        _color = _defaultColor;
        // This should probably be set by a material manager
        _cursor.SetCursorMat(Mat);
    }

    public void SetMaterial(string newMat)
    {
        // Handle for when material does not exist
        _mat = newMat;
        _cursor.SetCursorMat(Mat);
    }

    public void SetColor(string newColor)
    {
        // Handle for when material does not exist
        _color = newColor;
        _cursor.SetCursorMat(Mat);
    }

    public Material GetLoadedMat(string mat)
    {
        return _materials.Find(m => m.name == mat);
    }

    // Make the brush ID an enum or something so this ain't hard codeds
    public void SetBrush(int brushId)
    {
        SetBrush((ToolTypes)brushId);
    }

    public void SetBrush(ToolTypes brushId)
    {
        isBrushTool = false;
        if ((ToolTypes)brushId == ToolTypes.Tube)
        {
            _currentTool = _tubeTool;
        }
        else if ((ToolTypes)brushId == ToolTypes.Ribbon)
        {
            _currentTool = _ribbonTool;
        }
        else if ((ToolTypes)brushId == ToolTypes.Triangle)
        {
            _currentTool = _triangleTool;
        }
        else if ((ToolTypes)brushId == ToolTypes.Brush)
        {
            _currentTool = _brushTool;
            isBrushTool = true;
        }

        _isMidTriangle = false;
    }

    public void StartStroke(Transform parent)
    {
        if (!_isMidTriangle)
        {
            _currentTool.StartStroke(parent, Mat);
            _isMidTriangle = !IsStrokeTool;
        }
    }

    public void EndStroke(Transform parent)
    {
        if (!_isMidTriangle)
        {
            // Save command once the stroke has been completed for undo/redo
            ICommand command = (ICommand)_currentTool.EndStroke(parent, Mat);
            _undoStack.Push(command);
            _redoStack.Clear();
        }
        else
        {
            _triangleTool.StartNewTri();
        }
    }

    public void EndTriangle(Transform parent)
    {
        if (_isMidTriangle)
        {
            // Save command once the stroke has been completed for undo/redo
            ICommand command = (ICommand)_currentTool.EndStroke(parent, Mat);
            _undoStack.Push(command);
            _redoStack.Clear();
            _isMidTriangle = false;
        }
    }

    public void UpdateStroke(Vector3 pos, Quaternion rot, float r)
    {
        // Maybe have this do something else if it's not drawing triangles
        _currentTool.UpdateStroke(pos, rot, r);
    }

    public void Undo()
    {
        // Only run if there are commands to Undo
        if (_undoStack.Count > 0)
        {
            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
    }

    public void Redo()
    {
        // Only run if there are commands to Redo
        if (_redoStack.Count > 0)
        {
            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}
