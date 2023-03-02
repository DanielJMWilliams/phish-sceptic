using Xunit;
using PhishSceptic.Client.Utilities;
using System.Collections.Generic;
using PhishSceptic.Client.Components;

namespace PhishScepticTests
{
    public class LinkAnalyserTests
    {
        [Fact]
        public void CheckDomainTest()
        {
            LinkAnalyser.CheckReputation("www.piratebay.com");

        }

    }
}
