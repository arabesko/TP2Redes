using Fusion;
using System.Collections;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private float _boundsWith, _boundsHeight;
    public GameObject deathUI;

    [Networked] private NetworkBool IsPowerUpActive { get; set; }
    [SerializeField] private GameObject _doubleShootPrefab;
    [SerializeField] private float _timeToPowerAgain = 3;
    [SerializeField] private float _timeToDesapear = 3;

    [Networked] private TickTimer TickTimerAgain { get; set; }
    [Networked] private TickTimer TickTimerDesapear { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_boundsWith, 0, _boundsHeight));
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority) // Solo el host inicia el ciclo
        {
            PowerAgain();
        }
    }

    public override void Render()
    {
        _doubleShootPrefab.SetActive(IsPowerUpActive);
    }

    private void PowerAgain()
    {
        IsPowerUpActive = true;
        _doubleShootPrefab.transform.position = new Vector3(Random.Range(-_boundsWith/2, _boundsWith/2), 0, Random.Range(-_boundsHeight/2, _boundsHeight/2));
        TickTimerDesapear = TickTimer.CreateFromSeconds(Runner, _timeToDesapear);
        TickTimerAgain = TickTimer.None;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_DesapearPower()
    {
        IsPowerUpActive = false;
        TickTimerAgain = TickTimer.CreateFromSeconds(Runner, _timeToPowerAgain);
        TickTimerDesapear = TickTimer.None;
    }

    public void PowerCollected()
    {
       RPC_DesapearPower();
    }

    public override void FixedUpdateNetwork()
    {
        if (TickTimerDesapear.Expired(Runner))
        {
            RPC_DesapearPower();
        }

        if (TickTimerAgain.Expired(Runner))
        {
            PowerAgain();
        }
    }

    public Vector3 AjustPositionToBounds(Vector3 position)
    {
        float with = _boundsWith / 2;
        float height = _boundsHeight / 2;

        if (position.x > with) position.x = -with;
        if (position.x < -with) position.x = with;

        if (position.z > height) position.z = -height;
        if (position.z < -height) position.z = height;

        position.y = 0;

        return position;
    }

    public void ShowDeathUI()
    {
        deathUI.SetActive(true);
    }
}
