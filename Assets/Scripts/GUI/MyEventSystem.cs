using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MyEventSystem : EventSystem {

	protected override void OnEnable () {

	}

	protected override void Update () {
		EventSystem originalCurrent = EventSystem.current;
		current = this;
		base.Update ();
		current = originalCurrent;
	}
}
