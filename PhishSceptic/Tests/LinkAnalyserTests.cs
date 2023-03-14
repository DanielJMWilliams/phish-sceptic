using Xunit;
using PhishSceptic.Client.Utilities;
using System.Collections.Generic;
using PhishSceptic.Client.Components;

namespace PhishScepticTests
{
    public class LinkInspectorTests
    {
        [Fact]
        public void CheckDomainTest()
        {
            LinkInspector.CheckReputation("www.piratebay.com");

        }

    }
}
