using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TeamSetupController : MonoBehaviour {

	public Helpers.Teams team;

	public GameObject classPicker;
	public CharacterSheetController sheetController;

	public ClassButton fighterButton;
	public ClassButton clericButton;
	public ClassButton rogueButton;
	public ClassButton wizardButton;
	public ClassButton blankButton1;
	public ClassButton blankButton2;

	public TeamSetupPortraitController[] teamSetupPortraits;
	public Dictionary<Helpers.CharacterClass, ClassButton> classButtons;

	private GameManager gameManager;

	void Start () {
		gameManager = GameManager.Instance ();

		teamSetupPortraits [0].SetPlusButtonMode ();
		teamSetupPortraits [1].SetEmptyMode ();
		teamSetupPortraits [2].SetEmptyMode ();
		teamSetupPortraits [3].SetEmptyMode ();

		classPicker.SetActive (false);
		sheetController.gameObject.SetActive (false);
		classButtons = new Dictionary<Helpers.CharacterClass, ClassButton> {
			{ Helpers.CharacterClass.Fighter, fighterButton },
			{ Helpers.CharacterClass.Cleric, clericButton },
			{ Helpers.CharacterClass.Rogue, rogueButton },
			{ Helpers.CharacterClass.Wizard, wizardButton },
		};
	}

	public void ClassPicked (string c) {
		Debug.Log ("Class picked: " + c);

		Helpers.CharacterClass character_class = Helpers.stringToClass[c];
		teamSetupPortraits [gameManager.teamCombatants[team].Count].SetPortraitMode (Helpers.classPortraits [character_class]);

		classButtons [character_class].SetAvailable (false);
		classPicker.SetActive (false);

		sheetController.StartCustomization (CharacterSheet.NewCharacterSheet (character_class));
	}

	public void AddCharacterClicked () {
		Debug.Log ("Add Character Button Clicked!");

		classPicker.SetActive (true);

		foreach (KeyValuePair<Helpers.CharacterClass, ClassButton> c in classButtons) {
			if (c.Value.button.IsInteractable ()) {
				c.Value.button.Select ();
				break;
			}
		}

		teamSetupPortraits [gameManager.teamCombatants [team].Count].button.interactable = false;
	}

	public void CharacterSheetFinished () {
		CharacterSheet sheet = sheetController.sheet;
		bool teamsFull = gameManager.AddPlayerToTeam(sheet, team);

		if (gameManager.teamCombatants [team].Count < gameManager.maxTeamSize) {
			teamSetupPortraits [gameManager.teamCombatants [team].Count].SetPlusButtonMode ();
			teamSetupPortraits [gameManager.teamCombatants [team].Count].button.Select ();
		} else {
			// TODO: team completed animation

			if (teamsFull) {
				Invoke ("LoadCombatScene", 1f);
			}
		}

		sheetController.gameObject.SetActive (false);
	}

	private void LoadCombatScene () {
		SceneManager.LoadSceneAsync ("CombatArea", LoadSceneMode.Single);
	}
}
