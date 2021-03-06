#nullable enable
using Godot;
using KoreDefenceGodot.Core.Scripts.Enemy;
using KoreDefenceGodot.Core.Scripts.Engine.Game;

namespace KoreDefenceGodot.Core.Scripts.Player
{
	public abstract class PlayerBase : Area2D
	{
		private const int DefaultHealth = 1000;
		private AnimatedSprite? _baseSprite;
		private ShaderMaterial? _shader;
		private bool _takingDmg;
		private float _time;
		private int Health { get; set; }
		
		[Signal]
		public delegate void HealthChanged(int newHealth);

		public void Setup(int x, int y, int health = DefaultHealth)
		{
			Health = health;
			Position = new Vector2(x, y);
		}

		public void Setup(Vector2 pos, int health = DefaultHealth)
		{
			Health = health;
			Position = pos;
		}

		public override void _Ready()
		{
			_baseSprite = GetNode<AnimatedSprite>("Base");
			_baseSprite.Play("BaseNormal");
			_shader = _baseSprite.Material as ShaderMaterial;
			GameInfo.PlayerBase = this;
		}

		public override void _Process(float delta)
		{
			UpdateDamage(delta);
			if (IsDead()) return;
			if (Health > DefaultHealth * 0.33 && Health < DefaultHealth * 0.66) _baseSprite?.Play("BaseDamage1");

			if (Health < DefaultHealth * 0.33) _baseSprite?.Play("BaseDamage2");
		}

		public void Damage(int amount)
		{
			var newHealth = Health - amount;
			Health = newHealth < 0 ? 0 : newHealth;
			_takingDmg = true;
			EmitSignal(nameof(HealthChanged), Health);
		}

		private void UpdateDamage(float delta)
		{
			_time += delta;
			const float animTime = 0.1f;
			if (!(_time > animTime)) return;
			_time = 0;
			
			// responsible for the "damage flash" effect
			if (_takingDmg && (int) (_shader?.GetShaderParam("FlashStatus") ?? false) == 0)
			{
				_shader?.SetShaderParam("FlashStatus", 1);
				_takingDmg = false;
			}
			else
			{
				_shader?.SetShaderParam("FlashStatus", 0);
			}
		}

		public bool IsDead()
		{
			return Health <= 0;
		}

		protected virtual void _OnBodyEntered(object body)
		{
			if (body is BaseEnemy enemy) enemy.EnemyReachedBase();
		}


	}
}
