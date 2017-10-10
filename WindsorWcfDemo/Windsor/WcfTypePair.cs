using System;

namespace Phoenix.Core.Infrastructure.Windsor
{
    public class WcfTypePair
    {
        public Type Implementation { get; set; }

        public Type Interface { get; set; }
        
        public override string ToString()
        {
            return $"Interface={Interface.Name}, Implementation={Implementation.Name}";
        }

        public static WcfTypePair Factory<TInterface, TImplmentation>()
        {
            return new WcfTypePair { Implementation = typeof(TImplmentation), Interface = typeof(TInterface) };
        }
    }
    
}