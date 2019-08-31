using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class SkillTypeFactory
{
    private static Dictionary<string, Type> skillTypes = new Dictionary<string, Type>();

    public SkillTypeFactory()
    {
        var types = Assembly.GetAssembly(typeof(SkillType)).GetTypes().Where(mType => mType.IsClass && !mType.IsAbstract && mType.IsSubclassOf(typeof(SkillType)));

        foreach (var type in types)
        {
            var tempType = Activator.CreateInstance(type) as SkillType;
            skillTypes.Add(tempType.GetName(), type);
        }
    }

    public SkillType GetSkillType(string attackType)
    {
        if (skillTypes.ContainsKey(attackType))
        {
            
            Type type = skillTypes[attackType];
            var typ = Activator.CreateInstance(type) as SkillType;
            return typ;
        }

        return null;
    }

    internal IEnumerable<string> GetSkillTypes()
    {
        return skillTypes.Keys;
    }
}
