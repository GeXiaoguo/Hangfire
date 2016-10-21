# Hangfire dome and patterns  
Hangfire allows jobs to be defined as lambda expressions and thos lambda expressions are serialized as json string and saved in a database. Later on, those json string can be retrieve from the database, deserialized, compiled, and executed. This has the limitation that jobs can only defined in C# and any now job definition needs code change, rebuild and redeploy. This project provides several options to workaround this limitation. Two options are demoastrated  
  1. Implement jobs as MEF plugins. Job definition can be done by simply copyingthe plugin to the deployment folder  
  2. Implement jobs as console programs or scripts. The job defintion can be done with configuration files  
  
## Hangfire has 4 components  
 * Hangfire Client  
 * Hangfire Dashboard  
 * Hangfire Server  
 * Hangfire Storage    
 These 4 components are distributed in the followign 6 projects  
 
##The 6 projects in this solution  

###1. HangfireClient  
  *  Add hangfire job definitions to Hangfire Storage. The job definitions are collected from the other project below  
       a)  SydneyWeatherSnapShots  
           Job implemented as static method in class library  
           Job definitions are hard coded in C#  
           
       b)  SydneyWeatherSnapshotsConsole  
           Job is implemented as a console exe.   
           Job definitions are in default.conf and loaded by JsonConfig   
           
       c)  PluginJobs  
           Job is implemented as a MEF plugin  
           The plugin is loaded by MEFPluginJobAdaptor  
           
  * Start the Hangfire Dashboard   
    
###2. HangfireServer  
  A console program which starts the Hangfire Server. Hangfire server will keep retrieving job definitions from Hangfire storage and exectues them.  
  Jobs are defined as lambda expressions. HandfireServer need to be able to reference all assembleis that are referenced by the lambda expression  
     
###3. SydneyWeatherSnapShots  
  A class library which define method SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot().  
  Every time this method is called, it polls the current temperature from http://api.openweathermap.org/ and writes it to a local database  
     
###4. SydneyWeatherSnapshotsConsole  
  A console program calling SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot() when executed  
    
###5. PluginJobs   
  A class library which defines class SampleJob which can be dynanically loaded by MEFPluginJobAdaptor as long as the SampleJob assembly is in the search path  
  SampleJob.Execute simply calls SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot()  
     
###6. HangfireCommon  
  Utilities to allow define Hangfire jobs with  
    1. MEF plugins  
    2. Json configuration file execute scripts and console programs  
