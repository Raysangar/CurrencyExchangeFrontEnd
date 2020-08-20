using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace CurrencyExchanger
{
    public class UIManager : MonoBehaviour
    {
        public void SetMainPanel(CurrencyExchanger currencyExchanger)
        {
            this.currencyExchanger = currencyExchanger;

            var names = new List<string>();
            foreach (var currency in currencyExchanger.Currencies)
                names.Add(currency.Name);
            currencyFrom.ClearOptions();
            currencyFrom.AddOptions(names);
            currencyFrom.value = 0;
            currencyTo.ClearOptions();
            currencyTo.AddOptions(names);
            currencyTo.value = 1;

            loadingText.gameObject.SetActive(false);

            mainPanel.alpha = 1;
            mainPanel.interactable = true;
            mainPanel.blocksRaycasts = true;
        }

        public void SetSystemError(string errorMessage)
        {
            loadingText.text = errorMessage;
        }

        private void Awake()
        {
            InitDateDropdowns();
            mainPanel.alpha = .4f;
            mainPanel.interactable = false;
            mainPanel.blocksRaycasts = false;

            convertButton.onClick.AddListener(OnConvertButtonClicked);
            specifyDateToggle.onValueChanged.AddListener(OnToggleValueChanged);

            specifyDateToggle.isOn = false;
        }

        private void OnConvertButtonClicked()
        {
            float parsedAmount = string.IsNullOrEmpty(amount.text) ? 1 : float.Parse(amount.text);
            if (specifyDateToggle.isOn)
            {
                try
                {
                    var date = new DateTime(StartingYear + year.value, month.value + 1, day.value + 1);
                    currencyExchanger.Convert(currencyExchanger.Currencies[currencyFrom.value].Code, currencyExchanger.Currencies[currencyTo.value].Code, parsedAmount, date, OnResultRetrieved, OnErrorOccurred);
                    convertButton.interactable = false;
                }
                catch (Exception)
                {
                    result.text = "Date is wrong";
                }
            }
            else
            {
                currencyExchanger.Convert(currencyExchanger.Currencies[currencyFrom.value].Code, currencyExchanger.Currencies[currencyTo.value].Code, parsedAmount, OnResultRetrieved, OnErrorOccurred);
                convertButton.interactable = false;
            }
        }

        private void OnResultRetrieved(float result)
        {
            this.result.text = result.ToString("F2");
            convertButton.interactable = true;
        }

        private void OnErrorOccurred(string error)
        {
            result.text = error;
            convertButton.interactable = true;
        }

        private void OnToggleValueChanged(bool isOn)
        {
            dateCanvasGroup.alpha = isOn ? 1 :.4f;
            dateCanvasGroup.interactable = isOn;
            dateCanvasGroup.blocksRaycasts = isOn;
        }

        private void InitDateDropdowns()
        {
            var list = new List<string>();
            for (int i = 1; i < 32; i++)
                list.Add(i.ToString());
            day.ClearOptions();
            day.AddOptions(list);
            day.value = 0;

            list.Clear();
            for (int i = 1; i < 13; i++)
                list.Add(i.ToString());
            month.ClearOptions();
            month.AddOptions(list);
            month.value = 0;

            list.Clear();
            for (int i = StartingYear; i <= DateTime.Now.Year; i++)
                list.Add(i.ToString());
            year.ClearOptions();
            year.AddOptions(list);
            year.value = 0;
        }

        [SerializeField] TextMeshProUGUI loadingText;

        [SerializeField] CanvasGroup mainPanel;
        [SerializeField] TMP_Dropdown currencyFrom;
        [SerializeField] TMP_Dropdown currencyTo;
        [SerializeField] TMP_InputField amount;

        [SerializeField] Toggle specifyDateToggle;
        [SerializeField] CanvasGroup dateCanvasGroup;
        [SerializeField] TMP_Dropdown day;
        [SerializeField] TMP_Dropdown month;
        [SerializeField] TMP_Dropdown year;

        [SerializeField] Button convertButton;
        [SerializeField] TMP_InputField result;

        private const int StartingYear = 2015;

        private CurrencyExchanger currencyExchanger;
    }
}
