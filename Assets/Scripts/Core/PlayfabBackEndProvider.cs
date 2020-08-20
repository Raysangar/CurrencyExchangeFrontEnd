using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.Json;
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
                PlayFabId = "FF68592653210042",
                FunctionParameter = new Dictionary<string, string>()
                {
                    { "key", currencyExchangerApiKey },
                    { "from", currencyFrom },
                    { "to", currencyTo },
                }
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                var jsonObject = result.FunctionResult as JsonObject;
                string exchangeRateKey = currencyFrom + "_" + currencyTo;
                if (jsonObject != null && jsonObject.ContainsKey(exchangeRateKey))
                {
                    excahngeRateRetrievedCallback(float.Parse(jsonObject[exchangeRateKey].ToString()));
                }
                else
                {
                    errorCallback("Couldn't retrieve exchange rate from API: " + result.FunctionResult.ToString());
                }

            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }

        public void GetExchangeRateFor(string currencyFrom, string currencyTo, DateTime date, Action<float> excahngeRateRetrievedCallback, Action<string> errorCallback)
        {
            var request = new ExecuteCloudScriptServerRequest()
            {
                FunctionName = "GetExchangeRate",
                PlayFabId = "FF68592653210042",
                FunctionParameter = new Dictionary<string, string>()
                {
                    { "key", currencyExchangerApiKey },
                    { "from", currencyFrom },
                    { "to", currencyTo },
                    { "date", date.ToString("yyyy-MM-dd") }
                }
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                var jsonObject = result.FunctionResult as JsonObject;
                string exchangeRateKey = currencyFrom + "_" + currencyTo;
                if (jsonObject != null && jsonObject.ContainsKey(exchangeRateKey))
                {
                    jsonObject = jsonObject[exchangeRateKey] as JsonObject;
                    foreach(var pair in jsonObject)
                        excahngeRateRetrievedCallback(float.Parse(pair.Value.ToString()));
                }
                else
                {
                    errorCallback("Couldn't retrieve exchange rate from API: " + result.FunctionResult.ToString());
                }

            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }

        public void RetrieveCurrencies(Action<List<Currency>> currenciesRetrievedCallback, Action<string> errorCallback)
        {
            var request = new ExecuteCloudScriptServerRequest()
            {
                FunctionName = "GetCurrencyList",
                PlayFabId = playfabKey,
                FunctionParameter = new Dictionary<string, string>()
                {
                    { "key", currencyExchangerApiKey },
                }
            };

            PlayFabServerAPI.ExecuteCloudScript(request, (result) =>
            {
                var list = new List<Currency>();
                var jsonObject = result.FunctionResult as JsonObject;
                if (jsonObject != null && jsonObject.ContainsKey("results"))
                {
                    jsonObject = jsonObject["results"] as JsonObject;
                    foreach (var pair in jsonObject)
                    {
                        list.Add(new Currency
                        {
                            Code = pair.Key,
                            Name = (pair.Value as JsonObject)["currencyName"] as string
                        });
                    }
                    currenciesRetrievedCallback(list);
                }
                else
                {
                    errorCallback("Couldn't retrieve currency list from API: " + result.FunctionResult.ToString());
                }
            }, (error) => {
                errorCallback(error.ErrorMessage);
            });
        }

        public PlayfabBackEndProvider(string playfabKey, string currencyExchangerApiKey)
        {
            this.playfabKey = playfabKey;
            this.currencyExchangerApiKey = currencyExchangerApiKey;
        }

        private string playfabKey;
        private string currencyExchangerApiKey;
    }
}
