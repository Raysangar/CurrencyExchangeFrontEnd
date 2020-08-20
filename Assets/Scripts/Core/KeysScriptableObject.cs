using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurrencyExchanger
{
    [CreateAssetMenu(fileName ="Keys", menuName = "Currency Exchanger/Keys")]
    public class KeysScriptableObject : ScriptableObject
    {
        public string PlayfabKey => playfabKey;
        public string CurrenchExchangeApiKey => currencyExchangeApiKey;

        [SerializeField] string playfabKey;
        [SerializeField] string currencyExchangeApiKey;
    }
}
