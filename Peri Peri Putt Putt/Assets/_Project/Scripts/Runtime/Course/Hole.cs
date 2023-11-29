using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject[] Spawns;
    public int _spawnIndex = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 0.0f)
        {
            if (_spawnIndex == Spawns.Length - 1)
            {
                _spawnIndex = 0;
            }
            else
            {
                _spawnIndex++;
            }
            other.gameObject.transform.position = Spawns[_spawnIndex].transform.position;
        }
    }
}