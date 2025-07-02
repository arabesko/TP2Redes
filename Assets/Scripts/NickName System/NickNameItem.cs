using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class NickNameItem : MonoBehaviour
{
    [SerializeField] private float _yOffset;
    private Transform _owner;
    private TMP_Text _nickNameText;
    private Vector3 _offset;
    public void SetOwner(Transform target)
    {
        _owner = target;
        _nickNameText = GetComponent<TMP_Text>();
        _offset = Vector3.forward * _yOffset;
    }

    public void UpdateText(string newNick)
    {
        _nickNameText.text = newNick;
    }

    public void UpdatePosition()
    {
        transform.position = _owner.position + _offset;
    }
}
