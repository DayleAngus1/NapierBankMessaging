using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NapierBankMessaging.Import;
using NapierBankMessaging.MessageConvert;
using NapierBankMessaging.Model;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NapierBankMessagingTests.JsonConverter
{
    [TestFixture]
    public class EmailMessageConverterTests
    {
        private Mock<IIncidentListImporter> _importer;
        private EmailMessageConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _importer = new Mock<IIncidentListImporter>();

            _importer.Setup(x => x.ImportIncidentList()).Returns(new List<string>()
            {
                "Theft",
                "Staff Attack",
                "ATM Theft",
                "Raid",
                "Customer Attack",
                "Staff Abuse",
                "Bomb Threat",
                "Terrorism",
                "Suspicious Incident",
                "Intelligence",
                "Cash Loss"
            });

            _converter = new EmailMessageConverter(_importer.Object);
        }

        [Test]
        public void ConvertMessage_WrongMessageTypeThrowsMismatchException()
        {
            var message = new Mock<Message>();

            Assert.Throws(typeof(MessageTypeMismatchException),
                () => _converter.ConvertMessage(It.IsAny<string>(), It.IsAny<string>(), message.Object));
        }

        [Test]
        public void ConvertMessage_EmptyBodyReturnsFalse()
        {
            var message = new Mock<EmailMessage>();

            var body = string.Empty;

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message.Object));
        }

        [TestCase("plainaddress")]
        [TestCase("#@%^%#$@#$@#.com")]
        [TestCase("@example.com")]
        [TestCase("Joe Smith <email@example.com>")]
        [TestCase("email.example.com")]
        public void ConvertMessage_InvalidSenderThrowsException(string invalidEmail)
        {
            var message = new Mock<EmailMessage>();

            Assert.Throws(typeof(InvalidSenderException),
                (() => _converter.ConvertMessage(It.IsAny<string>(), invalidEmail, message.Object)));
        }

        [TestCase("dmouse@live.com")]
        [TestCase("brainless@att.net")]
        [TestCase("firstname.lastname@example.com")]
        [TestCase("email@subdomain.example.com")]
        [TestCase("email@123.123.123.123")]
        [TestCase("firstname-lastname@example.com")]
        [TestCase("1234567890@example.com")]
        public void ConvertMessage_ValidSenderDoesNotThrowException(string validEmail)
        {
            var message = new Mock<EmailMessage>();

            Assert.DoesNotThrow(() => _converter.ConvertMessage(It.IsAny<string>(), validEmail, message.Object));
        }

        [Test]
        public void ConvertMessage_EmptySubjectReturnsFalse()
        {
            var message = new Mock<EmailMessage>();

            var body = "validemail@valid.com\n";

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message.Object));
        }

        [Test]
        public void ConvertMessage_InvalidSubjectThrowsMessageFormatException()
        {
            var message = new Mock<EmailMessage>();

            var body = "validemail@valid.com\nThisMessageSubjectLineIsTooLong";

            Assert.Throws<MessageFormatException>(() =>
                _converter.ConvertMessage(It.IsAny<string>(), body, message.Object));
        }

        [TestCase("01234567890123456789")]
        [TestCase("Thisis20charactersss")]
        [TestCase("1")]
        [TestCase("!\"£$\"%^&*()")]
        public void ConvertMessage_ValidSubjectSetsMessageTypeToStandardEmailMessage(string validSubject)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\n" + validSubject;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual(EmailMessageConverter.StandardEmailTypeString, message.Type);
        }

        [Test]
        public void ConvertMessage_ValidSubjectSetsSubject()
        {
            var message = new EmailMessage();
            var subject = "test";

            var body = "validemail@valid.com\n" + subject;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual(subject, message.Subject);
        }

        [TestCase("SIR12/12/12")]
        [TestCase("sir23/09/11")]
        [TestCase("SIR9/1/12")]
        public void ConvertMessage_SirSubjectSetsCorrectType(string subject)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\n" + subject;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual(EmailMessageConverter.SirTypeString, message.Type);
        }

        [Test]
        public void ConvertMessage_EmptySortCodeReturnsFalse()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\n";

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message));

        }

        [TestCase("Sort Code:123456")]
        [TestCase("invalid:12-12-12")]
        [TestCase("12-34-56")]
        public void ConvertMessage_InvalidSortCodeLineThrowsMessageFormatException(string sortCodeLine)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\n" + sortCodeLine;

            Assert.Throws<MessageFormatException>(() => _converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [TestCase("12-34-56")]
        public void ConvertMessage_ValidSortCodeSetsSortCode(string sortCode)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\nSort Code:" + sortCode;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual(sortCode, message.SortCode);
        }

        [Test]
        public void ConvertMessage_EmptyNatureOfIncidentLineReturnsFalse()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\nSort Code:12-12-12\n";

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [TestCase("Theft")]
        [TestCase("ATM Theft")]
        [TestCase("Intelligence")]
        public void ConvertMessage_ValidNatureOfIncidentSetsEmailProperty(string natureOfIncident)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\nSort Code:12-12-12\nNature of Incident:" + natureOfIncident;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual(natureOfIncident, message.NatureOfIncident);
        }

        [Test]
        public void ConvertMessage_InvalidIncidentLineThrowsMessageFormatException()
        {
            var message = new EmailMessage();

            var body = "validEmail@valid.com\nSIR12/12/12\nSort Code:12-12-12\nNot Correct:Dummy";

            Assert.Throws<MessageFormatException>(() => _converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [Test]
        public void ConvertMessage_InvalidNatureOfIncidentThrowsMessageFormatException()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\nSort Code:12-12-12\nNature of Incident:Incorrect";

            Assert.Throws<MessageFormatException>(() => _converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [Test]
        public void ConvertMessage_EmptyStandardMessageBodyReturnsFalse()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nStandardSubject\n";

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [Test]
        public void ConvertMessage_EmptySirMessageBodyReturnsFalse()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nSIR12/12/12\nSort Code:12-12-12\nNature of Incident:Theft\n";

            Assert.IsFalse(_converter.ConvertMessage(It.IsAny<string>(), body, message));
        }

        [TestCase("https://www.Google.com")]
        [TestCase("www.facebook.com")]
        [TestCase("http://linkedin.org")]
        [TestCase("ftp://listenfi.go:12")]
        public void ConvertMessage_UrlsPlacedInQuarantineList(string url)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nStandardSubject\nExtra " + url;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.IsTrue(message.Quarantined.Contains(url));
        }

        [TestCase("https://www.Google.com")]
        [TestCase("www.facebook.com")]
        [TestCase("http://linkedin.org")]
        [TestCase("ftp://listenfi.go:12")]
        public void ConvertMessage_UrlsReplacedByQuarantineString(string url)
        {
            var message = new EmailMessage();

            var body = "validemail@valid.com\nStandardSubject\nExtra " + url;

            _converter.ConvertMessage(It.IsAny<string>(), body, message);

            Assert.AreEqual("Extra <URL Quarantined>",message.Body);
        }

        [Test]
        public void ConvertMessage_CompleteMessageBodyReturnsTrue()
        {
            var message = new EmailMessage();

            var body = "validemail@valid.comn\nStandartSubject\nMessage body";

            Assert.IsTrue(_converter.ConvertMessage(It.IsAny<string>(),body,message));
        }
    }
}
