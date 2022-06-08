using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    float hp = 10;
    public float speed = 10;
    CharacterController characterController;

    GameObject target;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        target = FindObjectOfType<PlayerMovement>().gameObject;

        Vector3 nextPos = target.transform.position - transform.position;
        nextPos.Normalize();
        nextPos *= speed * Time.deltaTime;

        Vector3 newForward = characterController.velocity;
        newForward.y = 0;

        if (newForward.magnitude > 0)
            transform.forward = Vector3.Lerp(transform.forward, newForward, 5 * Time.deltaTime);

        if (!characterController.isGrounded)
        {
            nextPos.y -= 9.8f * Time.deltaTime;
        }

        characterController.Move(nextPos);
    }
    public void TakeDamage(float damage)
    {
        hp -= damage;

        Debug.Log(hp);

        if (hp <= 0)
        {
            Debug.Log("Á¦°Å");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("A");
            transform.GetComponent<Renderer>().material.DOColor(Color.red, 3);
        }
    }

}
