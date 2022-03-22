namespace Helpers
{
    public static class Literals
    {
        public enum PlayerStates
        {
            IdleWalkRunBlend,
            Roll,
            JumpLand,
            InAir,
            JumpStart,
            FirstAttack,
            SecondAttack,
            ThirdAttack,
            FourthAttack
        }

        public enum PlayerParameters
        {
            Speed,
            Jump,
            Grounded,
            FreeFall,
            MotionSpeed,
            Attack,
            Roll,
            AttackCount
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