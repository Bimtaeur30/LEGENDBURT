using System.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    public void Initialize() { }
    private void Start()
    {
        StartCoroutine(SetPlayerPos());
    }
    private IEnumerator SetPlayerPos()
    {
        yield return null;

        Player player = FindFirstObjectByType<Player>();

        Debug.Log($"Before : {player.transform.position}");

        player.Rigid.position = playerSpawnPoint.position;
        player.Rigid.rotation = playerSpawnPoint.rotation;
        player.Rigid.linearVelocity = Vector3.zero;
        player.Rigid.angularVelocity = Vector3.zero;

        Debug.Log($"After : {player.transform.position}");
        Debug.Log($"Spawn : {playerSpawnPoint.position}");
    }
}
