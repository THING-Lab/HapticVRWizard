using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ViveMenuToggle : MonoBehaviour {
	public UnityEvent _selectEvent;
	public UnityEvent _deselectEvent;

	public bool _isSelected = false;

	public Color _unselectedColor;
	public Color _disabledColor;
	public Color _selectedColor;

	public void Execute() {
		if (_isSelected) {
			// Check if there is a parent manager, if so call that?
			_isSelected = false;
			_deselectEvent.Invoke();
			GetComponent<Image>().color = Color.black;
		} else {
			// Check if there is a parent manager, if so call it's deselect function first for the others
			_isSelected = true;
			_selectEvent.Invoke();
			GetComponent<Image>().color = _selectedColor;
		}
	}
}
