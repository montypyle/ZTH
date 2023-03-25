using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCtrl : MonoBehaviour
{
    public int obsHealth, maxHealth, spawninterval, spawnradius, targetLife;
    public Transform spawnpos;
    public GameObject target;
    public bool cleared;

    private void Start()
    {
        cleared = false;
    }
    public void ToggleCoroutine(bool on)
    {
        Coroutine coroutine = StartCoroutine(SpawnObstacles());
        if (on)
        {
            StartCoroutine(SpawnObstacles());
        }
        else
        {
            StopCoroutine(coroutine);
        }
    }
   
    public void DisableObject()
    {
        ToggleCoroutine(false);
        cleared = true;
        SpriteRenderer barrier = this.GetComponentInChildren<SpriteRenderer>();
        barrier.enabled = false;

    }
    IEnumerator SpawnObstacles()
    {
        while (MissionCtrl.instance.gameActive && !cleared)
        {
                yield return new WaitForSeconds(spawninterval / 10);
                Vector2 obsPos = this.transform.position;
                Vector2 randOffset = Random.insideUnitCircle * spawnradius;
                Vector2 currentspawn = obsPos + randOffset;
                spawnpos.position = currentspawn;
                TargetCtrl targScript = target.GetComponent<TargetCtrl>();
                targScript.source = this.gameObject;
                Instantiate(target, spawnpos);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.CompareTag("Player"))
            {
                ToggleCoroutine(true);
            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
            if (collision.gameObject.CompareTag("Player"))
            {
                ToggleCoroutine(false);
            }
    }
}
