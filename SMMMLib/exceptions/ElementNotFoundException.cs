using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMMMLib.exceptions
{
    [Serializable]
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException() { }
        public ElementNotFoundException(string message) : base(message) { }
        public ElementNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ElementNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
