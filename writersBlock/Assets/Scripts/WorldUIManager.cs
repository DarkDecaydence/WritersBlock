using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldUIManager : MonoBehaviour {

    public GameObject healthBar;

    public Slider instantiateHealthbar()
    {

        GameObject obj = Instantiate(healthBar);
        obj.transform.SetParent(this.transform);
        obj.name = "HealthBar " + transform.childCount;
        return obj.GetComponent<Slider>();

    }
}
