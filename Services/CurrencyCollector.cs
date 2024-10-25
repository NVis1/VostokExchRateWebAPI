using Newtonsoft.Json.Linq;
using VostokExchRateWebAPI.Models;


namespace VostokExchRateWebAPI.Services;


class ExternalApiException : Exception
{
    public ExternalApiException() { }

    public ExternalApiException(string message) : base(message) { }

    public ExternalApiException(string message, Exception innerException) : base(message, innerException) { }
}


internal abstract class CurrencyCollector
{
    internal static readonly HttpClient httpc = new();

    internal static void ReturnInvalidFieldWarning()
    {
        throw new ExternalApiException("Could not find \"currencies\" property in retrieved json response object. " +
            "Probably the API's json format has changed.");
    }

    internal static void ReturnInvalidCurrencyFieldWarning(Exception ex)
    {
        throw new ExternalApiException("Could not find currency property or some properties in retrieved currency object. " +
            "The entry was not saved in the database. Probably the API's json format has changed. Details: " + ex.Message);
    }

    internal static void ReturnTimeoutWarning(Exception ex)
    {
        // throw new NotImplementedException();
        Console.WriteLine("The request to external bank API timed out. Probably the external API has changed. Details: " + ex.Message);
    }

    internal abstract Task<JArray?> CurrencyParser();
    internal abstract CurrencyModel? CurrencyConverter(JToken iCurrency);

    internal IEnumerable<CurrencyModel>? Get()
    {
        JArray? currencies;
        try
        {
            currencies = CurrencyParser().Result;
        }
        catch (TaskCanceledException ex)
        {
            ReturnTimeoutWarning(ex);
            return null;
        }

        if (currencies is null)
        {
            ReturnInvalidFieldWarning();
            return null;
        }

        List<CurrencyModel> result = [];

        foreach (JToken iCurrency in currencies)
        {
            try
            {
                result.Add(CurrencyConverter(iCurrency));
            }
            catch (NullReferenceException ex)
            {
                ReturnInvalidCurrencyFieldWarning(ex);
                continue;
            }
        }

        return result;
    }
}


internal class VostokCurrencyCollector : CurrencyCollector
{
    internal override async Task<JArray?> CurrencyParser()
    {
        string response = await httpc.GetStringAsync(
            "https://bvr.api.vostok.bank/public/api/v1/currency-rates"
            );

        JObject jsonResponse = JObject.Parse(response);

        return jsonResponse["currencies"] is JArray currencies ? currencies : null;
    }

    internal override CurrencyModel? CurrencyConverter(JToken iCurrency)
    {
        CurrencyModel? currency = iCurrency.ToObject<CurrencyModel>();
        if (currency is not null)
        {

            JToken rates = iCurrency["currencyRates"].FirstOrDefault();

            currency.Date = rates["date"].ToObject<DateTime>();
            currency.Buy = rates["buy"].ToObject<float>();
            currency.Sell = rates["sell"].ToObject<float>();
        }
        return currency;
    }
}


internal class MonoCurrencyCollector : CurrencyCollector
{
    internal override async Task<JArray?> CurrencyParser()
    {
        string response = await httpc.GetStringAsync(
            "https://api.monobank.ua/bank/currency"
            );

        JToken jsonResponse = JToken.Parse(response);

        return jsonResponse["currencies"] is JArray currencies ? currencies : null;
    }

    internal override CurrencyModel? CurrencyConverter(JToken iCurrency)
    {
        CurrencyModel currency = new()
        {
            Code = iCurrency["currencyCodeA"].ToObject<short>(),
            Date = DateTimeOffset.FromUnixTimeSeconds(
                 (long)iCurrency["date"]
                ).ToLocalTime().DateTime
        };

        if (iCurrency["rateCross"] is not null)
        {
            currency.Buy = iCurrency["rateCross"].ToObject<float>();
            currency.Sell = iCurrency["rateCross"].ToObject<float>();
        }
        else
        {
            currency.Buy = iCurrency["rateBuy"].ToObject<float>();
            currency.Sell = iCurrency["rateSell"].ToObject<float>();
        }

        return currency;
    }
}
