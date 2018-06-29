#!/bin/bash

dotnet publish PlogBot.App -c Release
dotnet publish PlogBot.DataSync -c Release
dotnet publish PlogBot.Alerts -c Release
scp -r ./PlogBot.App/bin/Release/netcoreapp2.0/publish lfeldman@leolinux.eastus.cloudapp.azure.com:~/plogbot/app
scp -r ./PlogBot.DataSync/bin/Release/netcoreapp2.0/publish lfeldman@leolinux.eastus.cloudapp.azure.com:~/plogbot/datasync
scp -r ./PlogBot.Alerts/bin/Release/netcoreapp2.1/publish lfeldman@leolinux.eastus.cloudapp.azure.com:~/plogbot/alerts