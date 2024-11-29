using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancer : MonoBehaviour
{
    [Header("Дом")]
    public int house_coast; 
    public int house_lvlUpCoast; 
    public int house_maxLVL;
    public float house_gpm;
    public float house_addingGpm;
    [Space]
    [Header("Лаба")]
    public int laboratory_coast;
    public int laboratory_lvlUpCoast;
    public int laboratory_maxLVL;
    public int laboratory_coastForOneExperement;
    public int laboratory_minPercent;
    public int laboratory_maxPercent;
    public int laboratory_addingPercent;

}