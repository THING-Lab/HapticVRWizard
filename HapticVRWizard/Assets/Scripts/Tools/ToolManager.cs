using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {
	public RibbonTool _ribbonTool;
	public TubeTool _tubeTool;

	public string _defaultMat;
	public string _defaultColor;

	public List<Material> _materials;

	public BrushCursorManager _cursor;

	// Undo Redo Datastructures
	private Stack<ICommand> _undoStack = new Stack<ICommand>();
	private Stack<ICommand> _redoStack = new Stack<ICommand>();

	private ITool _currentTool;

	private string _mat;
	private string _color;

	private Material Mat {
		get { return _materials.Find(m => m.name == (_mat + _color)); }
	}


	// Use this for initialization
	void Start () {
		// Initial Tool Choice, Probs want to display this in the UI somehow
		_currentTool = _tubeTool;
		_mat = _defaultMat;
		_color = _defaultColor;
		// This should probably be set by a material manager
		_cursor.SetCursorMat(Mat);
	}

	public void SetMaterial(string newMat) {
		// Handle for when material does not exist
		_mat = newMat;
		_cursor.SetCursorMat(Mat);
	}

	public void SetColor(string newColor) {
		// Handle for when material does not exist
		_color = newColor;
		_cursor.SetCursorMat(Mat);
	}

	public Material GetLoadedMat(string mat) {
		return _materials.Find(m => m.name == mat);
	}

	// Make the brush ID an enum or something so this ain't hard codeds
	public void SetBrush(int brushId) {
		if (brushId == 1) {
			_currentTool = _tubeTool;
		} else if (brushId == 0) {
			_currentTool = _ribbonTool;
		}
	}

	public void StartStroke(Transform parent) {
		_currentTool.StartStroke(parent, Mat);
	}

	public void EndStroke(Transform parent) {
		// Save command once the stroke has been completed for undo/redo
		ICommand command = (ICommand)_currentTool.EndStroke(parent, Mat);
		_undoStack.Push(command);
		_redoStack.Clear();
	}

	public void UpdateStroke(Vector3 pos, Quaternion rot, float r) {
		_currentTool.UpdateStroke(pos, rot, r);
	}

	public void Undo() {
		// Only run if there are commands to Undo
		if (_undoStack.Count > 0) {
			ICommand command = _undoStack.Pop();
			command.Undo();
			_redoStack.Push(command);
		}
	}

	public void Redo() {
		// Only run if there are commands to Redo
		if (_redoStack.Count > 0) {
			ICommand command = _redoStack.Pop();
			command.Execute();
			_undoStack.Push(command);
		}
	}
}
