using System.Numerics;

public interface IWeapon
{
    public int CurrentAmmo { get; }
    public int MaxAmmo { get; }

    public bool CanShoot { get; }
    public bool CanReload { get; }

    public bool Shoot(Vector3 direction);
    public bool Reload();
}