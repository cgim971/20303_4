using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{

    public GameObject _monster;

    private void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            GameObject monster = Instantiate(_monster, null);
            monster.transform.position = transform.position;
            monster.transform.position += Vector3.up;
        }
    }

}
