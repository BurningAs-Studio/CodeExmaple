namespace FAS.Projectiles
{
	public class ProjectilePool : ObjectPool<Projectile>
	{
		protected override void InitializeObject(Projectile projectile)
		{
			projectile.Initialize();
			projectile.OnMoveComplete += ReturnToPool;
		}

		protected override void CleanupObject(Projectile projectile)
		{
			projectile.OnMoveComplete -= ReturnToPool;
		}
	}
}