using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    private GameObject[] playerShips;
    private int numPlayers = 0;

    [SerializeField] private GameObject playerShipPrefab;

	// Use this for initialization
	void Start () {
	    // TODO-DG: Place the whale
        string invoke = PlayerInputController.Instance.tag;
        playerShips = new GameObject[8];
	}

    // Update is called once per frame
    void Update() {
        int newPlayerID = PlayerInputController.Instance.CheckForDropIn();
        if (newPlayerID != -1) {
            // Create new player
            GameObject newPlayer = (GameObject)Instantiate(playerShipPrefab);
            // TODO-DG: Calc location and position.
            newPlayer.GetComponent<PlayerLogic>().Init(this, newPlayerID, Vector2.zero, Quaternion.identity);
            playerShips[numPlayers++] = newPlayer;
        }
	}
}
