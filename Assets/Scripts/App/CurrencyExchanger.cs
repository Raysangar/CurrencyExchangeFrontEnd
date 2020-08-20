using System;
using System.Collections.Generic;

namespace CurrencyExchanger
{
    public class CurrencyExchanger
    {

        public bool IsReady => Currencies != null;

        public List<Currency> Currencies
        {
            get;
            private set;
        }
        public void Convert(string currencyFrom, string currencyTo, int amount, Action<float> exhangeRetrievedCallback, Action<string> errorCallback)
        {
            backEndProvider.GetExchangeRateFor(currencyFrom, currencyTo, (exchangeRate) => exhangeRetrievedCallback(exchangeRate * amount), errorCallback);
        }

        public CurrencyExchanger(IBackEndProvider backEndProvider, Action systemReadyCallback, Action<string> initializationErrorCallback)
        {
            Currencies = null;
            this.backEndProvider = backEndProvider;
            this.systemReadyCallback = systemReadyCallback;
            backEndProvider.RetrieveCurrencies(OnCurrenciesRetrieved, initializationErrorCallback);
        }

        private void OnCurrenciesRetrieved(List<Currency> currencies)
        {
            Currencies = currencies;
            systemReadyCallback();
        }

        private IBackEndProvider backEndProvider;
        private Action systemReadyCallback;

    }
}