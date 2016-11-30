using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ConversationController : MonoBehaviour {

	public GameObject modalPanel;
	public Text questText;

	private string currentNPC;
	private string[] currentConversation;
	private int conversationCounter;
	private bool ignoreFirstClick = false;

	void Update () {
		if (!GameManager.Instance().IsInteracting) {
			return;
		}
		if (ignoreFirstClick) {
			ignoreFirstClick = false;
			return;
		}
		if (Input.GetButtonDown ("Fire1_P1")) {
			ProgressConversation ();
		}
	}

	public void StartConversation (string npc, string[] conversation) {
		GameManager.Instance().IsInteracting = true;
		ignoreFirstClick = true;
		currentNPC = npc;
		currentConversation = conversation;
		conversationCounter = 0;
		modalPanel.SetActive (true);

		SetText (currentConversation [conversationCounter]);
	}

	private void ProgressConversation () {
		conversationCounter++;
		if (conversationCounter >= currentConversation.Length) {
			EndConversation ();
		} else {
			SetText (currentConversation [conversationCounter]);
		}
	}

	private void EndConversation () {
		modalPanel.SetActive (false);
		Invoke ("EndInteracting", 0.2f);
	}

	private void EndInteracting () {
		GameManager.Instance().IsInteracting = false;
	}

	private void SetText (string text)  {
//		Debug.Log (text);
		questText.text = currentNPC + ": " + text;
	}
}
