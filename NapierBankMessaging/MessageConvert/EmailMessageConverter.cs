using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using NapierBankMessaging.Import;
using NapierBankMessaging.Model;

namespace NapierBankMessaging.MessageConvert
{
    public class EmailMessageConverter : MessageConverter
    {
        private readonly List<string> _incidents;
        public static string SirTypeString = "Significant Incident Report";
        public static string StandardEmailTypeString = "Standard Email Message";
        private const string UrlPattern = @"(((http|ftp|https):\/\/)?[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";

        public EmailMessageConverter(IIncidentListImporter incidentListImporter)
        {
            _incidents = incidentListImporter.ImportIncidentList();
        }
        public override bool ConvertMessage(string header, string body, Message message)
        {
            //Check if message is of type EmailMessage
            if (!(message is EmailMessage emailMessage))
            {
                throw new MessageTypeMismatchException("Message must be of type EmailMessage for this converter");
            }

            // Set message header
            message.Header = header;

            using var reader = new StringReader(body);

            // Read First line of body
            var sender = reader.ReadLine();

            // Return false if first line of body is empty
            if (string.IsNullOrEmpty(sender)) return false;

            // Throw InvalidSenderException if the first line of the message is not a valid email address
            if (!ValidateSender(sender))
                throw new InvalidSenderException(
                    "Invalid Sender : The first line of an Email Message body must be the senders Email address");

            // Set the message sender to the valid email address;
            emailMessage.Sender = sender.Trim();

            // Read second line of body
            var subject = reader.ReadLine();

            // Return false if the second line of the body is empty or null
            if (string.IsNullOrEmpty(subject)) return false;

            // Throw message format exception if subject is too long
            if (!ValidateSubject(subject))
                throw new MessageFormatException("Format Error : Subject must be less than or equal to 20 characters");

            // Set message subject
            emailMessage.Subject = subject;

            // Get the type of message from the subject line
            var type = GetEmailTypeFromSubject(subject);

            // Set email type
            emailMessage.Type = type;

            // If the message type is a Significant Incident Report
            if (string.Equals(type, SirTypeString))
            {
                // Read Line three of the Significant incident report
                var sortCodeLine = reader.ReadLine();
                
                // if the third line of the message is empty return false
                if(string.IsNullOrEmpty(sortCodeLine)) return false;

                // Throw a MessageFormatException if the sort code line is not valid
                if (!ValidateSortCodeLine(sortCodeLine))
                    throw new MessageFormatException(
                        "Format Error : Significant Incident Reports must have a Sort Code after the subject line in the form - Sort Code: 99-99-99");

                // Extract the sort code from the sort code line
                var sortCode = GetSortCode(sortCodeLine);

                // Set the message sort code
                emailMessage.SortCode = sortCode;

                // Read line four of the message
                var incidentLine = reader.ReadLine();

                // Return false if the fourth line of the message is empty
                if(string.IsNullOrEmpty(incidentLine)) return false;

                // Throw a MessageFormatException if the Nature of Incident line is incorrectly formatted
                if (!ValidateIncidentLine(incidentLine))
                    throw new MessageFormatException(
                        "Format Error : Significant Incident Reports must have a Nature of Incident after the Sort Code in the example form Nature of Incident: Theft");

                // Extract the nature of incident from the incident line
                var natureOfIncident = GetNatureOfIncident(incidentLine);

                // If the message does not have a nature of incident return false
                if(string.IsNullOrEmpty(natureOfIncident)) return false;

                // Throw MessageFormatException if the nature of incident is invalid
                if (!ValidateNatureOfIncident(natureOfIncident))
                    throw new MessageFormatException(
                        "Format Error : Nature of Incident is not Valid. Value must be one of the following - " +
                        string.Join(", ", _incidents));

                // Set the NatureOfIncident email field equal to a correctly formatted string
                emailMessage.NatureOfIncident = _incidents.FirstOrDefault(x =>
                    x.Equals(natureOfIncident, StringComparison.InvariantCultureIgnoreCase));
            }

            // Read the rest of the message
            var messageText = reader.ReadToEnd();

            // If the message is empty return false
            if (string.IsNullOrEmpty(messageText)) return false;

            // Get a list of URLs from the message
            var quarantinedUrls = GetQuarantinedUrls(messageText);

            // Set the List of Quarantined URLs
            emailMessage.Quarantined = quarantinedUrls;

            // Remove Quarantined URLs from the message text
            emailMessage.Body = QuarantineUrls(messageText,quarantinedUrls);

            return true;

        }

        private string QuarantineUrls(string messageText, List<string> quarantinedUrls)
        {
            var quarantinedText = messageText;
            foreach (var quarantinedUrl in quarantinedUrls)
            {
                quarantinedText = quarantinedText.Replace(quarantinedUrl, "<URL Quarantined>");
            }

            return quarantinedText;
        }

        private List<string> GetQuarantinedUrls(string messageText)
        {
            return Regex.Matches(messageText,
                pattern: UrlPattern).Select(x => x.Value).ToList();
        }

        private bool ValidateNatureOfIncident(string natureOfIncident)
        {
            return _incidents.Exists(x => x.Equals(natureOfIncident, StringComparison.InvariantCultureIgnoreCase));
        }

        private string GetNatureOfIncident(string incidentLine)
        {
            return incidentLine.Replace("Nature of Incident", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace(":", "").Trim();
        }

        private bool ValidateIncidentLine(string incidentLine)
        {
            return incidentLine.StartsWith("Nature of Incident:", StringComparison.InvariantCultureIgnoreCase) ||
                   incidentLine.StartsWith("Nature of Incident :", StringComparison.InvariantCultureIgnoreCase);
        }

        private bool ValidateSortCodeLine(string sortCodeLine)
        {
            return Regex.IsMatch(sortCodeLine, "^[S|s]ort [C|c]ode\\s?:\\s?[0-9][0-9]-[0-9][0-9]-[0-9][0-9]");
        }

        private string GetSortCode(string sortCodeLine)
        {
            return sortCodeLine.Replace("Sort Code", "", StringComparison.InvariantCultureIgnoreCase).Replace(":","").Trim();
        }

        private string GetEmailTypeFromSubject(string subject)
        {
            return Regex.IsMatch(subject, "^(SIR||sir)[0-3]?[0-9]/[0-1]?[0-9]/[0-9][0-9]") ? SirTypeString : StandardEmailTypeString;
        }

        private bool ValidateSubject(string subject)
        {
            return subject.Length <= 20;
        }

        public override bool ValidateSender(string sender)
        {
            if (sender.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(sender);
                return addr.Address == sender;
            }
            catch
            {
                return false;
            }
        }
    }
}
