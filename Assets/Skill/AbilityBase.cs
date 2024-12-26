using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour, IAbility {
    public abstract void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig);

    public Vector3 GetPlayerPosition() {
        return Mouse.Instance.GetPosition();
    }

    public Enemy GetClosestEnemyInRange() {
        return Enemy.GetClosestEnemyInRange(GetPlayerPosition());
    }

    public Enemy GetRandomEnemyInRange() { 
        return Enemy.GetRandomEnemyInRange();
    }
}
