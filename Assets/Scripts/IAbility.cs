using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    public void Create(Transform playerTransform, Ability ability, Ability.AbilityConfig abilityConfig);
}