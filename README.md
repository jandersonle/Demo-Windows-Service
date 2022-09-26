# DemoWindowsService
Project structure of a Windows Service with a stand-alone FTP service

This project represents a prototype Windows service with a modular standalone FTP service located under the subfolder, FTPLogging. Within FTPLogging there's a file called FTPHelper where you can specify the server and files you would like to access over a FTP connection. This project also features a semi-generic timer which accepts a method, a time interval, and number of times to execute and will execute the provided method x number of times with y milliseconds between each. 
