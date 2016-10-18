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
            newIncantation.Position = source.GetComponent<IGamePiece>().GetPosition();
            newIncantation.Direction = direction;
        } else {
            Debug.LogError("Derp, no such spell. Try again later.");
        }
    }

    private SpellData data;
    public Vec2i Position;
    public Vector2 Direction;

    private bool isMoving = false;

    void Update()
    {
        if (!isMoving) {
            // Check if incantation has collided with anything.
            var collidingPiece = GameData.gamePieces.FirstOrDefault(o => o.GetPosition().Equals(Position));
            if (!GameData.grid.getTile(Position).isWalkAble() || (collidingPiece != null && collidingPiece != GameData.playerCharacter)) {
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
            var tickDistance = Time.deltaTime * (float)data.Speed;
            stepDistance += tickDistance;
            transform.Translate(new Vector3(Direction.x, 0, Direction.y) * tickDistance);
            yield return null;
        }
        Position = Position + new Vec2i(Direction);
        isMoving = false;
    }

}
