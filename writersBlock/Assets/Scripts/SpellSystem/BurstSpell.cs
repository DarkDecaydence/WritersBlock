using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class BurstSpell : MonoBehaviour
{
    #region Static methods
    public static string SpellElementName(SpellElement e)
    {
        return EnumUtil.GetValues<SpellElement>().FirstOrDefault(s => EnumUtil.Parse<SpellElement>(s) == e);
    }

    //public static void SpawnIncantation(GameObject source, GameObject spellPrefab, SpellData data, Vector2 direction)
    //{
    //    if (spellPrefab != null) {
    //        var newGObj = Instantiate<GameObject>(spellPrefab);
    //        var newIncantation = newGObj.GetComponent<BurstSpell>();
    //        newGObj.transform.position = source.transform.position;
    //        newGObj.transform.localScale = new Vector3(data.Size, data.Size, data.Size);
    //        newIncantation.data = data;
    //        newIncantation.Position = source.GetComponent<Character>().Pos;
    //        newIncantation.Direction = direction;

    //        if (data.SpellElement != SpellElement.Invalid) {
    //            GameData.audioManager.PlaySpell(SpellElementName(data.SpellElement) + "Spell");
    //        }
    //        else {
    //            GameData.audioManager.PlaySpell("Fizzle");
    //        }
    //    }
    //    else {
    //        Debug.LogError("Derp, no such spell. Try again later.");
    //    }
    //}
    #endregion

    private SpellData data;
    private Vector2 Direction;
    private Vec2i Position;

    private bool isDestroyed = false;

    public void Start()
    {
        StartCoroutine(CoStart());
    }

    public IEnumerator CoStart()
    {
        yield return StartCoroutine(CoCheckCone());
        yield return StartCoroutine(destroyAfter(0.6f));
    }

    private IEnumerator CoCheckCone()
    {
        for (int curY = 1; curY <= 4; curY++) {
            var curX = (curY - 1) * -1;
            while (Math.Abs(curX) < curY) {
                // Check tile (curX, curY) for monsters and damage them.
                var targetTile = new Vec2i(curX, curY);
                // Rotate targetTile to direction, since targetTile assumes that spell is pointed upwards.
                curX++;
            }
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator destroyAfter(float t)
    {
        isDestroyed = true;
        if (gameObject.GetComponent<ParticleSystem>() != null)
            gameObject.GetComponent<ParticleSystem>().Stop();
        Transform child = transform.FindChild("Point light");
        Light l;
        if (child != null)
            l = child.GetComponent<Light>();
        else
            l = null;

        float tSplit = t / 10f;
        t = tSplit;
        for (int i = 0; i < 10; i++) {
            t += tSplit;
            if (l != null)
                l.intensity = 1 - t;
            yield return new WaitForSeconds(tSplit);
        }

        Destroy(gameObject);
    }
}
