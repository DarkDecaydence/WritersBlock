using UnityEngine;
using System.Collections;
using System.Linq;
using System.Reflection;

public class Incantation : MonoBehaviour
{
    #region Static methods.
    public static SpellData MisfireData
    {
        get { return new SpellData(SpellElement.Invalid, SpellType.Invalid, 0, 0, 0); }
    }

    public static string SpellElementName(SpellElement e)
    {
        return EnumUtil.GetValues<SpellElement>().FirstOrDefault(s => EnumUtil.Parse<SpellElement>(s) == e);
    }

    public static void SpawnIncantation(GameObject source, GameObject spellPrefab, SpellData data, Vector2 direction)
    {
        if (spellPrefab != null) { 
            var newGObj = Instantiate<GameObject>(spellPrefab);
            var newIncantation = newGObj.GetComponent<Incantation>();
            newGObj.transform.position = source.transform.position;
            newGObj.transform.localScale = new Vector3(data.Size, data.Size, data.Size);
            newIncantation.data = data;
            newIncantation.Position = source.GetComponent<Character>().pos;
            newIncantation.Direction = direction;

            if (data.SpellElement != SpellElement.Invalid) {
                GameData.audioManager.PlaySpell(SpellElementName(data.SpellElement) + "Spell");
            } else {
                GameData.audioManager.PlaySpell("Fizzle");
            }
        } else {
            Debug.LogError("Derp, no such spell. Try again later.");
        }
    }
    #endregion

    private SpellData data;
    private Vec2i Position;
    private Vector2 Direction;

    private bool isMoving = false;
    private bool isDestroyed = false;
    private bool isTriggered = false;

    public bool rotate = false;

    void FixedUpdate()
    {
        Rotate();
        
    }

    void LateUpdate()
    {
        if (!isMoving && !isDestroyed)
        {
            // Check if incantation has collided with anything.
            //var collidingPiece = GameData.gamePieces.FirstOrDefault(o => o.GetPosition().Equals(Position));
            if (!GameData.grid.getTile(Position).isWalkAble())
            {
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
        //var stepDistance = 0f;

        float t = 0;
        Vector3 pos = transform.position;
        Vector3 newPos = pos + new Vector3(Direction.x, 0, Direction.y);
        while (t < 1) {
            //var tickDistance = Time.deltaTime * (float)data.Speed;
            //stepDistance += tickDistance;
            
            transform.position = Vector3.Lerp(pos, newPos, t);
            t += (Time.deltaTime * data.Speed);
            //transform.Translate(new Vector3(Direction.x, 0, Direction.y) * tickDistance);
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
        if(gameObject.GetComponent<ParticleSystem>() != null)
            gameObject.GetComponent<ParticleSystem>().Stop();
        Transform child = transform.FindChild("Point light");
        Light l;
        if (child != null)
            l = child.GetComponent<Light>();
        else
            l = null;

        float tSplit = t / 10f;
        t = tSplit;
        for(int i = 0; i < 10; i++)
        {
            t += tSplit;
            if(l != null)
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

    void Rotate()
    {
        if (rotate)
        {
            Debug.Log(Direction);
            transform.Rotate(new Vector3(Direction.y, 0, Direction.x));

        }
    }

}
