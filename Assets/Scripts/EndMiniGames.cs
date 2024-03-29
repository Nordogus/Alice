﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMiniGames : MonoBehaviour
{

    [SerializeField] private int _miniGameToEnd = 1;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (_miniGameToEnd)
            {
                case 1:
                    if (MiniGameManager.instance.state == State.TUTO)
                    {
                        StartCoroutine(ObjectSpawner.instance.WaitAndEndTuto());
                    }
                    break;
                case 2:
                    if (MiniGameManager.instance.state == State.FIRSTMG)
                    {
                        FirstMiniGame.instance.StopMiniGame();
                    }
                    break;
                case 3:
                    if (MiniGameManager.instance.state == State.SECONDMG)
                    {
                        SecondMiniGame.instance.StopSecondMiniGame();
                        MiniGameManager.instance.ChangeState(State.NONE);
                        if (GameManager.instance.onPhaseChange != null) GameManager.instance.onPhaseChange.Invoke(6);
                    }
                    break;
            }
        }
    }
}
