using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {
	public RibbonTool _ribbonTool;
	public TubeTool _tubeTool;

	public Material _defaultMat;
	public Color _defaultColor;

	public List<Material> _materials;

	// Undo Redo Datastructures
	private Stack<ICommand> _undoStack = new Stack<ICommand>();
	private Stack<ICommand> _redoStack = new Stack<ICommand>();

	private Material _mat;
	private Color _col;

	private ITool _currentTool;

	// Use this for initialization
	void Start () {
		// Initial Tool Choice, Probs want to display this in the UI somehow
		_currentTool = _tubeTool;
		_mat = _defaultMat;
		_col = _defaultColor;
		_mat.color = _col;
	}

	public void SetMaterial(string newMat) {
		// Handle for when material does not exist
		_mat = _materials.Find(m => m.name == newMat);
	}

	public void SetBrush(int brushId) {
		if (brushId == 0) {
			_currentTool = _tubeTool;
		} else if (brushId == 1) {
			_currentTool = _ribbonTool;
		}
	}

	public void SetColor(Color newCol) {
		_col = newCol;
		_mat.color = _col;
	}

	public void StartStroke(Transform parent) {
		_currentTool.StartStroke(parent, _mat);
	}

	public void EndStroke(Transform parent) {
		// Save command once the stroke has been completed for undo/redo
		ICommand command = (ICommand)_currentTool.EndStroke(parent, _mat);
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
