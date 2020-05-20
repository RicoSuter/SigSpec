using System;
using System.Collections.Generic;
using SigSpec.Core;

namespace SigSpec.AspNetCore.Middlewares
{
    public class SigSpecSettings : SigSpecGeneratorSettings
    {
        public ICollection<Type> Hubs { get; set; } = new List<Type>();

        public SigSpecDocument Template { get; set; } = new SigSpecDocument();
    }
}
