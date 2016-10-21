# Hangfire dome and patterns  
## Hangfire has 4 compontents  
  * Hangfire Client  
  * Hangfire Dashboard  
  * Hangfire Server  
  * Hangfire Storage  
  
##There are 6 projects in this solution  

###1. HangfireClient  
  * Add hangfire job definitions to Hangfire Storage. The job definitions are collected from the other project below  
       a) SydneyWeatherSnapShots - Job definitions are hard coded to call SydneyWeatherSnapShots.Jobs.SydneyWetherSnapShot()  
       b) SydneyWeatherSnapshotsConsole - Job defined to execute a console exe. Definitions are in default.conf and loaded by JsonConfig        c) PluginJobs - Job definition from MEFPlugin  
            Job definitions are MEF plugins and are loaded by MEFPluginJobAdaptor  
           
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
      * MEF plugins  
      * Json configuration file execute scripts and console programs  
