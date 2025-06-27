using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public float moveInputX;
    public float moveInputZ;
    public NetworkBool isFirePressed;

    public NetworkButtons networkButtomFireClick;
}

enum MyButtoms
{
    fireSpace = 0,
    fireClick = 1
}
