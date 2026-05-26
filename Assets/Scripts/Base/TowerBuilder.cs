using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerBuilder : MonoBehaviour
{
    [Header("Посилання на сітки")]
    public Tilemap grassTilemap;
    public Tilemap roadTilemap;


    [Header("Економіка та Дані")]
    public PlayerEconomy economy;
    public TowerData selectedTower;
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandleGridClick();
        }
    }

private void HandleGridClick()
    {
        if (selectedTower == null)
        {
            return;
        }

        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -Camera.main.transform.position.z; 
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(screenPos);
        mouseWorldPos.z = 0f; 

        Vector3Int cellPosition = grassTilemap.WorldToCell(mouseWorldPos);

        ValidateAndBuild(cellPosition, mouseWorldPos);
    }

    private void ValidateAndBuild(Vector3Int cellPos, Vector3 worldPos)
    {
        if (occupiedTiles.Contains(cellPos) || roadTilemap.HasTile(cellPos) || !grassTilemap.HasTile(cellPos) || !economy.CanAfford(selectedTower.price))
        {
            return;
        }

        economy.SpendGold(selectedTower.price);
        Vector3 centerPos = grassTilemap.GetCellCenterWorld(cellPos);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buildSound);
        Instantiate(selectedTower.towerPrefab, centerPos, Quaternion.identity);
        occupiedTiles.Add(cellPos);
        
    }
    
    public void SelectTower(TowerData towerToSelect)
    {
        selectedTower = towerToSelect;
    }
}