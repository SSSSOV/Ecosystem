using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class mouse : MonoBehaviour
{
    [SerializeField] public GameObject m_target;
    [SerializeField] public TextMeshProUGUI m_textMeshPro;

    void Print_Info(GameObject gameObject) {
        m_textMeshPro.text = "Name: " + gameObject.name.ToString() + "\n";
        m_textMeshPro.text += "Pos: " + gameObject.transform.position.ToString();
        string stats = "\nStats: \n";

        Entity entity = gameObject.GetComponent<Entity>();
        if (entity == null) return;
        // Получаем все публичные поля объекта и добавляем их в строку для отображения
        foreach (var field in entity.GetType().GetFields()) {
            stats += field.Name + ": " + field.GetValue(entity).ToString() + "\n";
        }
        m_textMeshPro.text += stats;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null) {
                Debug.Log("Нажали на: " + hit.collider.gameObject.name);
                m_target = hit.collider.gameObject;
            }
            else m_target = null;
        }
        if (m_target != null) Print_Info(m_target);
        else m_textMeshPro.text = "Select a entity.";
    }
    public void Close() {
        Application.Quit();
    }
}
