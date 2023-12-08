using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Classes {
    class Sheep : Entity {

        protected override void Start() {
            base.Start();
            speed = 1f;
            hunger_rate = 0.05f;
            hunger_reprod = 0.5f;
        }

        protected override void SearchForFood() {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

            float closest_distance = float.MaxValue;
            GameObject closest_food = null;
            Debug.Log("Ищу еду.");

            foreach (Collider2D col in colliders) {
                if (col.CompareTag("grass")) {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closest_distance) {
                        closest_distance = distance;
                        closest_food = col.gameObject;
                    }
                }
            }
            if (closest_food != null) {
                Debug.Log("Еда найдена.");
                target_food = closest_food;
            }
        }

        protected override void SearchForReprod() {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

            float closest_distance = float.MaxValue;
            GameObject closest_partner = null;
            Debug.Log("Ищу партнера.");

            foreach (Collider2D col in colliders) {
                if (col.CompareTag("sheep") && col != this.GetComponent<Collider2D>() && col.GetComponent<Entity>().isReprod == true) {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closest_distance) {
                        closest_distance = distance;
                        closest_partner = col.gameObject;
                    }
                }
            }
            if (closest_partner != null) {
                Debug.Log("Партнер найдена.");
                target_partner = closest_partner;
            }
        }
    }
}
