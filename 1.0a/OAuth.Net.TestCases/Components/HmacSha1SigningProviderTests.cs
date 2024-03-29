﻿// Copyright (c) 2008 Madgex
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// OAuth.net uses the Common Service Locator interface, released under the MS-PL
// license. See "CommonServiceLocator License.txt" in the Licenses folder.
// 
// The examples and test cases use the Windsor Container from the Castle Project
// and Common Service Locator Windsor adaptor, released under the Apache License,
// Version 2.0. See "Castle Project License.txt" in the Licenses folder.
// 
// XRDS-Simple.net uses the HTMLAgility Pack. See "HTML Agility Pack License.txt"
// in the Licenses folder.
//
// Authors: Bruce Boughton, Chris Adams
// Website: http://lab.madgex.com/oauth-net/
// Email:   oauth-dot-net@madgex.com

#if DEBUG
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using OAuth.Net.Components;
using OAuth.Net.Common;
using System;

namespace OAuth.Net.TestCases.Components
{
    [TestFixture]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "HMAC and SHA are domain acronyms")]
    public class HmacSha1SigningProviderTests
    {
        [Test]
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Unit test")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Unit test methods cannot be static")]
        public void Test_AuthCore1_0_AppendixA_5_2_Example()
        {
            string sigbase = "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal";
            string consumerSecret = "kd94hf93k423kf44";
            string tokenSecret = "pfkkdhi9sl3r4s00";

            HmacSha1SigningProvider signingProvider = new HmacSha1SigningProvider();
            Assert.That(signingProvider.SignatureMethod, Is.EqualTo("HMAC-SHA1"));

            string hash = signingProvider.ComputeSignature(sigbase, consumerSecret, tokenSecret);
            Assert.That(hash, Is.EqualTo("tR3+Ty81lMeYAr/Fid0kMTYa/WM="));
        }

        [Test]
        public void Test_SignatureCompareWithSpaceInSignature()
        {
            OAuthParameters parameters = new OAuthParameters()
            {
                ConsumerKey = "key",
                Nonce = "5b434e59-729a-444b-9a11-2d8e57b1f2fb",
                SignatureMethod = "HMAC-SHA1",
                Timestamp = "1251983826",                
                Version = "1.0",
                Callback = "http://yourownsite.com/"
            };

            string sigbase = SignatureBase.Create(
                "GET",
                new Uri("http://localhost:3423/request-token.ashx"),
                parameters);

            string consumerSecret = "secret";
            string tokenSecret = null;

            HmacSha1SigningProvider signingProvider = new HmacSha1SigningProvider();
            Assert.That(signingProvider.SignatureMethod, Is.EqualTo("HMAC-SHA1"));

            string hash = signingProvider.ComputeSignature(sigbase, consumerSecret, tokenSecret);
            Assert.That(hash, Is.EqualTo("zHTiQHg8X5Lpkh+/0MSatKeNEFg="));

            Assert.That(signingProvider.CheckSignature(sigbase, Rfc3986.Decode("zHTiQHg8X5Lpkh+/0MSatKeNEFg="), consumerSecret, tokenSecret), "Signature did not match");

        }
    }
}
#endif