namespace Player
{
    public class PlayerEntity : Entity
    {
        public static PlayerEntity Instance;
        
        protected override void Awake()
        {
            Instance = this;
            base.Awake();
        }
    }
}