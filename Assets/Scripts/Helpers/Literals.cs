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
            Enemy
        }

        public enum UserTypes
        {
            Player,
            FreeSpectator
        }

        public enum ScriptableObjects
        {
            //PlayerData
        }

    }

    internal static class LiteralToStringParse
    {
        //Tags
        //public static readonly string MainCamera = Tags.MainCamera.ToString();
        //public static readonly string Weapon = Tags.Weapon.ToString();

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
        

    }
}