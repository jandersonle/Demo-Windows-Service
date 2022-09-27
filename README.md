# Demo-Windows-Service
Project structure of a Windows Service with a stand-alone FTP service

This project represents a prototype Windows service with a modular standalone FTP service located under the subfolder, FTPLogging. Within FTPLogging there's a file called FTPHelper where you can specify the server and files you would like to access over a FTP connection. This project also features a semi-generic timer which accepts a method, a time interval, and number of times to execute and will execute the provided method x number of times with y milliseconds between each. 

## Installation


First build the project within Visual Studio, then run Visual Studio's command prompt with admin rights and run the following command - installutil.exe" Service1.exe

Once that is build, run services.msx as an admin and locate "justin's test service" and you may start the service. You can find the eventLog for this service within the event viewer under
the name myTestLog
