using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStarter : MonoBehaviour
{
    public Rigidbody2D rb;
    public float thrust;
    public SpriteRenderer spriteRenderer;
    public Sprite[] rpsIcons;
    public string[] teams = {"rock", "paper", "scissors"};
    public string team = "rand";
    private string targetTeam;
    public GameObject currentTarget;
    public Vector3 direction;

    void Start()
    {
        if (team == "rand")
        {
            team = teams[Random.Range(0, teams.Length)];
        }
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        Vector2 randomDirection = new Vector2(randomX, randomY);
        rb.AddForce(randomDirection * thrust);
        if (team == "rock")
        {
            targetTeam = "scissors";
        }
        else if (team == "paper")
        {
            targetTeam = "rock";
        }
        else if (team == "scissors")
        {
            targetTeam = "paper";
        }
        InvokeRepeating("Attack", 0.1f, 0.5f);
    }
    void Update()
    {
        if (team == "rock")
        {
            spriteRenderer.sprite = rpsIcons[0];
        }
        else if (team == "paper")
        {
            spriteRenderer.sprite = rpsIcons[1];
        }
        else if (team == "scissors")
        {
            spriteRenderer.sprite = rpsIcons[2];
        }
        ObjectStarter[] objectStarters = FindObjectsOfType<ObjectStarter>();
        float closestDistance = float.MaxValue;
        ObjectStarter closestObject = null;

        foreach (ObjectStarter otherObject in objectStarters)
        {
            if (otherObject != this && otherObject.team == targetTeam)
            {
                float distance = Vector3.Distance(transform.position, otherObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = otherObject;
                }
            }
        }
        if (closestObject != null)
        {
            currentTarget = closestObject.gameObject;
        }
        else
        {
            currentTarget = null;
        }
    }
    void Attack()
    {
        if (currentTarget != null)
        {
            direction = currentTarget.transform.position - transform.position;
            rb.AddForce(direction.normalized * thrust/100, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ObjectStarter otherCircle = other.GetComponent<ObjectStarter>();
        if (otherCircle != null && otherCircle.team != team)
        {
            // Debug.Log(otherCircle.team);
            if (team == "paper")
            {
                if (otherCircle.team == "rock")
                {
                    otherCircle.team = team;
                }
                else if (otherCircle.team == "scissors")
                {
                    team = otherCircle.team;
                }
            }
            else if (team == "rock")
            {
                if (otherCircle.team == "paper")
                {
                    team = otherCircle.team;
                }
                else if (otherCircle.team == "scissors")
                {
                    otherCircle.team = team;
                }
            }
            else if (team == "scissors")
            {
                if (otherCircle.team == "rock")
                {
                    team = otherCircle.team;
                }
                else if (otherCircle.team == "paper")
                {
                    otherCircle.team = team;
                }
            }
        }
    }
}
