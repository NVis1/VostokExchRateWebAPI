using VostokExchRateWebAPI.Models;
using VostokExchRateWebAPI.Services;


class ProgramOld
{
    private delegate IEnumerable<CurrencyModel>? UsingBank();  // простейший пример использования делегата просто чтоб было
    
    static void Main()
    {
        UsingBank GetCurrenciesFromBank = new(UseVostok);
    }

    private static IEnumerable<CurrencyModel>? UseVostok()
    {
        CurrencyCollector collector = new VostokCurrencyCollector();
        return collector.Get();
    }

    private static IEnumerable<CurrencyModel>? UseMono()
    {
        CurrencyCollector collector = new MonoCurrencyCollector();
        return collector.Get();
    }

    private static void PrintCurrencies(IEnumerable<CurrencyModel> curList)
    {
        foreach (var item in curList)
        {
            Console.WriteLine(curList.ToString());
            Console.WriteLine();
            Console.WriteLine(
                $"Code: {item.Code}\n" +
                $"Short name: {item.ShortName}\n" +
                $"Name: {item.Name}\n" +
                $"Date: {item.Date}\n" +
                $"  Buy:  {item.Buy}\n" +
                $"  Sell: {item.Sell}\n"
                );
        }
    }
}
