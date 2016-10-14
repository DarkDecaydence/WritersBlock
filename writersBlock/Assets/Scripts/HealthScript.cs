using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour {

    float maxHealth = 100f;
    float health;

    Slider healthBar;

    public WorldUIManager uiManager;

    void Awake()
    {

        healthBar = uiManager.instantiateHealthbar();
        setLife(maxHealth);

    }

    void Update()
    {
        updateHealthbarPos();
    }

    void updateHealthbarPos()
    {

        healthBar.gameObject.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);

    }

    public void setLife(float f)
    {

        health = Mathf.Clamp(f, 0, maxHealth);
        updateHealthBarVisuals();

    }

    public void addHealth(float f)
    {

        health = Mathf.Clamp(f + health, -1, maxHealth);
        updateHealthBarVisuals();
        if (health <= 0)
        {
            killObj();
        }

    }

    void updateHealthBarVisuals()
    {
        healthBar.value = Mathf.Lerp(0, 1, health / maxHealth);
    }

    public void killObj()
    {
        Destroy(this.gameObject);
    }

}
