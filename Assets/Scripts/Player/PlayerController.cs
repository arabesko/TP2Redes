using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooting))]
public class PlayerController : NetworkBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerShooting _playerShooting;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerShooting = GetComponent<PlayerShooting>();
    }

    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData inputData))
        {
            _animator.SetFloat("LastX", inputData.moveInputX);
            _animator.SetFloat("LastZ", inputData.moveInputZ);

            Vector3 movi = new Vector3(inputData.moveInputX, 0, inputData.moveInputZ);
            _playerMovement.Move(movi.normalized);
            Debug.LogWarning($"Movimiento: {inputData.moveInputX}/{inputData.moveInputZ} - " +
                             $"Fire: {inputData.isFirePressed} - " +
                             $"FireClick {inputData.networkButtomFireClick.IsSet(MyButtoms.fireClick)}");

            if (inputData.isFirePressed)
            {
                _playerShooting.Shoot();
            }
        
        }
    }
}
