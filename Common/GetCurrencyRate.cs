using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class GetCurrencyRate
    {        
        public static decimal GetByCurrency(string currency)
        {
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);

           string currencyValue = xmlDoc.SelectSingleNode($"Tarih_Date/Currency[@Kod ='{currency.ToUpper()}']/BanknoteSelling").InnerXml;
            return  Convert.ToDecimal(currencyValue.Replace(".", ",").Substring(0,currencyValue.ToCharArray().Length-2));
        }  
    }
}
