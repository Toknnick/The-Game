using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "НовоеЗдание", menuName = "Buildings", order = 51)]
public class Building : ScriptableObject
{
    public List<Sprite> sprites; // Хранит спрайт
    public UsingScript usingScript; // Хранит спрайт
    public int gpm; // Можно добавить описание или другие данные
    public int coast; // Можно добавить описание или другие данные
    public int maxLVL; // Можно добавить описание или другие данные
    public int lvlUpCoast; // Можно добавить описание или другие данные
    public int addingGpm; // Можно добавить описание или другие данные
}
