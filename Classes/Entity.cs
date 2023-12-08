using Assets.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Entity : MonoBehaviour {

    System.Random random = new System.Random();

    public float speed = 0.5f;
    public float hunger_max = 1f;
    public float hunger_rate = 0.1f;
    public float hunger_reprod = 0.6f;
    public float hunger;
    public int age = 0;
    public int age_max = 100;

    protected GameObject target_food;
    [SerializeField] protected GameObject target_partner;
    public Vector2 target_pos = Vector2.zero;

    public bool isMove;
    public bool isSleeping;
    public bool isHunger;
    public bool isReprod;
    protected Rigidbody2D rb;

    protected virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        hunger = hunger_max;
        age = random.Next(10);
        isSleeping = false;
        isMove = false;
        isHunger = false;
        isReprod = false;
        target_food = null;
        target_partner = null;
        target_pos = Vector2.zero;
        InvokeRepeating("UpdateHunger", 1f, 1f);
        InvokeRepeating("UpdateAge", 1f, 1f);
    }

    protected virtual void FixedUpdate() {
        if (isMove && target_pos != Vector2.zero) {
            Move(target_pos);
            return;
        } else rb.velocity = Vector2.zero;
        if (isHunger && target_food == null) {
            SearchForFood();
        }
        else if (isHunger && target_food != null) {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), target_food.transform.position) < 0.2f) {
                Eat(target_food);
            }
            else {
                target_pos = target_food.transform.position;
                Move(target_pos); 
            }
        }
        else if (isReprod && target_partner == null) {
            SearchForReprod();
            if (target_partner == null) RandomMove();
        }
        else if(isReprod && target_partner != null) {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), target_partner.transform.position) < 0.2f) {
                Reprod(target_partner);
            }
            else {
                target_pos = target_partner.transform.position;
                Move(target_pos);
            }
        }
        else RandomMove();
    }
    protected virtual void RandomMove() {
        isMove = true;
        target_pos = (Vector2)transform.position + new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        Move(target_pos);
    }
    protected virtual void Move(Vector2 target) {
        Vector2 moveDirection;
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), target) < 0.2f) {
            isMove = false;
            target_pos = Vector2.zero;
            Debug.Log("Дошел до точки: " + target.ToString());
            moveDirection = Vector2.zero;
        }
        else moveDirection = (target - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = moveDirection * speed;
    }
    protected virtual void UpdateHunger() {
        hunger -= hunger_rate;
        if(hunger >= hunger_max * hunger_reprod) {
            isReprod = true;
        }
        if (hunger <= hunger_max * 0.25) {
            isHunger = true;
        }
        if(hunger <= 0) {
            Destroy(gameObject);
        }
    }
    protected virtual void UpdateAge() {
        age++;
        if(age >= age_max) Destroy(gameObject);
    }
    protected virtual void SearchForFood() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        float closest_distance = float.MaxValue;
        GameObject closest_food = null;
        Debug.Log("Ищу еду.");

        foreach (Collider2D col in colliders) {
            if (col.CompareTag("food")) {
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
    protected virtual void SearchForReprod() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        float closest_distance = float.MaxValue;
        GameObject closest_partner = null;
        Debug.Log("Ищу партнера.");

        foreach (Collider2D col in colliders) {
            if (col.CompareTag("entity") && col != this.GetComponent<Collider2D>() && col.GetComponent<Entity>().isReprod == true) {
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
    protected virtual void Eat(GameObject food) {
        Destroy(food);
        hunger = hunger_max;
        isHunger = false;
        Debug.Log("Покушал.");
    }
    protected virtual void Reprod(GameObject partner) {
        Entity entity = partner.GetComponent<Entity>();
        if (entity.age < age) {
            GameObject child = Instantiate(gameObject);
            child.GetComponent<Entity>().age = random.Next(0, 4);
            isReprod = false;
            entity.isReprod = false;
            hunger = hunger_max * 0.5f;
            entity.hunger = hunger_max * 0.5f;
        }
    }
}
