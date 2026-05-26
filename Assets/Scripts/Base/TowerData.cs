using UnityEngine;

public enum AttackType { Single, AoE, Slow }

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower Data")]
public class TowerData : ScriptableObject
{
    [Header("Базові характеристики")]
    public string towerName;
    public int price; 
    
    [Header("Бойові характеристики")]
    public float range;
    public int damage;
    public float fireRate;
    public AttackType attackType;
    
    [Header("Префаби")]
    public GameObject towerPrefab;
    public GameObject projectilePrefab;
}