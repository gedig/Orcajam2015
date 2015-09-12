using UnityEngine;
using System.Collections;

public class PlayerLogic : Logic {

    #region Components
    private GameEntity coreComponent;
    private Rigidbody rigidBodyRef;
    #endregion
    #region Control Information
    private int inputID;  // The rewired player.
    private Vector3 moveVector;
    private Vector3 aimVector;
    #endregion
    #region AI
    //private GameSettings.TeamActor actor; // TODO-DG: We could totally put AI in woah
    #endregion
    #region Movement
    public float Acceleration = 30.0f;
    public float MaxSpeed = 20.0f;
    #endregion
    #region Attacking
    private float attackCooldown = 0.5f;
    private int bulletDamage = 10;
    public int BulletDamage { get { return bulletDamage; } }
    private float bulletSpeed = 25.0f;
    public float BulletSpeed { get { return bulletSpeed; } }
    private float bulletRange = 3.0f;
    public float BulletRange { get { return bulletRange; } }

    private bool bulletPenetration = false;
    public bool BulletPenetration { get { return bulletPenetration; } }
    #endregion
    #region Death and Undying
    public bool Dead = false;
    private bool respawnable = false;
    private float respawnTimer;
    private float initialRespawnTime = 10.0f;
    private float respawnTimeIncrease = 20.0f;
    private Vector2 spawnPos;
    private Quaternion spawnRot;
    #endregion
    private AudioSource mAudio;
    private ParticleSystem particle;
    private Transform horn;
    private IEnumerator hornAnim;
    private bool hornAnimating = false;
    [SerializeField] private AudioClip[] foghorns;

    public void Awake() {
        hornAnim = HornAnim();
        Transform child = transform.Find("Model");
        horn = child.Find("Horn");
        mAudio = child.GetComponent<AudioSource>();
        coreComponent = GetComponent<GameEntity>();
        particle = horn.GetComponent<ParticleSystem>();
        rigidBodyRef = child.GetComponent<Rigidbody>();
    }

    public void Init(GameController gc, int playerID, Vector2 spawnPos, Quaternion spawnRot) {
        this.spawnPos = transform.position = spawnPos;
        this.spawnRot = spawnRot;
        this.inputID = playerID;
        coreComponent.GameController = gc;
    }

    float shotTimer = 0.0f;
	void Update () {
        if (Dead) {
            if (respawnable) {
                if (respawnTimer <= 0f) {
                    Respawn();
                } else {
                    respawnTimer -= Time.deltaTime;
                }
            }
        } else {
            if (shotTimer > 0.0f) {
                shotTimer -= Time.deltaTime;
            }
        }
        
        //if (actor == GameSettings.TeamActor.Human) {
        if (!Dead) {
            ProcessMovementInput();
            ProcessButtons();
        }
        //} else {
            // TODO-DG: AI for computer players
        //}
	}

    /**
     * Function that changes AI difficulty and to and from human
     **/
    /*public void ChangeActor(GameSettings.TeamActor newActor, int id) {
        if (actor != newActor) {
            // Stop behaviour coroutine, start new coroutine or set human.
            actor = newActor;
            switch (newActor) {
                case GameSettings.TeamActor.Human:
                    this.inputID = id;
                    break;
                    // TODO-DG: Assign proper behaviour coroutine.
            }
        } else {
            if (newActor == GameSettings.TeamActor.Human) {
                this.inputID = id;
            }
        }
    }*/

    private void ProcessMovementInput() {
        moveVector = PlayerInputController.Instance.GetMovement(inputID);
        aimVector = PlayerInputController.Instance.GetAim(inputID);

        MovePlayer();

        if (aimVector.x != 0.0f || aimVector.y != 0.0f) {
            if (shotTimer <= 0.0f) { // check cooldown and fire, if possible
                // Shoot!
                //coreComponent.GameController.ShootBullet(aimVector, this);
                shotTimer = attackCooldown;
            }
        }
    }

    private void ProcessButtons() {
        for (int i = 0; i < foghorns.Length; i++) {
            if (PlayerInputController.Instance.GetFoghorn(inputID, i+1)) {
                // Play Foghorn sound
                mAudio.PlayOneShot(foghorns[i]);
                particle.Emit(1);
                // TODO-DG: Lerp horn to animate up to 3x in x or y, shrink down to .5x, then return to normal
                if (!hornAnimating) {
                    StopCoroutine(hornAnim);
                    hornAnim = HornAnim(); // Seriously no better way to reset a coroutine? :/
                    StartCoroutine(hornAnim);
                }
            }
        }
    }

    private float maxSize = 3.0f;
    private float minSize = .5f;
    private WaitForFixedUpdate itsame = new WaitForFixedUpdate();
    private IEnumerator HornAnim() {
        hornAnimating = true;
        bool scaleX = (Random.value < .5f);
        float scaleFactor = scaleX ? horn.localScale.x : horn.localScale.y;
        int numFrames = 30;
        float animRate = (maxSize - scaleFactor) / numFrames;
        for (int i = 0; i < numFrames; i++) {
            scaleFactor += animRate;
            PerformScale(scaleFactor, scaleX);
            yield return itsame;
        }
        numFrames = 30;
        animRate = (maxSize - minSize) / numFrames;
        for (int i = 0; i < numFrames; i++) {
            scaleFactor -= animRate;
            PerformScale(scaleFactor, scaleX);
            yield return itsame;
        }
        numFrames = 30;
        animRate = (1.0f - minSize) / numFrames;
        for (int i = 0; i < numFrames; i++) {
            scaleFactor += animRate;
            PerformScale(scaleFactor, scaleX);
            yield return itsame;
        }
        hornAnimating = false;
    }

    // Return true if we are at the size.
    private void PerformScale(float scaleFactor, bool scaleX) {
        if (scaleX) {
            horn.localScale = new Vector3(scaleFactor, 1.0f, 1.0f);
        } else {
            horn.localScale = new Vector3(1.0f, scaleFactor, 1.0f);
        }
    }

    private void MovePlayer() {
        if (moveVector.x != 0.0f || moveVector.z != 0.0f) {
            rigidBodyRef.AddForce(moveVector * Acceleration);
            if (rigidBodyRef.velocity.magnitude > MaxSpeed) {
                rigidBodyRef.velocity = rigidBodyRef.velocity.normalized * MaxSpeed;
            }
            Vector3 targetPosition = new Vector3(moveVector.x, 0f, moveVector.z);
            transform.Find("Model").right = targetPosition;
        }
    }
    public void Respawn() {
        /*transform.position = spawnPos;
        Dead = false;
        GetComponent<Attackable>().ResetHealth();
        coreComponent.ChangeSprite("Level1", true); // TODO-DG: Load for current level
        coreComponent.ChildSprite.transform.rotation = spawnRot;*/
    }

    public override void Kill() {
        GetComponent<Collider2D>().enabled = false;
        Dead = true;
        if (respawnable) {
            respawnTimer = initialRespawnTime;
            initialRespawnTime += respawnTimeIncrease;
        }
    }

    public override void ChangeTeam(int newTeam) {
        // Nothing in here yet
    }
}