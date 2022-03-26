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
            FirstAttack,
            SecondAttack,
            ThirdAttack,
            FourthAttack
        }

        public static class AnimatorStatesStrings
        {
            public static readonly string IdleWalkRunBlend = AnimatorStates.IdleWalkRunBlend.ToString();
            public static readonly string Roll = AnimatorStates.Roll.ToString();
            public static readonly string BackStep = AnimatorStates.BackStep.ToString();
            public static readonly string JumpLand = AnimatorStates.JumpLand.ToString();
            public static readonly string InAir = AnimatorStates.InAir.ToString();
            public static readonly string JumpStart = AnimatorStates.JumpStart.ToString();
            public static readonly string FirstAttack = AnimatorStates.FirstAttack.ToString();
            public static readonly string SecondAttack = AnimatorStates.SecondAttack.ToString();
            public static readonly string ThirdAttack = AnimatorStates.ThirdAttack.ToString();
            public static readonly string FourthAttack = AnimatorStates.FourthAttack.ToString();
        }

        public enum AnimatorParameters
        {
            Speed,
            Jump,
            Grounded,
            FreeFall,
            MotionSpeed,
            isInteracting,
            Attack,
            Roll,
            AttackCount,
            NormalAttack,
            HeavyAttack,
            Horizontal,
            Vertical
        }

        public class AnimatorParametersStrings
        {
            public static readonly string Speed = AnimatorParameters.Speed.ToString();
            public static readonly string Jump = AnimatorParameters.Jump.ToString();
            public static readonly string Grounded = AnimatorParameters.Grounded.ToString();
            public static readonly string FreeFall = AnimatorParameters.FreeFall.ToString();
            public static readonly string MotionSpeed = AnimatorParameters.MotionSpeed.ToString();
            public static readonly string isInteracting = AnimatorParameters.isInteracting.ToString();
            public static readonly string Attack = AnimatorParameters.Attack.ToString();
            public static readonly string Roll = AnimatorParameters.Roll.ToString();
            public static readonly string AttackCount = AnimatorParameters.AttackCount.ToString();
            public static readonly string NormalAttack = AnimatorParameters.NormalAttack.ToString();
            public static readonly string HeavyAttack = AnimatorParameters.HeavyAttack.ToString();
        }
        
        public enum PlayerLayers
        {
            BaseLayer = 0,
            Rolls = 1,
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
        
        public class TagsStrings
        {
            public static readonly string MainCamera = Tags.MainCamera.ToString();
            public static readonly string Weapon = Tags.Weapon.ToString();
        }
    }
}