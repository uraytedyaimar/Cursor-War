using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParticleSystem : MonoBehaviour
{
    private ParticleSystem deadParticleSystem;

    private void Awake() {
        deadParticleSystem = GetComponent<ParticleSystem>();

        float duration = deadParticleSystem.main.duration;
        Destroy(gameObject, duration);
    }
}
