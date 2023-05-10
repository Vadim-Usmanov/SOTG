using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject _shadowPrefab;
    [SerializeField] private Exit _exit;
    [SerializeField] private Spawn _spawn;
    [SerializeField] private List<PracticeArea> _practiceAreas;
    public Exit Exit { get => _exit; }
    public IList<PracticeArea> PracticeAreas { get => _practiceAreas.AsReadOnly(); }
    public GameObject CreateShadow(Sprite sprite)
    {
        GameObject shadow = Instantiate(_shadowPrefab, transform);
        shadow.GetComponent<SpriteRenderer>().sprite = sprite;
        return shadow;
    }
    public Disk SpawnDisk()
    {
        Disk disk = Instantiate<Disk>(_gameController.DiskPrefab, transform);
        disk.transform.position = _spawn.RandomPosition();
        return disk;
    }
    public Player SpawnPlayer()
    {
        Player player = Instantiate<Player>(_gameController.PlayerPrefab, transform);
        player.transform.position = _spawn.RandomPosition();
        if (Random.value < 0.08f) player.DiskHandling.IsLeftHanded = true;
        return player;
    }
}
