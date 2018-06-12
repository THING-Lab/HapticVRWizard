using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveMenuRadioList : MonoBehaviour {
	public List<ViveMenuToggle> _items;

	public void DeselectOthers(int itemId) {
		// Gross for loop, but eh
		for (int i = 0; i < _items.Count; i++) {
			if (i != itemId) {
				_items[i].Deselect();
			}
		}
	}
}
