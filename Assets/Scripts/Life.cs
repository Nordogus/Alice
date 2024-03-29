﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    #region Script Parameters

    public static Life instance;

    #endregion

    #region Fields

    private int indexLife = 0;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public void SetLifeUI(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void LooseLife()
    {
        GameObject life = transform.GetChild(indexLife).gameObject;

        if (life != null)
        {
            life.SetActive(false);
        }

        indexLife++;
    }

    public bool CheckSecretEnd()
    {
        if (indexLife == 0)
        {
            return true;
        }

        return false;
    }
}
