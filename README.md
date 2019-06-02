# Weather-Service-case-study
Weather service which will fetch city wise data from Open WeatherRESTful service

# Project Overview
 As per the problem statement, the developer solution is an Asp.net WebAPI project which will allow user to upload CSV file containing city names and unique IDs. Everyday, the user will upload a file and after successful file upload,validation and api calls to open weather service, it will create individual files for each city with current date and time in mentioned folder.
  
1) Have used https://joshclose.github.io/CsvHelper/ for validation and creation csv files. Its a very lightweight library.

2) Have used MsTest framework to create unit test cases.

3) All IDisposable classes, I have used in 'Using' tag, so it gets disposed automatically.

4) Have used single instance of Httpclient for multiple API calls, which makes object creation efficient and improves performance.

5) Have used Log4net library for logging errors in log file.

6) Have also added swagger library to WEBAPI project to test API

7) Repo contains valid (citylist.csv) file and invalid (citylist - Copy.csv,myapp - Copy.csv) files to test

# Things to be implemented
  1)We can use Polly based framework for retry mechanism in http calls.
  
  2)We can use chunked based file upload for large files or in case of slow internet.


# Alternative Approaches
  1) I was unsure whether its a windows service which will run on schedule and pick up file from server running the service. But this approach also can be used for above case study except file upload functionality.
  
  2) We can implement Azure Function based scheduler which will run on fixed schedule and will pick up city list file from Azure Blob or Azure File System. In addition to this, it will also store the city wise files on storage blob or Azure file system.
  


