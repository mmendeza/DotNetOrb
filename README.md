# DotNetOrb CORBA Standard Library for C#

 Welcome to the DotNetOrb CORBA Standard Library for C#!

This library is designed to provide seamless integration with the Common Object Request Broker Architecture (CORBA) standard in your C# applications.

With CORBA, you can easily communicate between distributed objects, regardless of the programming languages they are implemented in, making it a powerful tool for building robust and interoperable systems.

It's partially based on [JacOrb](https://www.jacorb.org) with an idl compiler built from scratch for C# and the communication layer built on top of [DotNetty](https://github.com/Azure/DotNetty) for performance.
 
## How to Contribute

We welcome contributions from the community to help improve and expand the functionality of DotNetOrb.

If you encounter any issues or have suggestions for new features, please feel free to open an issue or submit a pull request.  

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Installation
 
You can install DotNetOrb via [NuGet](https://www.nuget.org/packages/DotNetOrb.Core/): 

### Package Manager Console

  

```bash

Install-Package DotNetOrb.Core

```

  

### .NET CLI

  

```bash

dotnet add package DotNetOrb.Core

```

You can also install DotNetOrb IdlCompiler as a global tool

### .NET CLI
 

```bash

dotnet tool install --global DotNetOrb.IdlCompiler

```

## Configuration
Configuration options can be set in different ways:

 - as properties passed as arguments to ORB.Init() function.
 - as command line properties
 - as environment variables
 - in configuration files (appsettings.json)
 
 The ORBid property can  only be set with the first two options. The default ORBid is "DotNetOrb". 
 The settings in the configuration file must be included within a section with the ORBid name. 
 So for example an orb initialized with 

> -ORBid TestORB

 the configuration file would be:
 

      "TestORB": {
	      "OAIAddr": "myhost",
	      "OAPort": 3001
      }

  ### Listen endpoints
  To supply an explicit listener protocol and address the following properties can be used:
  
  
|Property|Description|Example|
|--|--|--|
|OAAddress|\<protocol>://\<address>|iiop://myhost:3001|
|OASSLAdress|\<protocol>://\<address>|iiop://myhost:443|
|OAIAddr|IP address|myhost|
|OAPort|Port|3001|
|OASSLPort|SSL Port|443|

Alternatively, you can use the command-line option 

> -ORBListenEndpoints iiop://myhost:3001

Multiple endpoints can be specified by delimiting endpoints with a semi-colon, or using multiple -ORBListenEndpoints options. 
For ssl endpoints use the form:

> -ORBListenEndpoints iiop://myhost:3001/ssl_port:443

### Initial references
Initial references are object references thar are available to CORBA application through *orb.ResolveInitialReferences* method. 
Property|Description|Example|
|--|--|--|
|ORBInitRef.\<service>|service will be resolved to the url specified|file:///tmp/tao imr locator.ior|

The url could be directly an IOR string or point to an http server or file.

### Codesets
|Property|Description|Example|
|--|--|--|
|DotNetOrb.NativeCharCodeset|codeset name|iso-8859-1|
|DotNetOrb.NativeWCharCodeset|codeset name|utf-16|

Currently supported codesets:

 - us-ascii
 - macintosh
 - iso-8859-1
 - iso-8859-15
 - utf-8
 - utf-16
 - UCS2

### Security
|Property|Description|Example|
|--|--|--|
|OASSLAdress|\<protocol>://\<address>|iiop://myhost:443|
|OASSLPort|SSL Port|443|
|DotNetOrb.Security.SSL.IsSupported|Must be "true" to activate ssl support|true/false|
|DotNetOrb.Security.SSL.Client.SupportedOptions|SSL client supported options|20|
|DotNetOrb.Security.SSL.Client.RequiredOptions|SSL client required options|1|
|DotNetOrb.Security.SSL.Server.SupportedOptions|SSL server supported options|40|
|DotNetOrb.Security.SSL.Server.RequiredOptions|SSL server required options|60|
|DotNetOrb.IIOP.SSL.Certificate|Path to certificate file|
|DotNetOrb.IIOP.SSL.CertificatePassword|Certificate password|

IIOP/SSL parameters (numbers are hex values, without the leading 0x):
 - NoProtection = 1 
 - EstablishTrustInClient = 40 
 - EstablishTrustInTarget = 20
 - Mutual authentication = 60
 ### POA Configuration
 |Property|Description|Default|
|--|--|--|
|DotNetOrb.POA.ThreadPoolMin|Minumum threads for request processing|5|
|DotNetOrb.POA.ThreadPoolMax|Maximum threads for request processing|20|
|DotNetOrb.POA.ThreadPoolShared|Use shared thread pool between all POAs. Only with ORB_CTRL_MODEL|false
|DotNetOrb.POA.ThreadTimeout|Whether to timeout witing to aquire a Request-Processor if the thread pool is empty|0 (wait for infinity)
|DotNetOrb.POA.ThreadPriority|Request processing threads in the POA will run at this priority|4 (Highest)
|DotNetOrb.POA.QueueWait|Whether the POA should boock when the request queue is full, or throw Transient exceptions|false
|DotNetOrb.POA.QueueMin| If QueueWait is true and the request queue get full, then the POA blocks until the queue contains no more than QueueMin requests|10
|DotNetOrb.POA.QueueMax|The max length of the request queue. If this length has been reached, and further request arrive, QueueWait specifies what to do|100
|DotNetOrb.POA.QueueListeners|Name of the classes that implement IRequestQueueListener|
|DotNetOrb.POA.CheckReplyEndTime|Set this to true for server-side checking of expired ReplyEndTimePolicy. This also applies to RelativeRoundtripTimeoutPolicy|false

### Implementation Repository
 |Property|Description|Default|
|--|--|--|
|DotNetOrb.UseImR|Activate to contact the Implementation Repository on evert server start-up. Mutually exclusive with UserTaoImr|false
|DotNetOrb.UseTaoImR|To contact the TAO Implementation Repository on every startup. Mutually exclusive with UseImR|false
|DotNetOrb.ImR.AllowAutoRegister|If set on servers that don't already have an entry on their first call to the IR, will get automatically registered. Otherwise an UnknownServerException is thrown|false
|DotNetOrb.ImR.CheckObjectLiveness|If set on the IR it will try to ping every object reference before returning it. If the reference is not alive Transient Exception is thrown|false
|ORBInitRef.ImplementationRepository|The initial reference to the IR|
|DotNetOrb.ImR.TableFile|File in which the IR stores data|
|DotNetOrb.ImR.BackupFile|Backup data file for the IR|
|DotNetOrb.ImR.IorFile|File to which the IR writes its IOR. This is usually referred to by the initial reference for the IR.|
|DotNetOrb.ImR.Timeout|Time in milliseconds that the IR will wait for a started server to register. After this timeout is exceeded the IR assumes the server has failed to start|12000 (2min)
|DotNetOrb.ImR.Endpoint.Host|Listen endpoint for the IR|0.0.0.0
|DotNetOrb.ImR.Endpoint.Port|Por number for the IR|0
|DotNetOrb.ImR.Endpoint.IsSSL|Enable SSL for the IR|false
|DotNetOrb.ImR.Endpoint.Certificate|Certificate file for SSL connections|
|DotNetOrb.ImR.Endpoint.CertificatePassword|Cetificate password|
|DotNetOrb.ImplName|The implementation name for persistent servers|
|DotNetOrb.Exec|Command used by the IR to start servers|


### Logging
To configure logging you must implement DotNetOrb.Core.ILogger interface and provide the class name in the following property:
|Property|Description|Example|
|--|--|--|
|DotNetOrb.Logger|Full name of the class that does the actual logging|MyServer.Logger

## Getting started
The general process of developing a CORBA application involves the following steps:

 1. Write an IDL specification.
 2. Compile this specification to generate c# classes (interfaces, classes, helpers, stubs and skeletons).
 3. Write an implementation for the interface generated in previous step.
 4. Write a main class thar instantiates the server implementation and registers it with the ORB.
 5. Write a client class that retrieves a reference to the server object and makes remote invocations.

As an example we will develop a simple chat application with the following  idl file 

*chat.idl*



    module Chat
    {
    	exception MaxUsersReached
    	{
    		long numUsers;
    	};
    
    	exception UserNotAuthenticated
    	{		
    	};
    
    	enum ColorEnum {
    		Default,
    		Green,
    		Yellow,
    		Red
    	};
    
    	struct Message
    	{
    		string from;
    		ColorEnum color;
    		wstring text;
    	};
    
    	interface Client {
    		readonly attribute string Name;
    		void SendMessage(in Message msg);
    	};
    
    	interface Server {
    		readonly attribute string Name;
    		boolean RegisterUser(in Client clientRef, out long sessionId) raises(MaxUsersReached);
    		void UnregisterUser(in long sessionId);
    		void BroadcastMessage(in long sessionId, in Message msg) raises(UserNotAuthenticated);
    	};
    };

To compile the idl we use the DotNetOrb.IdlCompiler tool:

    IdlCompiler usage:
      DotNetOrb.IdlCompiler [options] [files]
    
    Creates C# source code for the OMG IDL definition files.
    files: idl files containg OMG IDL definitions.
    
    options are:
    -h or -help                              help
    -i directory                             additional directories for idl file includes (multiple -i allowed)
    -o directory                             output directory. Default is .\
    -d define                                defines a preprocessor symbol
    -sequence_type [array | list]            type of sequence fields: array (default) or List
    -ami                                     generate async methods
    -naming_scheme [dotnet | idl]            respect original idl names or convert to dotnet naming (default)

```bash
DotNerOrb.IdlCompiler -o .\compiled_idls chat.idl
```
To provide an implementation of the functionality promised by the interface we should implement 

*ServerPOA*

    public class ChatServerImpl : ServerPOA
    {
        private int currentSessionId = 1;
        private int maxUsers = 5;
    
        private Dictionary<int, IClient> registeredClients = new Dictionary<int, IClient> ();
    
        public override string Name { get => "My chat server"; }
    
        public override void BroadcastMessage(int sessionId, Message message)
        {
            if (!registeredClients.ContainsKey(sessionId))
            {
                throw new UserNotAuthenticated();
            }            
            foreach (var kvp in registeredClients)
            {
                if (kvp.Key != sessionId)
                {
                    kvp.Value.SendMessage(message);
                }
            }
        }
    
        public override bool RegisterUser(IClient clientRef, out int sessionId)
        {
            if (registeredClients.Count >= maxUsers)
            {
                throw new MaxUsersReached(maxUsers);
            }
            sessionId = currentSessionId;
            registeredClients.Add(currentSessionId++ , clientRef);
            return true;
        }
    
        public override void UnregisterUser(int sessionId)
        {
            if (registeredClients.ContainsKey(sessionId))
            {
                registeredClients.Remove(sessionId);
            }
        }
    }

*ClientPOA*

    public class ChatClientImpl : ClientPOA
    {
        private string name;
        public override string Name
        {
            get
            {
                return name;
            }
        }
        public ChatClientImpl(string name) 
        { 
            this.name = name;
        }
    
        public override void SendMessage(Message msg)
        {
            switch (msg.Color)
            {
                case ColorEnum.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ColorEnum.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ColorEnum.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine();
            Console.WriteLine(msg.From + ">" + msg.Text);
            Console.ResetColor();
            Console.Write($"{name}>");
        }
    }

To run the server we should initialize the orb and obtain a reference to the object adapter (the POA) and activate it to start processing incoming requests.  We should also create an instance of our server class and make its object reference available, in this case with a simple file.

    ORB orb = (ORB)ORB.Init();
    var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
    poa.ThePOAManager.Activate();
    
    CORBA.IObject o = poa.ServantToReference(new ChatServerImpl());
    var ior = orb.ObjectToString(o);
    using (StreamWriter writer = new StreamWriter("C:\\temp\\chatServer.ior"))
    {
        writer.WriteLine(ior);
    }
    orb.Run();
The client will do something similar, and also obtain the server ior from the file to connect to it.
        

	ORB orb = (ORB)ORB.Init();
    var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
    poa.ThePOAManager.Activate();
    var client = new ChatClientImpl("myclient");
    CORBA.IObject o = poa.ServantToReference(client);
    string serverIOR = File.ReadAllText("C:\\temp\\chatServer.ior");
    var chatServer = ServerHelper.Narrow(orb.StringToObject(serverIOR));            
    try
    {
        if (chatServer.RegisterUser(client._This(orb), out int sessionId))
        {
	        var message = new Message("myclient", ColorEnum.Green, "Hello!");
            chatServer.BroadcastMessage(sessionId, message);
            chatServer.UnregisterUser(sessionId);
        }
    }
    catch (MaxUsersReached) 
    {
        //Max users reached
    }            

For more info, please refer to the examples folder and  [JacORB documentation](https://www.jacorb.org/documentation.html)



