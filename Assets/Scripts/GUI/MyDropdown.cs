using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyDropdown : Dropdown {

	public EventSystem eventSystem;

	protected override void Awake () {
		base.Awake ();
		eventSystem = GetComponent<EventSystemProvider> ().eventSystem;
	}

	public override void OnPointerDown (PointerEventData eventData) {
		if (eventData.button != PointerEventData.InputButton.Left) {
			return;
		}

		if (IsInteractable () && navigation.mode != Navigation.Mode.None) {
			eventSystem.SetSelectedGameObject (gameObject, eventData);
		}

		base.OnPointerDown (eventData);
	}

	public override void Select () {
		if (eventSystem.alreadySelecting) {
			return;
		}

		eventSystem.SetSelectedGameObject (gameObject);
	}

	public override void OnMove (AxisEventData eventData) {

		Selectable next = this;
		switch (eventData.moveDir) {
		case MoveDirection.Down:
			next = FindSelectableOnDown ();
			break;
		case MoveDirection.Up:
			next = FindSelectableOnUp ();
			break;
		case MoveDirection.Left:
			next = FindSelectableOnLeft ();
			break;
		case MoveDirection.Right:
			next = FindSelectableOnRight ();
			break;
		default:
			break;
		}

		// only process the move event if the next selectable is linked to the same event system
		if (next != null) {
			EventSystem es = next.gameObject.GetComponent<EventSystemProvider> ().eventSystem;
			if (es != eventSystem) {
				return;
			}
		}

		base.OnMove (eventData);
	}
}
