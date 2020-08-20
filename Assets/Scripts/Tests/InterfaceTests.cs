using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CurrencyExchanger;
using Moq;

namespace Tests
{
    public class InterfaceTests
    {
        [UnityTest]
        public IEnumerator InterfaceReturnsListOfCurrenciesAfterItsReady()
        {
            var backEndProviderMock = new Mock<IBackEndProvider>();
            backEndProviderMock.Setup(c => c.RetrieveCurrencies(It.IsAny<System.Action<List<Currency>>>(), It.IsAny<System.Action<string>>()))
                .Callback((System.Action<List<Currency>> onCurrenciesRetrieved, System.Action<string> errorCallback) => onCurrenciesRetrieved(new List<Currency>()));

            var currencyExchanger = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, () => { }, (list) => { });
            yield return new WaitUntil(() => currencyExchanger.IsReady);
            Assert.NotNull(currencyExchanger.Currencies);
        }
    }
}
