using UnityEngine;
using System.Collections;

public class GameEntity : MonoBehaviour {

    public GameController GameController;

    public int TeamNumber = -1; // -1 for no team, useful for non-players
    public bool BlocksMinions = true;
    public bool BlocksBullets = true;

    #region Sprite Info
    public GameObject ChildSprite = null;
    public float SpriteRadius = 0.0f;
    #endregion

    void Awake() {
    }
	
	void Update () {
	}

    void LateUpdate() {
        // After everything, check if we need to remove this from the grid.
    }

    public void Kill() {
        Logic logic = GetComponent<Logic>();
        if (logic != null) {
            logic.Kill();
        }
    }

    #region GridStuff (optimization from Shipwreck)
    /*private void RemoveFromGrid() {
        GameController.RemoveFromGrid(gameObject, GridCell);
        GridCell = -1;
        StopGridCheck();
    }

    public void StartGridCheck(float timer = 0.8f) {
        gridCheckTimer = timer;
        StartCoroutine(gridCheck);
    }

    public void StopGridCheck() {
        StopCoroutine(gridCheck);
    }

    private IEnumerator GridCheck() {
        while (true) {
            // Check position and update layer if necessary
            GridCell = GameController.UpdateGridCell(gameObject, GridCell);
            yield return new WaitForSeconds(gridCheckTimer);
        }
    }*/
    #endregion
}