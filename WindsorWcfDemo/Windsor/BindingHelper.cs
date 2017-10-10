using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Xml;

namespace WindsorWcfDemo.Windsor
{
    public static class BindingHelper
    {
        public static Binding GetWsHttpBinding()
        {
            var isReliableSessions = false;

            var wsHttpBinding = new WSHttpBinding
            {
                ReliableSession = new OptionalReliableSession
                {
                    Enabled = isReliableSessions,
                    InactivityTimeout = new TimeSpan(0, 0, 30, 0),
                    Ordered = false,
                },
                SendTimeout = new TimeSpan(0, 0, 30, 0),
                ReceiveTimeout = new TimeSpan(0, 0, 30, 0),
                OpenTimeout = new TimeSpan(0, 0, 0, 30),
                CloseTimeout = new TimeSpan(0, 0, 00, 30),
                BypassProxyOnLocal = true,
                TransactionFlow = false,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                MessageEncoding = WSMessageEncoding.Text,
                TextEncoding = Encoding.UTF8,
                UseDefaultWebProxy = true,
                AllowCookies = false,
                ReaderQuotas = new XmlDictionaryReaderQuotas
                {
                    MaxArrayLength = Int32.MaxValue,
                    MaxBytesPerRead = Int32.MaxValue,
                    MaxDepth = Int32.MaxValue,
                    MaxNameTableCharCount = Int32.MaxValue,
                    MaxStringContentLength = Int32.MaxValue
                },
                MaxReceivedMessageSize = Int32.MaxValue,
                MaxBufferPoolSize = Int32.MaxValue,
                Security =
                {
                    Mode = SecurityMode.TransportWithMessageCredential,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.None,
                        ProxyCredentialType = HttpProxyCredentialType.None,
                        Realm = "",
                    },
                    Message = { ClientCredentialType = MessageCredentialType.Certificate, EstablishSecurityContext = false}
                }
            };

            return isReliableSessions ? CreateBindingWithMaxPendingChannelsSet(wsHttpBinding) : wsHttpBinding;
        }

        private static Binding CreateBindingWithMaxPendingChannelsSet(Binding baseBinding)
        {
            BindingElementCollection elements = baseBinding.CreateBindingElements();
            var reliableSessionElement = elements.Find<ReliableSessionBindingElement>();
            if (reliableSessionElement != null)
            {
                reliableSessionElement.MaxPendingChannels = 2000;
                reliableSessionElement.MaxTransferWindowSize = 4096;
                reliableSessionElement.FlowControlEnabled = true;
                reliableSessionElement.MaxRetryCount = 8096;
                reliableSessionElement.InactivityTimeout = new TimeSpan(0, 0, 30, 0);
                reliableSessionElement.Ordered = true;

                var newBinding = new CustomBinding(elements)
                {
                    CloseTimeout = baseBinding.CloseTimeout,
                    OpenTimeout = baseBinding.OpenTimeout,
                    ReceiveTimeout = baseBinding.ReceiveTimeout,
                    SendTimeout = baseBinding.SendTimeout,
                    Name = baseBinding.Name,
                    Namespace = baseBinding.Namespace
                };

                return newBinding;
            }
            else
            {
                throw new Exception("the base binding does not " +
                                    "have ReliableSessionBindingElement");
            }
        }
    }
}