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
    1. ~\MarsRover-master> dotnet restore ./MarsRover.sln
    2. ~\MarsRover-master\MarsRover.Host> dotnet build
  - To Run
    - ~\MarsRover-master\MarsRover.Host> dotnet run
      - all pictures will download to .\\download\\[Date]\\[RoverName]\\[CameraName]\\filename.xxx
      - ex) .\download\2016-07-13\Curiosity\FHAZ\FLB_521681963EDR_F0551864FHAZ00323M_.JPG
  
MarsRover.Web 
  - https://marsroverweb-rido.azurewebsites.net/
  - To build and run WebApi service first.
    1. ~\MarsRover-master> dotnet restore ./MarsRover.sln
    2. ~\MarsRover-master\MarsRover.WebApi> dotnet build
    3. ~\MarsRover-master\MarsRover.WebApi> dotnet run
  - To build and run Web project.
    0. npm install -g @angular/cli -> installs angular cli
  	1. ~\MarsRover-master\MarsRover.Web\ClientApp> npm install npm-install-peers
  		- restores all node packages
    2. ~\MarsRover-master\MarsRover.Web> dotnet build    
    3. ~\MarsRover-master\MarsRover.Web> dotnet run
    4. When Angular Live Dev Server starts, use the url displayed as below to open you browser on the address. It takes a few seconds for the dev server to start.
    	** Angular Live Development Server is listening on localhost:1982, open your browser on http://localhost:1982/ **


Docker
  - To build docker image
    1. ~\MarsRover-master> docker build -t marsroverhost .
    2. ~\MarsRover-master> docker run --rm marsroverhost     
    3. ~\MarsRover-master> docker run --rm -v c:/temp:/app/download marsroverhost
      - or to use the shared drive to save pictures to c:/temp on the host.
       
