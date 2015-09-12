using UnityEngine;
using Rewired;

/**
 * This is a pollable class that abstracts out our controller information.
 * This should be the ONLY class that should interact with our control library!
 **/
public class PlayerInputController : Singleton<PlayerInputController> {

    protected PlayerInputController () {} // Guarantees the singleton is only ever created once

    private Rewired.Player[] players;
    private bool[] isAssigned;

    public void Start() {
        DontDestroyOnLoad(this);

        players = new Rewired.Player[8];
        isAssigned = new bool[8];
        for (int i = 0; i < 8; i++) {
            players[i] = ReInput.players.GetPlayer(i);
            isAssigned[i] = false;
        }
    }

    public void Update() {
    }

    public void AssignPlayer(int id) {
        isAssigned[id] = true;
    }

    public Vector3 GetMovement(int id) {
        Vector3 moveVector = new Vector2();
        moveVector.x = players[id].GetAxis("MoveHorizontal");
        moveVector.z = players[id].GetAxis("MoveVertical");
        return moveVector;
    }

    public Vector3 GetAim(int id) {
        Vector3 aimVector = new Vector2();
        aimVector.x = players[id].GetAxis("AimHorizontal");
        aimVector.z = players[id].GetAxis("AimVertical");
        return aimVector;
    }

    #region DropIn
    public int CheckForDropIn() {
        // Check for drop-in players
        for (int i = 0; i < 8; i++) {
            if (!isAssigned[i]) {
                // Check if any bases are being selected. If they are then tell GameController about the request
                if (players[i].GetButtonDown("Foghorn")) {
                    isAssigned[i] = true;
                    return i;
                }
            }
        }
        return -1;
    }
    /**
     * If there are unassigned players, return the base that they have requested to join
     **/
    /*public bool QueryCommandButtons(int id, out GameObject targetBase) {
        bool rush = false;
        bool pressed = false;
        targetBase = null;
        foreach (string buttonName in GameSettings.Instance.CommandTable.Keys) {    // TODO-DG: Any way to get rid of this foreach loop?
            if (players[id].GetButtonDoublePressDown(buttonName)) {
                rush = true;
                pressed = true;
            } else if (players[id].GetButtonDown(buttonName)) {
                pressed = true;
            }

            if (pressed) {
                targetBase = GameSettings.Instance.CommandTable[buttonName];
                break;
            }
        }

        return rush;   
    }*/
    #endregion // DG: Code from Shipwreck.
}