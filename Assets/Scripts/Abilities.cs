using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType {Speed, Invunrable, None};
public class Abilities
{
    AbilityType[] abilityTypes = new AbilityType[2];
    public AbilityType GetAbility()
    {
        abilityTypes[0] = AbilityType.Speed;
        abilityTypes[1] = AbilityType.Invunrable;
        return abilityTypes[Random.Range(0,2)];
    }
}
