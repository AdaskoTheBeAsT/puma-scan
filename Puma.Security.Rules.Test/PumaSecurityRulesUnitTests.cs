using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puma.Security.Rules.Suites;
using TestHelper;

namespace Puma.Security.Rules.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {
        [TestMethod]
        public void EmptySourceDoesNotProduceDiagnostics()
        {
            VerifyCSharpDiagnostic(string.Empty);
        }

        [TestMethod]
        public void SystemRandomProducesDiagnostic()
        {
            var test = @"
using System;

class Example
{
    void Generate()
    {
        var random = new Random();
    }
}";
            var expected = new DiagnosticResult
            {
                Id = "SEC0115",
                Message = "System.Random does not provide cryptographically random numbers. Consider using the System.Security.Cryptography.RNGCryptoServiceProvider for random values used in a security context.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[]
                    {
                        new DiagnosticResultLocation("Test0.cs", 8, 22)
                    }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PumaDiagnosticSuite();
        }
    }
}
