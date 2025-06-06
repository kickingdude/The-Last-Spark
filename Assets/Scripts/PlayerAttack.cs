
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // get first child of game object
        attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
        else if (Input.GetKeyDown("w")){
            Attack();
        }
        else if (Input.GetKeyDown("e")){
            Attack();
        }
        else if (Input.GetKeyDown("r")){
            Attack();
        }

        // reset timer for attacking
        if (attacking)
        {
            timer += Time.deltaTime;

            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }

        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
}
