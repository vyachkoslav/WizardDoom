namespace Player
{
    
    public class PlayerEntity : Entity
    {
        public float MaxHealth { get { return maxHealth; } }
        public static PlayerEntity Instance;
        
        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }
    }
}