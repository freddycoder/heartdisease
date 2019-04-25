# Finding Heart Disease

The goal of this project is to experiment with AI technologie to find heart disease sign into data so it can be implement in device to prevent this type of disease. This can be sound, digits, image.

## Projects

This project is build using .Net Core 3.0. To run the project you must have a version of the sdk 3.0 and Visual Studio 2019

 - Visual Studio 2019 : https://visualstudio.microsoft.com/downloads/
 - .NET Core 3.0 : https://dotnet.microsoft.com/download/dotnet-core/3.0

## AI

This is the main librairy. For now, every algorythme is writen by me, but i want to incorporate keras model in the future. Or anything than can provide greate services. 

## AITweaker

This is a razor components app, so it is possible to play with the setting of the AI algoriythme with a UI.

## Heart

This is a console app. The main function train the agent and print information about the state of the agent.

This project also contains dataset. Here the source of those dataset :
 - https://www.kaggle.com/ronitf/heart-disease-uci
 - https://www.kaggle.com/shayanfazeli/heartbeat (this one is not in the repo because it's to big but it is present in the constant path variable)

# WebSite

Go to https://heartdiseasediagnosis.ca to try a train version of this agent. The agent that is execute is the release version of the origial branch which was train to get a accuracy of 0.83 on the dataset called heart.csv.

The code of this app is not in this repo for now because i need to secure some information first.
