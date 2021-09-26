using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Guard.Test
{
    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        public void Guard_Should_ReturnValue_IfNoValidationsFail()
        {
            var parameter = "abc";
            var value = Guard.Against(parameter).ShouldNotBeNull().ShouldNotBeEmpty().Value;
            value.Should().Be("abc");
        }
        
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void Guard_Should_ReturnDefaultValue_IfValidationsFail(string dataParam)
        {
            var value = Guard.Against(dataParam).WithDefaultValue("abc").ShouldNotBeNull().ShouldNotBeEmpty().Value;
            value.Should().Be("abc");
        }
        
        [DataTestMethod]
        [DataRow(null)]
        public void Guard_Should_ThrowArgumentNullException_IfValidationsFail(object dataParam)
        {
            Action act = () => _ = Guard.Against(dataParam).ShouldNotBeNull().ThrowArgumentNullException().Value;

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Parameter is null")
                .WithParameterName(string.Empty);

        }
        
        [DataTestMethod]
        [DataRow(null)]
        public void Guard_Should_ThrowArgumentNullException_WithParameterName_IfValidationsFail(object dataParam)
        {
            Action act = () => _ = Guard.Against(dataParam, nameof(dataParam)).ShouldNotBeNull().ThrowArgumentNullException().Value;

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Parameter is null (Parameter 'dataParam')")
                .WithParameterName(nameof(dataParam));

        }
        
        [DataTestMethod]
        [DataRow(null)]
        public void Guard_Should_ThrowArgumentException_WithMultipleErrorMessages_IfMultipleValidationsFail(string dataParam)
        {
            Action act = () => _ = Guard
                .Against(dataParam, nameof(dataParam))
                .ShouldNotBeNull()
                .ShouldNotBeEmpty()
                .ThrowArgumentNullException()
                .Value;

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*|*")
                .WithParameterName(nameof(dataParam));

        }
        
        [DataTestMethod]
        [DataRow(null)]
        public void Guard_Should_ThrowArgumentException_WithMultipleErrorMessages_IfMultipleValidationsFail_WithoutParameterName(string dataParam)
        {
            Action act = () => _ = Guard
                .Against(dataParam)
                .ShouldNotBeNull()
                .ShouldNotBeEmpty()
                .ThrowArgumentNullException()
                .Value;

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*|*")
                .WithParameterName(string.Empty);

        }
        
        [DataTestMethod]
        [DataRow(null)]
        public void Guard_Should_ThrowCustomException_WithErrorMessages_IfValidationsFail(string dataParam)
        {
            Action act = () => _ = Guard
                .Against(dataParam)
                .ShouldNotBeNull()
                .ShouldNotBeEmpty()
                .ThrowCustomException<string, FormatException>()
                .Value;

            act.Should().Throw<FormatException>()
                .WithMessage("*|*");
        }
        
        [DataTestMethod]
        [DataRow("abc")]
        public void Guard_Should_ThrowCustomException_WithCustomErrorMessages_IfValidationsFail(string dataParam)
        {
            Action act = () => _ = Guard
                .Against(dataParam)
                .ValidationFailsIf(s => s == "abc", "s is 'abc'")
                .ThrowArgumentException()
                .Value;

            act.Should().Throw<ArgumentException>()
                .WithMessage("s is 'abc'");
        }
        
        [DataTestMethod]
        [DataRow("abed")]
        public void Guard_Should_ThrowCustomException_WithCustomErrorMessagesAndParamName_IfValidationsFail(string dataParam)
        {
            Action act = () => _ = Guard
                .Against(dataParam, nameof(dataParam))
                .ValidationFailsIf(s => s != "abc", "s is not 'abc'")
                .ThrowArgumentException()
                .Value;

            act.Should().Throw<ArgumentException>()
                .WithMessage("s is not 'abc' (Parameter 'dataParam')")
                .WithParameterName(nameof(dataParam));
        }
    }
}