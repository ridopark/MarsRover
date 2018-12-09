# MarsRover
MarsRover.Host
  - console app to read dates
    - reads date.txt for date inputs
  - appsettings.json
    - MarsRover:APIKey 
      - apikey to NASA MarsRover API
      - ex) "xxxxxxxxxxxdsssaaaaaaaa",
    - MarsRover:APIBaseUrl
      - base url to NASA MarsRover API
      - ex) "https://api.nasa.gov/mars-photos/api/v1/"
    - MarsRover:DateFilename
      - file name that stores input dates
      - ex) "dates.txt"
    - MarsRover:DownloadBasePath
      - path to download pictures
      - "./download/"
  - To build
    1. ~\MarsRover-master\dotnet restore
    2. ~\MarsRover-master\MarsRover.Host\dotnet build
  - To Run
    - ~\MarsRover-master\MarsRover.Host\bin\Debug\netcoreapp2.1\dotnet MarsRover.Host.dll
      - all pictures will download to .\download\[Date]\[RoverName]\[CameraName]
      - ex) .\download\2016-07-13\Curiosity\FHAZ\FLB_521681963EDR_F0551864FHAZ00323M_.JPG
  
 
MarsRover.Web 


To run 
npm install npm-install-peers
