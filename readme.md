# Windsor WCF Demo


1. Create user account for IIS app pool
2. Create IIS website, binding to new app pool
2. Setup HTTPS and HTTP bindings in IIS
2. Browse to `https://localhost:443/services/myservice.svc`
2. Observe it serves the generated WCF page fine
2. Remove the HTTP binding
2. Observe error:

 `Service 'WindsorWcfDemo.Services.MyService' has zero application (non-infrastructure) endpoints. This might be because no configuration file was found for your application, or because no service element matching the service name could be found in the configuration file, or because no endpoints were defined in the service element.`


#### Notes
Using default app pool, the site can't run with the error

    HTTP could not register URL https://+:808/Services/MyService/. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details).

 If you run with a user and then revert back to the default application pool identity, this happens:

    Failed to listen on prefix 'https://+:443/Services/MyService/' because it conflicts with an existing registration on the machine

