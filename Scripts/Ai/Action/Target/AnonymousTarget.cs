using System.Collections;
using UnityEngine;

namespace AI.Action.TargetSelection   
{
    public class AnonymousTarget : ITarget
    {
        private Vector3 _position;

        public Vector3 Position
        {
            get { return _position; }            
        }

        public AnonymousTarget(Vector3 position) {
            _position = position;
        }
    }
}