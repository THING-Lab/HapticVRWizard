using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class ToolManager : MonoBehaviour {

	public GameObject _lineManager;
	public GameObject _tubeManager;

	// Undo Redo Code
	private Stack<ICommand> _undoStack = new Stack<ICommand>();
	private Stack<ICommand> _redoStack = new Stack<ICommand>();
	// .Clear(); .Pop(); .Empty();?

	private ITool _currentTool;

	// Use this for initialization
	void Start () {
		// Initial Tool Choice, Probs want to display this in the UI somehow
		_currentTool = _tubeManager.GetComponent<TubeTool>();

		// Gen filename here, base it off path
		string drawingLocation = Application.dataPath + "/Drawings";
		DirectoryInfo directory = new DirectoryInfo(drawingLocation);
		IOrderedEnumerable<FileInfo> drawFiles = directory.GetFiles("*.json")
			.OrderByDescending(f => f.LastWriteTime);

		if (drawFiles.Count() > 0) {
			_tubeManager.GetComponent<TubeTool>().ImportDrawing(drawFiles.First().FullName);
		}
	}

	public void StartStroke() {
		_currentTool.StartStroke();
	}

	public void EndStroke() {
		ICommand command = (ICommand)_currentTool.EndStroke();
		_undoStack.Push(command);
		_redoStack.Clear();
	}

	public void UpdateStroke(Vector3 pos, float r) {
		_currentTool.UpdateStroke(pos, r);
	}

	public void Undo() {
		if (_undoStack.Count > 0) {
			ICommand command = _undoStack.Pop();
			command.Undo();
			_redoStack.Push(command);
		}
	}

	public void Redo() {
		if (_redoStack.Count > 0) {
			ICommand command = _redoStack.Pop();
			command.Redo();
			_undoStack.Push(command);
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.T)) {
			_currentTool = _tubeManager.GetComponent<TubeTool>();
		}

		if(Input.GetKeyDown(KeyCode.L)) {
			_currentTool = _lineManager.GetComponent<LineTool>();
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			string date = System.DateTime.Now.ToString()
				.Replace(" ", "_")
				.Replace("/", "-")
				.Replace(":", ".");

			string filename = Application.dataPath + "/Drawings/drawing_" + date + ".json";
			_tubeManager.GetComponent<TubeTool>().ExportDrawing(filename);
		}
	}
}
