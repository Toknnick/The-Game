using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Building : MonoBehaviour
{
    public enum BuildingType { House, Laboratory, CityHall } // Типы зданий
    public BuildingType type;

    public List<Sprite> sprites; // Хранит спрайт
    public UsingScript usingScript; // Хранит спрайт
    [HideInInspector]public float coast; // Можно добавить описание или другие данные
    [HideInInspector] public int maxLVL; // Можно добавить описание или другие данные
    [HideInInspector] public float lvlUpCoast; // Можно добавить описание или другие данные
}
