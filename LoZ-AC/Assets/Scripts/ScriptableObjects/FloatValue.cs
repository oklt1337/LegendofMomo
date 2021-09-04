using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
    {
        public float initialValue;

        [NonSerialized]
        public float runtimeValue; 

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
