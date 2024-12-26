using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : AbilityBase
{
    public override void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig) {
        Instantiate(ability.abilityPrefab, playerTransform.position, Quaternion.identity);
    }
}
