using System;

namespace Lombiq.TrainingDemo.ViewModels
{
    public class CacheViewModel
    {
        public DateTime MemoryCachedDateTime { get; set; }
        public DateTime DynamicCachedDateTimeWith30SecondsExpiry { get; set; }
        public DateTime DynamicCachedDateTimeVariedByRoutes { get; set; }
    }
}
