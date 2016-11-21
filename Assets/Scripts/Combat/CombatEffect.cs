using UnityEngine;
using System.Collections;

public abstract class CombatEffect  {

	protected int duration;

	public abstract int ArmorClass (int ac);
	public abstract int ToHit (int toHit);
	public abstract int Damage (int dmg);

	public abstract bool EndsThisTurn ();
}
