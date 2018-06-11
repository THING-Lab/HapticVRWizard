using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ViveMenuToggle : MonoBehaviour {
	public UnityEvent _selectEvent;
	public UnityEvent _deselectEvent;

	public bool _isSelected = false;
	private bool _isEnabled = true;
	public bool IsEnabled {
		get { return _isEnabled; }
	}

	public Color _unselectedColor;
	public Color _disabledColor;
	public Color _selectedColor;
	public int _trackerId;

	private ViveMenuRadioList ParentList {
		get { return transform.parent.gameObject.GetComponent<ViveMenuRadioList>(); }
	}

	public void Start() {
		if (_isSelected) {
			GetComponent<Image>().color = _selectedColor;
		}
	}

	public void Disable() {
		// Might want to handle for if this is the selected one here...
		_isSelected = false;
		_isEnabled = false;
		GetComponent<Image>().color = _disabledColor;
	}

	public void Enable() {
		_isEnabled = true;
		GetComponent<Image>().color = _unselectedColor;
	}

	public void Deselect() {
		if (_isEnabled) {
			_isSelected = false;
			// Actual display of state
			GetComponent<Image>().color = _unselectedColor;
		}
	}

	public void Select() {
		if (_isEnabled) {
			_isSelected = true;
			// Actual display of state
			GetComponent<Image>().color = _selectedColor;
		}
	}

	public void Execute() {
		if (_isEnabled) {
			if (_isSelected) {
				// If there is a parent manager don't fire the deselect event
				if (ParentList == null) {
					Deselect();
					_deselectEvent.Invoke();
				}
			} else {
				// Check if there is a parent manager, if so call it's deselect function first for the others
				if (ParentList != null) {
					ParentList.DeselectOthers(_trackerId);
				}
				_selectEvent.Invoke();
				Select();
				
			}
		}
		
	}
}
