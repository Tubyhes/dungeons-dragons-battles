using UnityEngine;
using System.Collections;

public class FullDefense : CombatEffect {

	public FullDefense () {
		duration = 0;
	}

	public override int ArmorClass (int ac) {
		return ac + 4;
	}

	public override int ToHit (int toHit) {
		return toHit;
	}

	public override int Damage (int dmg) {
		return dmg;
	}

	public override bool EndsThisTurn () {
		if (duration == 0) {
			return true;
		} else {
			duration--;
			return false;
		}
	}
}
