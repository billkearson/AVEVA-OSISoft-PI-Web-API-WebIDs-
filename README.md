# AVEVA OSISoft PI WebIDs - PIWebAPI 

A simple example of creating AVEVA OSISoft PI WebIDs client-side without having to make a call to the PI Server. There are at least five types with three of these types capable of being used with load balancers. These three WebID v2.0 types that are commonly used are as follows:

* Full - Contains Path and Object GUIDs that you have to know. 
* IDOnly - Contains the GUID.
* PathOnly - Contains the Path (Server\PIPoint)  

This example focusing on writing to a PIServer so the PathOnly type WedID will be focused on. This will same us the cost of performing a search to retrieve the WedID. Creating them cline-side is more efficient and just cool. The code shows how to do this in C# and then test the results by retrieving the PIPoint attributes and current value.
 
## Getting Started

Download or pull the repository and open the solution in Visual Studio. Copy the .env-sample file and rename it to .env. Edit the file and provide your environments information. Note that an functioning PI Server with the PIWebAPI is needed to use this code. Once the .env file is completed the provided form will display the values of the variables stored in the .env file on initial startup.  

* Variables stored in a .env file
Create a .env file or copy the .env-sample file and rename it. Populate this file with the needed information using a Name=Value format, per line. 

* Environment variable file  
PIAPISERVER=piapiservername    
PIAPISERVERPORT=443  
PIAPIUSERNAME=piapiusername  
PIAPIPASSWORD=piapipassword  
PISERVER=piservername  
DEBUG=true  
ENABLESSLVALIDATION=true  
USEDEFAULTCREDENTIALS=true  


## Built With

* [Microsoft Visual Studio Community 2022 (64-bit)](https://visualstudio.microsoft.com/) 
* [Microsoft .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) 

## Conclusion

The PI WebAPI is a great addition to the already awesome PI system. This will allow for small IoT like devices to write directly to the PI Server. Being able to create the WebID client-side makes the process efficient. Using the PathOnly WebIDs we are able to create these knowing the PI Server and the PI Point name. It doesn't get much easier than this.     

## References


* [AVEVA PI Web API - WedID](https://docs.aveva.com/bundle/pi-web-api/page/1023105.html) 
* [AVEVA PI Web API Reference - WebID Type (WebID)](https://docs.aveva.com/bundle/pi-web-api/page/1023105.html) 

## License

This project is licensed under the MIT License - see the LICENSE.txt file for details

* You can freely modify and reuse.
* The original license must be included with copies of this software.
* Please link back to this repo if you use the source code.
