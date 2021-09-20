using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using CharacterStatModifier;

namespace CharacterStat
{
    [Serializable]
    public class Stat
    {
        public float BaseValue;

        public float Value
        {
            get
            {
                if (isAnOldValue || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    newValue = CalculateFinalValue();
                    isAnOldValue = false;
                }
                return newValue;
            }
        }

        protected bool isAnOldValue = true;
        protected float newValue;
        protected float lastBaseValue = float.MinValue;

        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public Stat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public Stat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatModifier modifier)
        {
            isAnOldValue = true;
            statModifiers.Add(modifier);
            statModifiers.Sort(CompareModifiersOrder);
        }

        public virtual bool RemoveModifier(StatModifier modifier)
        {
            if (statModifiers.Remove(modifier))
            {
                isAnOldValue = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool isRemoved = false;

            for (int i = statModifiers.Count-1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isAnOldValue = true;
                    isRemoved = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return isRemoved;
        }

        public virtual void SetDefaultValue(float defaultValue)
        {
            BaseValue = defaultValue;
        }

        public virtual float GetFinalValue()
        {
            return CalculateFinalValue();
        }

        protected virtual int CompareModifiersOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
            {
                return -1;
            }
            else if (a.Order > b.Order)
            {
                return 1;
            }
            return 0;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier modifier = statModifiers[i];

                if (modifier.Type == StatModifier.StatModType.Flat)
                {
                    finalValue += modifier.Value;
                }
                else if (modifier.Type == StatModifier.StatModType.PercentAdd)
                {
                    sumPercentAdd += modifier.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifier.StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (modifier.Type == StatModifier.StatModType.PercentMult)
                {
                    finalValue *= 1 + modifier.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }

}
