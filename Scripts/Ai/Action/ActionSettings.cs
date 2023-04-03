using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Action
{
    public class ActionSettings<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _settings;
        public ActionSettings() {
            _settings = new Dictionary<TKey, TValue>();
        }

        public TValue this[TKey setting]
        {
            get => _settings[setting];
            set { _settings[setting] = value; } 
        } 
    }
}