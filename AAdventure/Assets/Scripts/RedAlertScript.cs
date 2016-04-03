using UnityEngine;
using System.Collections;

public class RedAlertScript : MonoBehaviour {
    PlayerBehaviourScript playerScript;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        //playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>();
        if (playerScript != null && playerScript.hasTreasure)
        {
            Debug.Log(" treasure");
            animator.SetTrigger("TreasureFound");
        }
    }
}
