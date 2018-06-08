using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeCommand : ICommand {
	private string _id;
	private List<Vector3> _verts;
	private List<Vector2> _uvs;
	private List<int> _tris;
	private TubeTool _tool;
	private Transform _parent;

	public string Id { get { return _id; } }

	public TubeCommand(string id, TubeTool tool, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent) {
		_id = id;
		_tool = tool;
		_verts = verts;
		_tris = tris;
		_uvs = uvs;
		_parent = parent;
	}

	public void Undo() {
		_tool.RemoveTube(_id);
	}

	public void Execute() {
		_tool.AddTube(_id, _verts, _tris, _uvs, _parent);
	}
}
