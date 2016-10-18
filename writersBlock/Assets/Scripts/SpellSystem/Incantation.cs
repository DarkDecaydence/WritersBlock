using UnityEngine;
using System.Collections;
using System.Linq;

public class Incantation : MonoBehaviour
{
    public static SpellData MisfireData
    {
        get { return new SpellData(SpellElement.Invalid, SpellType.Invalid, 0, 0); }
    }

    public static void SpawnIncantation(GameObject source, GameObject spellPrefab, SpellData data, Vector2 direction)
    {
        if (spellPrefab != null) { 
            var newGObj = Instantiate<GameObject>(spellPrefab);
            var newIncantation = newGObj.GetComponent<Incantation>();
            newGObj.transform.position = source.transform.position;
            newIncantation.data = data;
            newIncantation.Position = source.GetComponent<Character>().pos;
            newIncantation.Direction = direction;
        } else {
            Debug.LogError("Derp, no such spell. Try again later.");
        }
    }

    private SpellData data;
    public Vec2i Position;
    public Vector2 Direction;

    private bool isMoving = false;
    private bool isDestroyed = false;
    private bool isTriggered = false;

    void FixedUpdate()
    {
        if (!isMoving && !isDestroyed) {
            // Check if incantation has collided with anything.
            //var collidingPiece = GameData.gamePieces.FirstOrDefault(o => o.GetPosition().Equals(Position));
            if (!GameData.grid.getTile(Position).isWalkAble()) {
                StartCoroutine(destroyAfter(0.75f));
                return;
            }
            // Start Coroutine to move to next tile.
            StartCoroutine(CoMoveStep());
        }
    }

    private IEnumerator CoMoveStep()
    {
        // Acknowledge movement has started
        isMoving = true;
        var stepDistance = 0f;
        while (stepDistance <= 1f) {
            var tickDistance = Time.deltaTime * (float)data.Speed;
            stepDistance += tickDistance;
            transform.Translate(new Vector3(Direction.x, 0, Direction.y) * tickDistance);
            yield return null;
        }
        Position = Position + new Vec2i(Direction);
        isMoving = false;
    }  

    void OnTriggerEnter(Collider coll)
    {   
        if (coll.CompareTag("Player") || isTriggered)
            return;

        isTriggered = true;

        doDamage(coll.gameObject);

        StartCoroutine(destroyAfter( 0.75f ));

    }

    void doDamage(GameObject target)
    {
        HealthScript health = target.GetComponent<HealthScript>();
        var mElem = target.GetComponent<Monster>().Element;
        if (health != null)
        {
            var multiplier = CalculateMultiplier(data.SpellElement, mElem);
            health.addHealth(-data.Power * multiplier);
        }
    }

    private IEnumerator destroyAfter(float t)
    {
        isDestroyed = true;
        gameObject.GetComponent<ParticleSystem>().Stop();
        Light l = transform.FindChild("Point light").GetComponent<Light>();
        float tSplit = t / 10f;
        t = tSplit;
        for(int i = 0; i < 10; i++)
        {
            t += tSplit;
            l.intensity = 1 - t;
            yield return new WaitForSeconds(tSplit);
        }
        
        Destroy(gameObject);

    }

    private float CalculateMultiplier(SpellElement sElem, MonsterElement mElem)
    {
        // If monster has no element, default multiplier should return.

        // Calculating monster weaknesses.
        if (mElem == MonsterElement.Fire) {
            switch (sElem) {
                case SpellElement.Water:
                    return 2.75f; // Water is super effective against Fire.
                case SpellElement.Earth:
                case SpellElement.Air:
                    return 1.5f; // Earth and Air is effective against Fire.
            }
        } else if ((mElem == MonsterElement.Air && sElem == SpellElement.Arcane) || (mElem == MonsterElement.Earth && sElem == SpellElement.Void)) {
            return 2f; // Arcane is effective against Air, and Void is effective against Earth.
        }

        // Calculating monster strengths.
        if (sElem == SpellElement.Fire) {
            switch (mElem) {
                case MonsterElement.Water:
                    return 0.36f; // Fire is super ineffective against Water.
                case MonsterElement.Earth:
                case MonsterElement.Air:
                    return 0.75f; // Fire is ineffective against Earth and Air.
            }
        } else if ((sElem == SpellElement.Air && mElem == MonsterElement.Arcane) || (sElem == SpellElement.Earth && mElem == MonsterElement.Void)) {
            return 0.5f; // Air is ineffective against Arcane, and Earth is Ineffective against Void.
        }

        return 1f; // If none of the above, calculate damage normally.
    }

}
