using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Bool : ScriptableObject,ISerializationCallbackReceiver
    {
        public bool initialValue;
        public bool runtimeValue; 

        public void OnAfterDeserialize()
        {
            runtimeValue = initialValue;
        }

        public void OnBeforeSerialize()
        {
            
        }
    }
}
