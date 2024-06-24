
using UnityEngine;


public class GameSetup : MonoBehaviour
{
    [Tooltip("Determines what is Timer value at the start of a new Sesion")]
    public float sessionStartingTime;

    private void Awake()
    {
        GetComponent<GameTimer>().SetInitialTimer(sessionStartingTime);      
    }
}
