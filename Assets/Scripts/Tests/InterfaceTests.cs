using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CurrencyExchanger;
using Moq;
using System;
using System.Runtime.InteropServices;

namespace Tests
{
    public class InterfaceTests
    {
        [Test]
        public void InterfaceReturnsListOfCurrenciesAfterItsReady()
        {
            bool systemReadyCallbackCalled = false;
            var backEndProviderMock = GetSuccesfullBackEndProvider();

            var currencyExchanger = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, 
                () => { systemReadyCallbackCalled = true; }, 
                (list) => { });

            Assert.NotNull(currencyExchanger.Currencies);
            Assert.IsTrue(systemReadyCallbackCalled);
        }

        [Test]
        public void InterfaceReturnsCurrencyExchange()
        {
            var backEndProviderMock = GetSuccesfullBackEndProvider();
            backEndProviderMock.Setup(c => c.GetExchangeRateFor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<float>>(), It.IsAny<Action<string>>()))
                .Callback((string currencyFrom, string currencyTo, Action<float> currencyExchangeRetrievedCallback, Action<string> errorCallback) => currencyExchangeRetrievedCallback(1));

            var currencyExchange = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, () => { }, (error) => { });
            float result = 0;
            currencyExchange.Convert("", "", 10, (value) => result = value, (error) => { });

            Assert.AreEqual(10, result, 0.001);
        }

        [Test]
        public void InterfaceReturnCurrencyExchangeFromSpecificDate()
        {
            var backEndProviderMock = GetSuccesfullBackEndProvider();
            backEndProviderMock.Setup(c => c.GetExchangeRateFor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Action<float>>(), It.IsAny<Action<string>>()))
                .Callback((string currencyFrom, string currencyTo, DateTime date, Action<float> currencyExchangeRetrievedCallback, Action<string> errorCallback) => currencyExchangeRetrievedCallback(1));

            var currencyExchange = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, () => { }, (error) => { });
            float result = 0;
            currencyExchange.Convert("", "", 10, new DateTime(), (value) => result = value, (error) => { });

            Assert.AreEqual(10, result, 0.001);
        }

        [Test]
        public void InterfaceReturnErrorIfInitializationFails()
        {
            bool errorCallbackCalled = false;
            var backEndProviderMock = new Mock<IBackEndProvider>();
            backEndProviderMock.Setup(c => c.RetrieveCurrencies(It.IsAny<Action<List<Currency>>>(), It.IsAny<Action<string>>()))
                .Callback((Action<List<Currency>> onCurrenciesRetrieved, Action<string> errorCallback) => errorCallback(""));

            var currencyExchanger = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object,
                () => { },
                (error) => { errorCallbackCalled = true; });

            Assert.IsFalse(currencyExchanger.IsReady);
            Assert.IsTrue(errorCallbackCalled);
        }

        [Test]
        public void InterfaceReturnsErrorIfConversionFails()
        {
            bool errorCallbackCalled = false;
            var backEndProviderMock = GetSuccesfullBackEndProvider();
            backEndProviderMock.Setup(c => c.GetExchangeRateFor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<float>>(), It.IsAny<Action<string>>()))
                .Callback((string currencyFrom, string currencyTo, Action<float> currencyExchangeRetrievedCallback, Action<string> errorCallback) => errorCallback(""));

            var currencyExchange = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, () => { }, (error) => { });
            currencyExchange.Convert("", "", 10, (value) => { }, (error) => { errorCallbackCalled = true; });

            Assert.IsTrue(errorCallbackCalled);
        }

        [Test]
        public void InterfaceReturnsErrorIfConversionWithDateFails()
        {
            bool errorCallbackCalled = false;
            var backEndProviderMock = GetSuccesfullBackEndProvider();
            backEndProviderMock.Setup(c => c.GetExchangeRateFor(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<Action<float>>(), It.IsAny<Action<string>>()))
                .Callback((string currencyFrom, string currencyTo, DateTime date, Action<float> currencyExchangeRetrievedCallback, Action<string> errorCallback) => errorCallback(""));

            var currencyExchange = new CurrencyExchanger.CurrencyExchanger(backEndProviderMock.Object, () => { }, (error) => {  });
            currencyExchange.Convert("", "", 10, new DateTime(), (value) => { }, (error) => { errorCallbackCalled = true; });

            Assert.IsTrue(errorCallbackCalled);
        }

        private Mock<IBackEndProvider> GetSuccesfullBackEndProvider()
        {
            var backEndProviderMock = new Mock<IBackEndProvider>();
            backEndProviderMock.Setup(c => c.RetrieveCurrencies(It.IsAny<Action<List<Currency>>>(), It.IsAny<Action<string>>()))
                .Callback((Action<List<Currency>> onCurrenciesRetrieved, Action<string> errorCallback) => onCurrenciesRetrieved(new List<Currency>()));
            return backEndProviderMock;
        }
    }
}
