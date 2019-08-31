using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class AttackTypeFactory
{
    private static Dictionary<string, Type> attackTypes = new Dictionary<string, Type>();

    public AttackTypeFactory()
    {
        var types = Assembly.GetAssembly(typeof(AttackType)).GetTypes().Where(mType => mType.IsClass && !mType.IsAbstract && mType.IsSubclassOf(typeof(AttackType)));

        foreach (var type in types)
        {
            var tempType = Activator.CreateInstance(type) as AttackType;
            attackTypes.Add(tempType.GetName(), type);
        }
    }

    public AttackType GetAttackType(string attackType)
    {
        if(attackTypes.ContainsKey(attackType))
        {
            Type type = attackTypes[attackType];
            var typ = Activator.CreateInstance(type) as AttackType;
            return typ;
        }

        return null;
    }

    internal IEnumerable<string> GetAttackTypes()
    {
        return attackTypes.Keys;
    }
}
