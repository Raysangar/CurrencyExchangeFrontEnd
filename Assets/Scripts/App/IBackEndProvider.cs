using System.Collections.Generic;

namespace CurrencyExchanger
{
    public interface IBackEndProvider
    {
        void RetrieveCurrencies(System.Action<List<Currency>> currenciesRetrievedCallback, System.Action<string> errorCallback);
    }
}
