using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player_Move player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player_Move>();
    }
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Daño"))
        {
            DecreaseHealthBy(10);
        }
    }
    protected override void Die()
    {
        base.Die();

        player.Die();
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }
}
