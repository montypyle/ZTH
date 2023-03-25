using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMGR : MonoBehaviour
{
    public static ObstacleMGR instance;
    public GameObject goalObj, bonus;
    public GameObject[] obstacles;
    void Start()
    {
        instance = this;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            ObstacleCtrl script = obstacle.GetComponent<ObstacleCtrl>();
            SpriteRenderer barrier = obstacle.GetComponentInChildren<SpriteRenderer>();
            script.maxHealth = (int)barrier.size.x;
            script.obsHealth = script.maxHealth;
        }
    }
    void Update()
    {
        foreach (GameObject obstacle in obstacles)
        {
            ObstacleCtrl script = obstacle.GetComponent<ObstacleCtrl>();
            SpriteRenderer barrier = obstacle.GetComponentInChildren<SpriteRenderer>();
            script.obsHealth = Mathf.Clamp(script.obsHealth, 0, script.maxHealth);
            barrier.size = new Vector2(script.obsHealth, script.obsHealth);
        }

    }
    public void SetReward(GameObject obstacle)
    {
        if (obstacle.gameObject.name == "Goal")
        {
            ObjectDestroyed(goalObj, obstacle);
        }
        if (obstacle.gameObject.name == "Reward")
        {
            ObjectDestroyed(bonus, obstacle);
        }

    }
    private void ObjectDestroyed(GameObject toSpawn, GameObject obstacle)
    {
        ObstacleCtrl script = obstacle.GetComponent<ObstacleCtrl>();
        if (!script.cleared)
        {
            Instantiate(toSpawn, obstacle.transform);
        }
        script.DisableObject();

    }


}

