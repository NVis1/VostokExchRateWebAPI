using System.ComponentModel.DataAnnotations;

namespace VostokExchRateWebAPI.Models;

public class CurrencyModel
{
    [Key]
    [MaxLength(10)]
    public short Code { get; set; }

    [MaxLength(5)]
    public string? ShortName { get; set; }
    
    [MaxLength(25)]
    public string? Name { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(10)]
    public float Buy { get; set; }

    [MaxLength(10)]
    public float Sell { get; set; }
}

public class CurrencyDto
{
    public short Code { get; set; }

    public string? ShortName { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public float Buy { get; set; }
    public float Sell { get; set; }
}
