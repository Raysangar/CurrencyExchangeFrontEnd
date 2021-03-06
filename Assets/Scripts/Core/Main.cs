﻿using System.Collections;
using UnityEngine;

namespace CurrencyExchanger
{
    public class Main : MonoBehaviour
    {
        void Awake()
        {
            currencyExchanger = new CurrencyExchanger(new PlayfabBackEndProvider(keys.PlayfabKey, keys.CurrenchExchangeApiKey), OnSystemReadyCallback, OnSystemInitializationError);
        }

        private void OnSystemReadyCallback()
        {
            uiManager.SetMainPanel(currencyExchanger);
        }

        private void OnSystemInitializationError(string errorMessage)
        {
            uiManager.SetSystemError(errorMessage);
        }

        [SerializeField] UIManager uiManager;
        [SerializeField] KeysScriptableObject keys;

        private CurrencyExchanger currencyExchanger;
    }
}
