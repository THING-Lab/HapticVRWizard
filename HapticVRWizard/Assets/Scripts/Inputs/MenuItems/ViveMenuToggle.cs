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

	public GameObject _selectedImage;
	public GameObject _unselectedImage;

	public int _trackerId;

	private ViveMenuRadioList ParentList {
		get { return transform.parent.gameObject.GetComponent<ViveMenuRadioList>(); }
	}

	public void Start() {
		if (_isSelected) {
			_unselectedImage.SetActive(false);
			_selectedImage.SetActive(true);
		}
	}

	public void Disable() {
		// Might want to handle for if this is the selected one here...
		_isSelected = false;
		_isEnabled = false;
		_unselectedImage.SetActive(true);
		_selectedImage.SetActive(false);
	}

	public void Enable() {
		_isEnabled = true;
		_unselectedImage.SetActive(true);
		_selectedImage.SetActive(false);
	}

	public void Deselect() {
		if (_isEnabled) {
			_isSelected = false;
			// Acutal display of state
			_selectedImage.SetActive(false);
			_unselectedImage.SetActive(true);
		}
	}

	public void Select() {
		if (_isEnabled) {
			_isSelected = true;
			// Actual display of state
			_unselectedImage.SetActive(false);
			_selectedImage.SetActive(true);
		}
	}

	public void Execute() {
		Debug.Log("It's Comin Ian");
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
					// This should be in a separate script
					ParentList.DeselectOthers(_trackerId);
				}
				_selectEvent.Invoke();
				Select();
				
			}
		}
		
	}
}
