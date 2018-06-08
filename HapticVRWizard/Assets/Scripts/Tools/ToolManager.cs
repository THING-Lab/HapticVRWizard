using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {

	public LineTool _lineTool;
	public TubeTool _tubeTool;
	public Transform _tracker;

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
			SaveDrawing();
		}
	}

	public void SaveDrawing() {
		string date = System.DateTime.Now.ToString()
			.Replace(" ", "_")
			.Replace("/", "-")
			.Replace(":", ".");

		string filename = Application.dataPath + "/Drawings/drawing_" + date + ".json";
		_tubeTool.ExportDrawing(filename);
	}

	public void LoadDrawing() {
		// Load previously saved file on startup
		string drawingLocation = Application.dataPath + "/Drawings";
		DirectoryInfo directory = new DirectoryInfo(drawingLocation);
		IOrderedEnumerable<FileInfo> drawFiles = directory.GetFiles("*.json")
			.OrderByDescending(f => f.LastWriteTime);

		if (drawFiles.Count() > 0) {
			_tubeTool.ImportDrawing(drawFiles.First().FullName);
		}
	}

	public void StartStroke() {
		_currentTool.StartStroke(_tracker);
	}

	public void EndStroke() {
		// Save command once the stroke has been completed for undo/redo
		ICommand command = (ICommand)_currentTool.EndStroke(_tracker);
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
