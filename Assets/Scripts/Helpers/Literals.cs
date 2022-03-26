namespace Helpers
{
    public static class Literals
    {
        public enum AnimatorStates
        {
            IdleWalkRunBlend,
            Roll,
            Rolls,
            BackStep,
            JumpLand,
            InAir,
            JumpStart,
        }

        public enum AnimatorParameters
        {
            Speed,
            Jump,
            Grounded,
            FreeFall,
            MotionSpeed,
            IsInteracting,
            Horizontal,
            Vertical,
            CanDoCombo
        }

        public enum PlayerLayers
        {
            BaseLayer = 0,
            Override = 1,
        }
        
        public enum AttackAnimations
        {
            OH_Light_Attack_1,
            OH_Light_Attack_2,
            OH_Light_Attack_3,
            OH_Heavy_Attack_1,
            OH_Heavy_Attack_2,
            OH_Heavy_Attack_3
        }

        public enum ItemNames
        {
            Sword
        }

        public enum SliderBarStates
        {
            Idle,
            Processing,
            Returning,
            Cooldown
        }

        public enum Tags
        {
            MainCamera,
            Weapon
        }

    }
}