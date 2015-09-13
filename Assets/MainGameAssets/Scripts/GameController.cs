using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField] private Camera overheadCamera;

    private GameObject[] playerShips;
    private int numPlayers = 0;

    [SerializeField] private GameObject playerShipPrefab;

    public Vector3 TopLeftScreenToWorld;
    public Vector3 BottomRightScreenToWorld;

    void Awake() {
        TopLeftScreenToWorld = overheadCamera.ScreenToWorldPoint(new Vector3(0f, overheadCamera.pixelHeight, overheadCamera.transform.position.y));
        BottomRightScreenToWorld = overheadCamera.ScreenToWorldPoint(new Vector3(overheadCamera.pixelWidth, 0f, overheadCamera.transform.position.y));
    }
	// Use this for initialization
	void Start () {
        string invoke = PlayerInputController.Instance.tag;
        playerShips = new GameObject[8];
	}

    // Update is called once per frame
    void Update() {
        Debug.DrawLine(TopLeftScreenToWorld, BottomRightScreenToWorld);
        int newPlayerID = PlayerInputController.Instance.CheckForDropIn();
        if (newPlayerID != -1) {
            // Create new player
            GameObject newPlayer = (GameObject)Instantiate(playerShipPrefab);
            // TODO-DG: Calc location and position.
            newPlayer.GetComponent<PlayerLogic>().Init(this, newPlayerID, Vector2.zero, Quaternion.identity);
            playerShips[numPlayers++] = newPlayer;
        }
	}

    public void EndGame(int playerID) {
        // TODO-DG: Do the thing
    }
}
