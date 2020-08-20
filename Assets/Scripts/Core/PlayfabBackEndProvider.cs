using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ServerModels;
using UnityEngine;

namespace CurrencyExchanger
{
    public class PlayfabBackEndProvider : IBackEndProvider
    {
        public void GetExchangeRateFor(string currencyFrom, string currencyTo, Action<float> excahngeRateRetrievedCallback, Action<string> errorCallback)
        {
            var request = new ExecuteCloudScriptServerRequest()
            {
                FunctionName = "GetExchangeRate",
                FunctionParameter = new Dictionary<string, string>()
                {
                    { "from", currencyFrom },
                    { "to", currencyTo },
                }
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                excahngeRateRetrievedCallback((float) result.FunctionResult);
            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }

        public void GetExchangeRateFor(string currencyFrom, string currencyTo, DateTime date, Action<float> excahngeRateRetrievedCallback, Action<string> errorCallback)
        {
            var request = new ExecuteCloudScriptServerRequest()
            {
                FunctionName = "GetExchangeRate",
                FunctionParameter = new Dictionary<string, string>()
                {
                    { "from", currencyFrom },
                    { "to", currencyTo },
                    { "date", date.ToShortDateString() }
                }
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                excahngeRateRetrievedCallback((float)result.FunctionResult);
            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }

        public void RetrieveCurrencies(Action<List<Currency>> currenciesRetrievedCallback, Action<string> errorCallback)
        {
            var request = new ExecuteCloudScriptServerRequest()
            {
                FunctionName = "GetCurrencyList"
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                var list = JsonUtility.FromJson<List<Currency>>(result.FunctionResult.ToString());
                currenciesRetrievedCallback(list);
            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }
    }
}
