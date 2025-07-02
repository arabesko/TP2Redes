using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class LifeHandler : NetworkBehaviour
{
    [SerializeField] private float _maxLife;
    [Networked] private float _currentLife { get; set; }
    [SerializeField] private RawImage _barLife;

    //[Networked, OnChangedRender(nameof(BarLife))] private NetworkBool RefreshBarLife { get; set; }

    public override void Spawned()
    {
        _currentLife = _maxLife;
    }

    public void TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        if (damage > _currentLife) _currentLife = 0;
        
        _currentLife -= damage;

        if (_currentLife != 0) return;

        Die();

    }

    public override void Render()
    {
        BarLife();
    }

    void BarLife()
    {
        _barLife.rectTransform.localScale = new Vector3(_currentLife / _maxLife, 1, 1);
    }

    private void Die()
    {
        if (!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }

        if (Object != null)
        {
            Runner.Despawn(Object);
        }

        
    }

}
