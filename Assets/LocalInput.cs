using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    NetworkInputData _inputData;
    private bool _isFiredPressedSpace;
    private bool _isFiredPressedClick;

    private void Start()
    {
        _inputData = new NetworkInputData();

    }

    private void Update()
    {
        _inputData.moveInputX = Input.GetAxis("Horizontal");
        _inputData.moveInputZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFiredPressedSpace = true;
        }

        //Esto es equivalente al if anterior
        //_isFiredPressedSpace |= Input.GetKeyDown(KeyCode.Space);
        _isFiredPressedClick |= Input.GetKeyDown(KeyCode.Mouse0);
    }

    public NetworkInputData GetLocalInputs()
    {
        _inputData.isFirePressed = _isFiredPressedSpace;
        _isFiredPressedSpace = false;

        _inputData.networkButtomFireClick.Set(MyButtoms.fireClick, _isFiredPressedClick);
        _isFiredPressedClick = false;

       return _inputData;
    }
}
