using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class spawner : MonoBehaviour
{
    [SerializeField] public Object grass_field;
    [SerializeField] public Object sheeps_field;
    [SerializeField] public Object wolf_field;

    [SerializeField] public GameObject Prefab_grass;
    [SerializeField] public GameObject Prefab_sheep;
    [SerializeField] public GameObject Prefab_wolf;

    public int grass_amount = 0;
    public int sheeps_amount = 0;
    public int wolfs_amount = 0;

    public void Spawn() {
        grass_amount = int.Parse(grass_field.GetComponent<InputField>().text);
        sheeps_amount = int.Parse(sheeps_field.GetComponent<InputField>().text);
        wolfs_amount = int.Parse(wolf_field.GetComponent<InputField>().text);

        if (grass_amount < 0) grass_amount = 0;
        if (sheeps_amount < 0) sheeps_amount = 0;
        if (wolfs_amount < 0) wolfs_amount = 0;

        for(int i = 0; i < grass_amount; i++) {
            GameObject gameObject = Instantiate(Prefab_grass, new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0), new Quaternion());
        }
        for (int i = 0; i < sheeps_amount; i++) {
            GameObject gameObject = Instantiate(Prefab_sheep, new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) - 2, UnityEngine.Random.Range(-0.5f, 0.5f), 0), new Quaternion());
            gameObject.GetComponent<Entity>().age = UnityEngine.Random.Range(0, 4);
        }
        for (int i = 0; i < wolfs_amount; i++) {
            GameObject gameObject = Instantiate(Prefab_wolf, new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f) + 2, UnityEngine.Random.Range(-0.5f, 0.5f), 0), new Quaternion());
            gameObject.GetComponent<Entity>().age = UnityEngine.Random.Range(0, 4);
        }
    }
}
