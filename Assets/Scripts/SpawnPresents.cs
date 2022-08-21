using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPresents : MonoBehaviour {

    [SerializeField]
    private GameObject presentPrefab = null;

    [SerializeField]
    private BoxCollider spawnBox = null;

    [SerializeField]
    private float spawnInterval = 5, maxPresents = 10;

    [SerializeField]
    private int initialPresents = 5;

    [SerializeField]
    private float spawnHeightOffGround = .5f;

    private List<GameObject> presents = new List<GameObject>();

    private IEnumerator Start() {
        Spawn(initialPresents);
        while (true) {
            yield return new WaitForSeconds(spawnInterval);
            Spawn(1);
        }
    }

    private void Spawn(int numPresents) {
        presents.RemoveAll(p => p == null);
        for (int i = 0; i < numPresents; i++) {
            if (presents.Count >= maxPresents)
                return;
            //https://forum.unity.com/threads/randomly-generate-objects-inside-of-a-box.95088/#post-1263920
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnBox.size.x, spawnBox.size.x),
                Random.Range(-spawnBox.size.y, spawnBox.size.y),
                Random.Range(-spawnBox.size.z, spawnBox.size.z)
            );
            spawnPosition = spawnBox.transform.TransformPoint(spawnPosition / 2);

            RaycastHit hit;
            if (Physics.Raycast(spawnPosition + Vector3.up * 1000, Vector3.down, out hit))
                presents.Add(Instantiate(presentPrefab, hit.point + Vector3.up * spawnHeightOffGround, Quaternion.identity));
        }
    }

}