using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Balancer : MonoBehaviour
{
    [Header("Стартовые данные")]
    public string userName = "GOD";
    public string shopName = "GOD`s SHOP";
    [HideInInspector] public string game_uuid = "c18376f6-6cf3-48ac-928c-69ec0e74f46b";
    public float startGold = 1000;
    public float startGpm = 10;
    [Header("Мини-игра сапер")]
    public float goldForGame = 10;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public int mineCount = 8;
    public int lives = 3;
    [Header("Дом")]
    public int house_coast; 
    public int house_lvlUpCoast; 
    public int house_maxLVL;
    public float house_gpm;
    public float house_addingGpm;
    [Header("Лаба")]
    public int laboratory_coast;
    public int laboratory_lvlUpCoast;
    public int laboratory_maxLVL;
    public int laboratory_coastForOneExperement;
    public int laboratory_minPercent;
    public int laboratory_maxPercent;
    public int laboratory_addingPercent;

}
