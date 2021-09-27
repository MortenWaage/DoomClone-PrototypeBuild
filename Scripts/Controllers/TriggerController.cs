using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    int baronCount = 0;

    [SerializeField] E_EnemyController[] trigger_Enemies;
    [SerializeField] O_Door exit_door;

    private void Start()
    {
        StartCoroutine("PeriodicChecks");
    }

    IEnumerator PeriodicChecks()
    {
        yield return new WaitForSeconds(2f);

        baronCount = 0;

        foreach (E_EnemyController enemy in trigger_Enemies)
            if (enemy.IsDead)
                baronCount++;

        GameController.Instance.debug.SetText(1, "Bosses killed: " + baronCount);

        if (baronCount == 2)
            exit_door.TriggerDoor();
        else
            exit_door.Close();

        StartCoroutine("PeriodicChecks");
    }
}
