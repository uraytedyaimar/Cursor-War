using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour, IAbility {
    public abstract void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig);
}
