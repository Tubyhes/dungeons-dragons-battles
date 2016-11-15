using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public string name;
	public TextAsset textFile;
	public Sprite upSprite;
	public Sprite horizontalSprite;
	public Sprite downSprite;

	private string[] thingsToSay;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();

		if (textFile != null) {
			thingsToSay = textFile.text.Split ('\n');
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Interact (Helpers.Direction fromDirection) {
		FaceDirection (fromDirection);

		ConversationController conversationController = FindObjectOfType<ConversationController> ();
		conversationController.StartConversation (name, thingsToSay);
	}

	private void FaceDirection (Helpers.Direction fromDirection) {
		if (fromDirection == Helpers.Direction.Up) {
			spriteRenderer.sprite = upSprite;
		} else if (fromDirection == Helpers.Direction.Down) {
			spriteRenderer.sprite = downSprite;
		} else if (fromDirection == Helpers.Direction.Left) {
			spriteRenderer.sprite = horizontalSprite;
			TurnLeft ();
		} else if (fromDirection == Helpers.Direction.Right) {
			spriteRenderer.sprite = horizontalSprite;
			TurnRight ();
		}
	}

	private void TurnLeft() {
		Vector3 newScale = transform.localScale;
		newScale.x = -1;
		transform.localScale = newScale;
	}

	private void TurnRight() {
		Vector3 newScale = transform.localScale;
		newScale.x = 1;
		transform.localScale = newScale;	
	}
}
