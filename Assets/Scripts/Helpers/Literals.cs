using static Helpers.Literals;

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

        public enum AnimatorLayers
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
            OH_Heavy_Attack_7,
            Skill_A,
            Skill_B,
            Skill_C,
            Skill_D,
            Skill_E,
            Skill_F,
            InitialState
        }

        public enum DamageAnimations
        {
            Damage_01,
            Damage_Die,
            Damage_Die_Loop
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
            Weapon,
            Player,
            Dummy,
            Enemy,
            InWorldHealthBar,
            OnScreenHealthBar
        }

        public enum UserTypes
        {
            Player,
            FreeSpectator
        }

    }

    internal static class LiteralToStringParse
    {
        //Tags
        //public static readonly string MainCamera = Tags.MainCamera.ToString();
        internal static readonly string Player = Tags.Player.ToString();

        //ScriptableObjects
        internal const string PlayerDataPath = "ScriptableObjects/Player/Data";
        internal const string PlayerData = "PlayerData";
        
        //Photon && Method names
        internal const string UserType = "user_type";
        internal const string PlayTargetAnimation = "PlayTargetAnimation";
        internal const string EnableCombo = "EnableCombo";
        
        //Scenes
        internal const string LoadingScreen = "LoadingScreen";
        internal const string OnlineScene = "OnlineScene";
        internal const string SampleScene = "SampleScene";
        
        //AttackAnimations
        // internal static readonly string OH_Light_Attack_1 = Literals.AttackAnimations.OH_Light_Attack_1.ToString();
        // internal static readonly string OH_Light_Attack_2 = Literals.AttackAnimations.OH_Light_Attack_2.ToString();
        // internal static readonly string OH_Light_Attack_3 = Literals.AttackAnimations.OH_Light_Attack_3.ToString();
        // internal static readonly string OH_Light_Attack_4 = Literals.AttackAnimations.OH_Light_Attack_4.ToString();
        // internal static readonly string OH_Light_Attack_5 = Literals.AttackAnimations.OH_Light_Attack_5.ToString();
        // internal static readonly string OH_Light_Attack_6 = Literals.AttackAnimations.OH_Light_Attack_6.ToString();
        // internal static readonly string OH_Heavy_Attack_1 = Literals.AttackAnimations.OH_Heavy_Attack_1.ToString();
        // internal static readonly string OH_Heavy_Attack_2 = Literals.AttackAnimations.OH_Heavy_Attack_2.ToString();
        // internal static readonly string OH_Heavy_Attack_3 = Literals.AttackAnimations.OH_Heavy_Attack_3.ToString();
        // internal static readonly string OH_Heavy_Attack_4 = Literals.AttackAnimations.OH_Heavy_Attack_4.ToString();
        // internal static readonly string OH_Heavy_Attack_5 = Literals.AttackAnimations.OH_Heavy_Attack_5.ToString();
        // internal static readonly string OH_Heavy_Attack_6 = Literals.AttackAnimations.OH_Heavy_Attack_6.ToString();
        // internal static readonly string OH_Heavy_Attack_7 = Literals.AttackAnimations.OH_Heavy_Attack_7.ToString();
        // internal static readonly string Skill_A = Literals.AttackAnimations.Skill_A.ToString();
        // internal static readonly string Skill_B = Literals.AttackAnimations.Skill_B.ToString();
        // internal static readonly string Skill_C = Literals.AttackAnimations.Skill_C.ToString();
        // internal static readonly string Skill_D = Literals.AttackAnimations.Skill_D.ToString();
        // internal static readonly string Skill_E = Literals.AttackAnimations.Skill_E.ToString();
        // internal static readonly string Skill_F = Literals.AttackAnimations.Skill_F.ToString();

    }
}