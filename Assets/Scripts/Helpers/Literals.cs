namespace Helpers
{
    public static class Literals
    {
        public enum PlayerStates
        {
            IdleWalkRunBlend,
            Roll,
            BackStep,
            JumpLand,
            InAir,
            JumpStart,
            FirstAttack,
            SecondAttack,
            ThirdAttack,
            FourthAttack
        }

        public static class PlayerStatesStrings
        {
            public static readonly string IdleWalkRunBlend = PlayerStates.IdleWalkRunBlend.ToString();
            public static readonly string Roll = PlayerStates.Roll.ToString();
            public static readonly string BackStep = PlayerStates.BackStep.ToString();
            public static readonly string JumpLand = PlayerStates.JumpLand.ToString();
            public static readonly string InAir = PlayerStates.InAir.ToString();
            public static readonly string JumpStart = PlayerStates.JumpStart.ToString();
            public static readonly string FirstAttack = PlayerStates.FirstAttack.ToString();
            public static readonly string SecondAttack = PlayerStates.SecondAttack.ToString();
            public static readonly string ThirdAttack = PlayerStates.ThirdAttack.ToString();
            public static readonly string FourthAttack = PlayerStates.FourthAttack.ToString();
        }

        public enum PlayerParameters
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
            HeavyAttack
        }

        public class PlayerParametersStrings
        {
            public static readonly string Speed = PlayerParameters.Speed.ToString();
            public static readonly string Jump = PlayerParameters.Jump.ToString();
            public static readonly string Grounded = PlayerParameters.Grounded.ToString();
            public static readonly string FreeFall = PlayerParameters.FreeFall.ToString();
            public static readonly string MotionSpeed = PlayerParameters.MotionSpeed.ToString();
            public static readonly string isInteracting = PlayerParameters.isInteracting.ToString();
            public static readonly string Attack = PlayerParameters.Attack.ToString();
            public static readonly string Roll = PlayerParameters.Roll.ToString();
            public static readonly string AttackCount = PlayerParameters.AttackCount.ToString();
            public static readonly string NormalAttack = PlayerParameters.NormalAttack.ToString();
            public static readonly string HeavyAttack = PlayerParameters.HeavyAttack.ToString();
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