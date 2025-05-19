using System;
using System.Collections.Generic;

namespace UrlShortener.Domain.Models;

public partial class ShortUrlDatum
{
    public string ShortUrl { get; set; } = null!;

    public string? OriginalUrl { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
