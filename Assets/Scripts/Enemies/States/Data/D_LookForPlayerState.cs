using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newLookForPlayerState", menuName = "Data/State Data/Look For Player State")]

public class D_LookForPlayerState : ScriptableObject
{
    public int amountOfTurns = 2;
    public float timeBetWeenTurns = 0.7f;
}
