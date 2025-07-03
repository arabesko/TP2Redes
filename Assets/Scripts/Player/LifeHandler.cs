using Fusion;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeHandler : NetworkBehaviour
{
    [SerializeField] private float _maxLife;
    [SerializeField] [Networked] private float _currentLife { get; set; }
    [SerializeField] private RawImage _barLife;
    private GameObject _deathText;

    [Networked] private NetworkBool PlayerDied { get; set; }

    public override void Spawned()
    {
        _currentLife = _maxLife;
    }

    public void TakeDamage(float damage)
    {
        if (!HasStateAuthority) return;

        if (damage > _currentLife)
        {
            _currentLife = 0;
        }
        else
        {
            _currentLife -= damage;
        }
        
        if (_currentLife > 0) return;

        PlayerDied = true;
    }


    public override void Render()
    {
        BarLife();
        CheckDeathUI();
    }

    private void CheckDeathUI()
    {
        // Solo ejecutar para el jugador local que murió
        if (Object.HasInputAuthority && PlayerDied)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowDeathUI();
                Die();
                PlayerDied = false; // Resetear para evitar múltiples activaciones
            }
        }
    }

    void BarLife()
    {
        _barLife.rectTransform.localScale = new Vector3(_currentLife / _maxLife, 1, 1);
    }

    //private void Die()
    //{
    //    if (!Object.HasInputAuthority)
    //    {
    //        Runner.Disconnect(Object.InputAuthority);
    //    }

    //    if (Object != null)
    //    {
    //        Runner.Despawn(Object);
    //    }
    //}

    private void Die()
    {
        if (Object != null && Object.IsValid)
        {
            // Solo el host puede despawnear objetos de red
            if (Runner.IsServer)
            {
                Runner.Despawn(Object);
            }
            else if (Object.HasInputAuthority)
            {
                // Clientes solicitan al host que los despawnee
                RPC_RequestDespawn();
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestDespawn()
    {
        if (Object != null && Object.IsValid)
        {
            Runner.Despawn(Object);
        }
    }
}
