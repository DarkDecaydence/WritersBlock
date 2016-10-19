using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class BurstSpell : MonoBehaviour
{
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
