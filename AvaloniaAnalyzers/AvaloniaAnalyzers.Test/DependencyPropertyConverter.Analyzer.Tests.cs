﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace AvaloniaAnalyzers.Test
{
    [TestClass]
    public class DependencyPropertyConverterAnalyzerTests : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DependencyPropertyConverterAnalyzer();
        }


        //No diagnostics expected to show up
        [TestMethod]
        public void NoDiagnosticOnEmptyInput()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }
        
        [TestMethod]
        public void DiagnosticTriggeredOnDependencyProperty()
        {
            var test = @"
    using System.Windows;

    namespace ConsoleApplication1
    {
        class TestProperty
        {
            public static readonly DependencyProperty Property1 = DependencyProperty.Register(""Property"", typeof(int), typeof(TestProperty));
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = DependencyPropertyConverterAnalyzer.DiagnosticId,
                Message = "Field is a WPF DependencyProperty",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 8, 55)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }
    }
}
