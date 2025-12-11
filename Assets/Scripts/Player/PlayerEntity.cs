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

        public void SetMaxHealth(float health)
        {
            maxHealth = health;
        }
        public override void ApplyDamage(float damage)
        {
            base.ApplyDamage(damage);
            SoundManager.Instance.PlaySound2D("PlayerDamage");
        }
    }
}