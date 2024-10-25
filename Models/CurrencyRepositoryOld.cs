namespace VostokExchRateWebAPI.Models;

internal class CurrencyRepositoryOld
{
    /*// Moved
    public static void Create(CurrencyModel currency)
    {
        using (DatabaseContext context = new())
        {
            context.Currencies.Add(currency);
            context.SaveChanges();
        }
    }

    // Moved
    public static List<CurrencyModel> Read()
    {
        using (DatabaseContext context = new())
            return context.Currencies.ToList();
    }

    // Moved
    public static CurrencyModel ReadFirst()
    {
        using (DatabaseContext context = new())
            return context.Currencies.First();
    }

    static void update(BankCurrency currency)
    {
        using (CurrencyContext context = new())
        {
            currency.property = value
                context.SaveChanges();
        }

    }

    // Moved
    public static void Delete(CurrencyModel currency)
    {
        using (DatabaseContext context = new())
        {
            context.Currencies.Remove(currency);
            context.SaveChanges();
        }

    }

    // Moved
    public static void SaveToDB(List<CurrencyModel> curs)
    {
        if (curs == null)
            return;

        foreach (var cur in curs)
        {
            CurrencyModel? existing;
            using (DatabaseContext context = new())
                existing = context.Currencies.SingleOrDefault(c => c.Code == cur.Code);

            if (existing != null)
            {
                cur.Name = existing.Name ?? cur.Name;
                cur.ShortName = existing.ShortName ?? cur.ShortName;
                Delete(existing);
            }

            Create(cur);
        }
    }*/
}
