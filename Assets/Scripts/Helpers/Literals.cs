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
            CanDoCombo,
            IntroAnimation
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
            OH_Light_Attack_4,
            OH_Light_Attack_5,
            OH_Light_Attack_6,
            OH_Heavy_Attack_1,
            OH_Heavy_Attack_2,
            OH_Heavy_Attack_3,
            OH_Heavy_Attack_4,
            OH_Heavy_Attack_5,
            OH_Heavy_Attack_6,
            Skill_A,
            Skill_B,
            Skill_C,
            Skill_D,
            Skill_E,
            Skill_F,
        }

        public enum ItemNames
        {
            Sword,
            GreatSword,
            Katana,
            Shield,
            Spear
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