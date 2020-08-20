using System;
using System.Collections.Generic;

namespace CurrencyExchanger
{
    public interface IBackEndProvider
    {
        void RetrieveCurrencies(Action<List<Currency>> currenciesRetrievedCallback, Action<string> errorCallback);
        void GetExchangeRateFor(string currencyFrom, string currencyTo, Action<float> excahngeRateRetrievedCallback, Action<string> errorCallback);
    }
}
