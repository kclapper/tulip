## This script starts the local development database.
## Please make sure you have docker installed. If 
## you are using an M1 base Mac, please ensure the
## Rosetta 2 support is enabled. 

## To setup the database for the first time:
##       ./database.sh setup
##       dotnet dotnet-ef database update

## To start the database run: ./database.sh start
## To stop the database run: ./database.sh stop

## To delete the database run: ./database.sh delete

if [[ "$1" == "setup" ]] 
then
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Tu1ippetal!" -p 1433:1433 --name tulip_db --hostname tulip_db --platform linux/amd64 -d mcr.microsoft.com/mssql/server:2022-latest
elif [[ "$1" == "start" ]] 
then
    docker start tulip_db
elif [[ "$1" == "stop" ]] 
then
    docker stop tulip_db
elif [[ "$1" == "delete" ]] 
then
    docker stop tulip_db && docker rm tulip_db
else
    echo "Invalid argument"
fi
