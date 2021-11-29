using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using NapierBankMessaging.Event;
using NapierBankMessaging.MessageConvert;
using NapierBankMessaging.Model;
using Prism.Commands;
using Prism.Events;

namespace NapierBankMessaging.ViewModel
{
    public class MessageConverterViewModel : ViewModelBase, IMessageConverterViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private string _header;
        private string _body;
        private string _json;
        private Message _message;
        private readonly IHeaderValidator _headerValidator;
        private bool _isValidHeader;
        private ErrorMessage _errorMessage;
        private readonly Dictionary<string, MessageConverter> _converters;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly Dictionary<string, IMessageFactory> _messageFactories;
        public ICommand OpenMainWindowCommand { get; set; }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
                ValidateHeader();
                if (!IsValidHeader) HandleInvalidHeader();
                else HandleValidHeader();
            }
        }
        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                TryMessageConversion();
                OnPropertyChanged();
            }
        }

        public string Json
        {
            get => _json;
            set
            {
                _json = value;
                OnPropertyChanged();
            }
        }

        public Message Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool IsValidHeader
        {
            get => _isValidHeader;
            set
            {
                _isValidHeader = value;
                if (value)
                {
                    InitialiseMessage();
                }
                OnPropertyChanged();
            }
        }


        public ErrorMessage ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public MessageConverterViewModel(IEventAggregator eventAggregator, IHeaderValidator headerValidator, IMessageConverterDictionaryBuilder messageConverterDictionaryBuilder, IMessageFactoryDictionaryBuilder messageFactoryDictionaryBuilder)
        {
            _eventAggregator = eventAggregator;

            _headerValidator = headerValidator;

            _converters = messageConverterDictionaryBuilder.Build();

            _messageFactories = messageFactoryDictionaryBuilder.Build();

            ErrorMessage = new ErrorMessage();

            _jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true
            };

            OpenMainWindowCommand = new DelegateCommand(OnOpenMainWindowCommandExecute);
        }
        private void InitialiseMessage()
        {
            Message = _messageFactories[Header[..1]].CreateMessage();
        }

        private void OnOpenMainWindowCommandExecute()
        {
            _eventAggregator.GetEvent<OpenMainWindowEvent>().Publish();
        }
        private void ValidateHeader()
        {
            IsValidHeader = _headerValidator.ValidateHeader(Header);
        }
        private void HandleInvalidHeader()
        {
            ErrorMessage.Error = @"Invalid Header : The header must contain a message type character " +
                        "prefix (S for SMS, E for Email, or T for Tweet) followed " +
                        "by exactly 9 digits";

            ShowError();
        }

        private void ShowError()
        {
            Json = JsonSerializer.Serialize(ErrorMessage,_jsonOptions);
        }

        private void HandleValidHeader()
        {
        }

        private void TryMessageConversion()
        {
            if (!IsValidHeader) return;
            try
            {
                _converters[Header[..1]].ConvertMessage(Header, Body, Message);
                ShowSerializedMessage();
            }
            catch (Exception exception)
            {
                ErrorMessage.Error = exception.Message;
                ShowError();
            }
        }

        private void ShowSerializedMessage()
        {
            Json = JsonSerializer.Serialize(Message, Message.GetType(), _jsonOptions);
        }
    }
}
