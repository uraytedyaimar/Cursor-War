using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    [SerializeField] private int experience;

    private void OnTriggerEnter2D(Collider2D collision) {
        Mouse mouse = collision.GetComponent<Mouse>();
        if (mouse != null) { 
            mouse.AddExperience(experience);
            Destroy(gameObject);
        }
    }
}
