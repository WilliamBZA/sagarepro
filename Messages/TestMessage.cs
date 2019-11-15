using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages
{
    public class TestMessage : ICommand
    {
        public Guid SagaGuid { get { return new Guid("{DC4F1D90-62D7-4D8D-811A-7352F11EC5A3}"); } }
    }
}
