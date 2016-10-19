using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthScript : MonoBehaviour {

    float maxHealth = 100;
    float health;

    Slider healthBar;

    void Awake()
    {

        healthBar = GameObject.Find("WorldCanvas").GetComponent<WorldUIManager>().instantiateHealthbar();
        setLife(maxHealth);

    }

    void Update()
    {
        updateHealthbarPos();
    }

    void updateHealthbarPos()
    {

        healthBar.gameObject.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);

    }

    public void setLife(float f)
    {

        health = Mathf.Clamp(f, 0, maxHealth);
        updateHealthBarVisuals();

    }

    // Split such that player and monster have "different" health scripts for death and damage sound purposes.
    // Should be done during refactoring.
    public void addHealth(float f)
    {

        health = Mathf.Clamp(f + health, -1, maxHealth);
        updateHealthBarVisuals();
        if (health <= 0)
        {
            killObj();
        }

        if (gameObject.CompareTag("Monster"))
        {
            Monster m = gameObject.GetComponent<Monster>();
            if(m != null)
            {
                if (m.state == Monster.State.Patroling)
                    m.aggro();
            }
        }
    }

    void updateHealthBarVisuals()
    {
        healthBar.value = Mathf.Lerp(0, 1, health / maxHealth);
    }

    public void killObj()
    {
        removeMonsterFromList();
        Destroy(healthBar.gameObject);
        Destroy(this.gameObject);
    }

    void removeMonsterFromList()
    {
        if (gameObject.CompareTag("Monster"))
        {
            GameData.monsterGenerator.killMonster(gameObject.GetComponent<Monster>());
        }
    }

}
