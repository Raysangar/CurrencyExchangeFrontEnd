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


        public CurrencyExchanger(IBackEndProvider backEndProvider, System.Action systemReadyCallback, System.Action<string> initializationErrorCallback)
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
        private System.Action systemReadyCallback;
    }
}