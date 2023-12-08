using UnityEngine;

namespace Assets.Classes {
    class Wolf : Entity {

        System.Random random = new System.Random();

        protected override void Start() {
            base.Start();
            speed = 1.5f;
            hunger_rate = 0.1f;
            hunger_reprod = 0.8f;
        }

        protected override void SearchForFood() {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

            float closest_distance = float.MaxValue;
            GameObject closestFood = null;
            Debug.Log("Ищу овцу.");

            foreach (Collider2D col in colliders) {
                if (col.CompareTag("sheep")) {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closest_distance) {
                        closest_distance = distance;
                        closestFood = col.gameObject;
                    }
                }
            }
            if (closestFood != null) {
                Debug.Log("Овца найдена.");
                target_food = closestFood;
            }
        }
        protected override void SearchForReprod() {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

            float closest_distance = float.MaxValue;
            GameObject closest_partner = null;
            Debug.Log("Ищу партнера.");

            foreach (Collider2D col in colliders) {
                if (col.CompareTag("wolf") && col != this.GetComponent<Collider2D>() && col.GetComponent<Entity>().isReprod == true) {
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
