using UnityEngine;
using System.Collections;

public abstract class Logic : MonoBehaviour {
    // Class that defines methods that must be found on BaseLogic, MinionLogic, etc. that GameEntity might need to access.
    public abstract void ChangeTeam(int newTeam);
    public abstract void Kill();
}
