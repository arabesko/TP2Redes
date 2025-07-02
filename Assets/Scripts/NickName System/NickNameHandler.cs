using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameHandler : MonoBehaviour
{
    [SerializeField] private NickNameItem _nickNameItemPrefab;
    public static NickNameHandler Instance { get; private set; }

    private List<NickNameItem> _nickNameInUse;

    private void Awake()
    {
        Instance = this;
        _nickNameInUse = new();
    }

    public NickNameItem CreateNewNickName(SetPlayerNickName target)
    {
        var newItem = Instantiate(_nickNameItemPrefab, transform);

        newItem.SetOwner(target.transform);

        _nickNameInUse.Add(newItem);

        target.OnLeft += () =>
        {
            _nickNameInUse.Remove(newItem);
            Destroy(newItem.gameObject);
        };

        return newItem;
    }

    private void LateUpdate()
    {
        foreach (var nickNameItem in _nickNameInUse)
        {
            nickNameItem.UpdatePosition();
        }
    }
}
