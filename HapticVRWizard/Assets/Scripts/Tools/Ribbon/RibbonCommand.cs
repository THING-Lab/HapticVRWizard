using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is literally the same as tube command -.-
// We only need separate things if we need to manipulate stuff after
public class RibbonCommand : ICommand {

	private string _id;
	private List<Vector3> _verts;
	private List<Vector2> _uvs;
	private List<int> _tris;
	private RibbonTool _tool;
	private Transform _parent;

	public string Id { get { return _id; } }

	public RibbonCommand(string id, RibbonTool tool, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent) {
		_id = id;
		_tool = tool;
		_verts = verts;
		_tris = tris;
		_uvs = uvs;
		_parent = parent;
	}

	public void Undo() {
		_tool.RemoveRibbon(_id);
	}

	public void Execute() {
		_tool.AddRibbon(_id, _verts, _tris, _uvs, _parent);
	}
}
