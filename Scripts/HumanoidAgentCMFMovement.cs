using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CMF;

public class HumanoidAgentCMFMovement : AdvancedWalkerController, IAgentMovement
{
    protected override void Setup()
    {
        characterInput = new SlaveCharacterInput();
        //print(characterInput);
    }
    public void Move(Vector3 movementVector)
    {
        var charInput = (SlaveCharacterInput) characterInput;
        charInput.SetMovementVector(movementVector);
    }

    private class SlaveCharacterInput : ICharacterInput
    {
        private Vector3 _movVector;

        public float GetHorizontalMovementInput()
        {
            return _movVector.x;
        }

        public float GetVerticalMovementInput()
        {
            return _movVector.z;
        }

        public bool IsJumpKeyPressed()
        {
            return _movVector.y > 0f;
        }

        public void SetMovementVector(Vector3 movementVector)
        {
            _movVector = movementVector.normalized;
        }
    }
}
