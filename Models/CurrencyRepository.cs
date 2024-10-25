using VostokExchRateWebAPI.Data;

namespace VostokExchRateWebAPI.Models
{
    public interface ICurrencyRepository
    {
        ICollection<CurrencyModel> GetAll();
        CurrencyModel GetFirst();
        void Create(CurrencyModel currency);
        void Delete(CurrencyModel currency);
        void SaveToDB(IEnumerable<CurrencyModel> curs);
    }

    public class CurrencyRepository(DatabaseContext ctx) : ICurrencyRepository
    {
        private readonly DatabaseContext _ctx = ctx;

        public ICollection<CurrencyModel> GetAll()
        {
            return _ctx.Currencies.ToList();
        }

        public CurrencyModel GetFirst()
        {
            return _ctx.Currencies.First();
        }

        public void Create(CurrencyModel currency)
        {
            _ctx.Currencies.Add(currency);
            _ctx.SaveChanges();
        }

        public void Delete(CurrencyModel currency)
        {
            _ctx.Currencies.Remove(currency);
            _ctx.SaveChanges();

        }

        public void SaveToDB(IEnumerable<CurrencyModel> curs)
        {
            foreach (var cur in curs)
            {
                CurrencyModel? existing = _ctx.Currencies.SingleOrDefault(c => c.Code == cur.Code);

                if (existing != null)
                {
                    cur.Name = existing.Name ?? cur.Name;
                    cur.ShortName = existing.ShortName ?? cur.ShortName;
                    Delete(existing);
                }

                Create(cur);
            }
        }
    }
}
