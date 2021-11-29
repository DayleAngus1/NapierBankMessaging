using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NapierBankMessaging.Import;
using NapierBankMessaging.MessageConvert;
using NapierBankMessaging.Model;
using NapierBankMessaging.ViewModel;
using Prism.Events;

namespace NapierBankMessaging.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<HeaderValidator>().As<IHeaderValidator>().SingleInstance();

            builder.RegisterType<MainViewModel>().As<IMainViewModel>();

            builder.RegisterType<TextSpeakConverter>().As<ITextSpeakConverter>();

            builder.RegisterType<TextSpeakAbbreviationsCsvImporter>().As<ITextSpeakAbbreviationsImporter>()
                .WithParameter("filename", "C:\\Users\\owner\\source\\repos\\NapierBankMessaging\\NapierBankMessaging\\Data\\textwords.csv");

            builder.RegisterType<IncidentListCsvImporter>().As<IIncidentListImporter>().WithParameter("filename", "C:\\Users\\owner\\source\\repos\\NapierBankMessaging\\NapierBankMessaging\\Data\\incidents.csv");

            builder.RegisterType<MessageFactoryDictionaryBuilder>().As<IMessageFactoryDictionaryBuilder>();
            builder.RegisterType<MessageConverterDictionaryBuilder>().As<IMessageConverterDictionaryBuilder>();

            builder.RegisterType<MessageConverterViewModel>().As<IMessageConverterViewModel>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MessageConverterWindow>().AsSelf();

            return builder.Build();
        }
    }
}
