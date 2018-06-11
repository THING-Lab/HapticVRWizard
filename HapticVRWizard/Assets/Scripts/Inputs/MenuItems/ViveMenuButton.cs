using UnityEngine;
using UnityEngine.Events;

public class ViveMenuButton : MonoBehaviour {
	public UnityEvent _buttonEvent;

	public void Execute() {
		_buttonEvent.Invoke();
	}
}
