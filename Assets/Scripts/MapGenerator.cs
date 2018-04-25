using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject grid;

    private GameObject[] mapList;
    private int nextMap;

    private void Start() {
        mapList = Resources.LoadAll("Map", typeof(GameObject))
            .Cast<GameObject>()
            .ToArray();
        nextMap = Random.Range(1, mapList.Length);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Player")) {
            GameObject newMap = instantiateNewMap(nextMap);
            newMap.transform.parent = grid.transform;

            Vector3 creationPosition = this.transform.position;
            creationPosition.x += 10.5f;

            newMap.transform.position = creationPosition;

            Vector3 newPosition = this.transform.position;
            newPosition.x += 5.5f;
            this.transform.position = newPosition;

            nextMap = Random.Range(1, mapList.Length);

        }
    }

    public GameObject instantiateNewMap(int map) {
        GameObject goMap = Instantiate(mapList[map]);
        goMap.transform.parent = this.transform;

        return goMap;
    }
}
