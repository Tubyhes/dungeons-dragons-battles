﻿using UnityEngine;
using System.Collections;

public static class AxisButtonDownGetter {

	private static bool axisInUse = false;

	public static Vector2 GetInput (Helpers.Teams team) {
		float fx = Input.GetAxis (Helpers.Horizontal(team));
		float fy = Input.GetAxis (Helpers.Vertical(team));
		if (Mathf.Abs(fx) > Mathf.Abs(fy)) {
			fx = fx > 0f ? 1f : -1f;
			fy = 0f;
		} else if (Mathf.Abs(fy) > Mathf.Abs(fx))  {
			fx = 0f;
			fy = fy > 0f ? 1f : -1f;
		}

		if ((fx != 0f || fy != 0f) && !axisInUse) {
			axisInUse = true;
			return new Vector2 (fx, fy);
		} 

		if (fx == 0f && fy == 0f && axisInUse) {
			axisInUse = false;
		}

		return Vector2.zero;
	}
}
