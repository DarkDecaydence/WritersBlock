using UnityEngine;
using System.Collections;

public class Incantation : MonoBehaviour
{
    public static SpellData MisfireData
    {
        get { return new SpellData(SpellElement.Invalid, SpellType.Invalid, 0, 0); }
    }

    public static void SpawnIncantation(GameObject source, GameObject spellPrefab, Vector2 direction)
    {
        var newGObj = Instantiate<GameObject>(spellPrefab);
        var newIncantation = newGObj.GetComponent<Incantation>();
        newIncantation.Position = new Vec2i(source.transform.position);
        newIncantation.Direction = direction;
    }

    private SpellData data;
    public Vec2i Position;
    public Vector2 Direction;

    private bool isMoving = false;

    void Update()
    {
        if (!isMoving) {
            // Check if incantation has collided with anything.
            if (!GameData.grid.getTile(Position).isWalkAble()) {
                Destroy(gameObject);
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
            stepDistance += Time.deltaTime * data.Speed;
            transform.Translate(Direction * stepDistance);
            yield return null;
        }
        Position = Position + new Vec2i(Direction);
        isMoving = false;
    }

}
