using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatArea : MonoBehaviour {

	// preset tiles for building the combat area
	public GameObject[] grassTiles;
	public GameObject[] dirtTiles;
	public GameObject[] dungeonTiles;
	public GameObject[] objectTiles;
	public GameObject[] outerWallTiles;

	// chosen tiles for building the combrat area
	private GameObject[] groundTiles;

	// size of the combat area
	public int width = 8;
	public int height = 8;
	public int numBlockingObjects = 0;

	// the actual playing area
	private Transform area;
	public List<Vector3> blockingObjects;

	// Use this for initialization
	public void SetupCombatArea (Helpers.GroundTypes groundType) {
		if (groundType == Helpers.GroundTypes.Grassland) {
			groundTiles = grassTiles;
		} else {
			// TODO: handle other ground types
			groundTiles = grassTiles;
		}

		area = new GameObject ("Area").transform;

		// make sure the camera will be centered on the combat screen
		CenterCamera ();

		// draw all elements of the combat
		DrawArea ();
		AddPlayers ();
		AddEnemies ();
		AddBlockingObjects ();
	}

	private void DrawArea () {
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				// make a new groundTile are this location
				GameObject groundTile = Instantiate (groundTiles [Random.Range (0, groundTiles.Length)], new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				groundTile.transform.SetParent (area);

				// if we're at the edge of the area, also build the outer wall
				if (x == 0 || y == 0 || x == width - 1 || y == height - 1) {
					GameObject wallTile = Instantiate (outerWallTiles [Random.Range (0, outerWallTiles.Length)], new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					wallTile.transform.SetParent (area);
				}
			}
		}
	}

	private void AddPlayers () {
		string playerResource = GameManager.Instance().playerType + "Combat";
		float xPos = 2;
		float yPos = height / 2; 
		GameObject player = Instantiate (Resources.Load(playerResource), new Vector3 (xPos, yPos, 0), Quaternion.identity) as GameObject;
		player.transform.SetParent (area);
	}

	private void AddEnemies () {
		string enemyResource = GameManager.Instance ().combat.enemyType + "Combat";
		float xPos = width - 3;
		float yPos = height / 2; 
		GameObject enemy = Instantiate (Resources.Load(enemyResource), new Vector3 (xPos, yPos, 0), Quaternion.identity) as GameObject;
		enemy.transform.SetParent (area);
	}

	private void CenterCamera () {
		float xPos = width / 2f - 0.5f; 
		float yPos = height / 2f - 0.5f;
		Camera.main.transform.position = new Vector3 (xPos, yPos, -10f);
		Camera.main.orthographicSize = height / 2f;
	}

	private void AddBlockingObjects () {
		blockingObjects = new List<Vector3> ();
		// add actual blocking objects
	}

	public bool IsGridPositionFree (Vector3 position) {

		// if this position is on the edge, it is definitely blocked
		if (position.x == 0 || position.x == width - 1 || position.y == 0 || position.y == height - 1) {
			return false;
		}

		// check if this position is taken by a blocking object
		if (blockingObjects.Contains (position)) {
			return false;
		}

		// no other things that might block
		return true;
	}
}
