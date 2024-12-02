using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancer : MonoBehaviour
{
    [Header("Стартовые данные")]
    public string userName = "Developer";
    [HideInInspector] public string game_uuid = "c18376f6-6cf3-48ac-928c-69ec0e74f46b";
    public float startGold = 1000;
    public float startGpm = 10;
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
