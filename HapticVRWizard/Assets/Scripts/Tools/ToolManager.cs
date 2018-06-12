using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {

	public LineTool _lineTool;
	public TubeTool _tubeTool;

	// Undo Redo Datastructures
	private Stack<ICommand> _undoStack = new Stack<ICommand>();
	private Stack<ICommand> _redoStack = new Stack<ICommand>();

	private ITool _currentTool;

	// Use this for initialization
	void Start () {
		// Initial Tool Choice, Probs want to display this in the UI somehow
		_currentTool = _tubeTool;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.T)) {
			_currentTool = _tubeTool;
		}

		if(Input.GetKeyDown(KeyCode.L)) {
			_currentTool = _lineTool;
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			// SaveDrawing();
		}
	}

	public void StartStroke(Transform parent) {
		_currentTool.StartStroke(parent);
	}

	public void EndStroke(Transform parent) {
		// Save command once the stroke has been completed for undo/redo
		ICommand command = (ICommand)_currentTool.EndStroke(parent);
		_undoStack.Push(command);
		_redoStack.Clear();
	}

	public void UpdateStroke(Vector3 pos, float r) {
		_currentTool.UpdateStroke(pos, r);
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
