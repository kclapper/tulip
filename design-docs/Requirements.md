# Tulip 

This document describes the requirements for Project Tulip, the
SAP gamification web app. Tulip is a web app built on top of the
SAP enterprise resource planning (ERP) software platform. SAP is
used by businesses and other large organizations for a variety
of adminstrative tasks including: finance, inventory,
sales, and analytics. The main goal of Tulip is to provide fun
incentives to make using SAP more enjoyable.

To that end, Tulip's interface makes using SAP feel similar to 
playing a game. Performing various tasks in SAP lets users earn 
points, badges, medals, and other rewards in the Tulip interface.
Community elements such as leaderboards and chat
help keep users engaged in a similar way to many multiplayer
video games. 

Tulip provides value to users in 2 main ways:
1. **Fun** - Tulip makes the act of using SAP enjoyable 
2. **Connection** - Tulip provides users a way to connect with each other 

The following sections detail the Tulip project requirements. Each 
requirement is numbered and linked to allow for easy reference. 

Note the use of _shall_, _should_, and _may_. _Shall_ denotes
a requirement. _Should_ denotes a recommendation. _May_ 
denotes permission.

# Tulip Product Requirements
Tulip has been in development prior to this team
joining the project. The following requirements 
describe only the features this team will work on.

## 1. [Dashboard](#dashboard)
### 1.1. [Case Study Selection](#case-study-selection)
The user shall be able to select which case study to examine on
the dashboard.
### 1.1. [Points](#points)
The dashboard shall show how many points the user has in
the currently selected case study.
### 1.2. [Badges](#badges)
The dashboard shall show which badges the user has earned for
the currently selected case study.
### 1.3. [Leveling System](#levels)
The dashboard shall show the user's level in
the currently selected case study.
### 1.4 [Medals](#medals)
The dashboard shall show the user's medal in the
currently selected case study.
### 1.5 [User Profile](#medals)
The dashboard shall show information about the current
user on the dashboard.

## 2. [Leaderboard](#leaderboard)
### 2.1. [Grouping](#leaderboard-groups)
The leaderboard shall be able to display users
ranked within user specified groups.
### 2.1.2. [Class Group](#leaderboard-class-groups)
The leaderboard shall be able to be grouped by class.
## 2.2. [Leaderboard Summary](#leaderboard-summary)
There shall be a leaderboard summary on each user's 
dashboard.
### 2.2.1. [Top 3 Users](#top-3-users)
The leaderboard summary shall show the top 3 users
on the leaderboard.
### 2.2.2. [User Context](#user-leaderboard-context)
The leaderboard summary shall show the location of 
the user on the leaderboard and the nearby 
leaderboard positions.

## 3. [Chat System](#chat)
There shall be a chat system in the application.
### 3.1. [User Chat](#user-chat)
Users shall be able to chat with each other.
### 3.2. [AI Chat](#ai-chat)
Users shall be able to chat with an AI about the 
SAP system.

## 4. [Admin Panel](#admin-panel)
### 4.1. [User Creation](#admin-user-creation)
The admin panel shall allow admins to create individual and large groups of 
users.
#### 4.1.1. [Individual User Creation](#individual-user-creation)
##### 4.1.1.1. [Manually Assigned Password](#manual-password)
Admin's shall be able to manuall assign a password to a new user.
#### 4.1.2. [Bulk User Creation](#bulk-user-creation)
A large group of users shall be able to be created all at once.
##### 4.1.2.1. [CSV User Creation](#csv-user-creation)
A CSV file can be used to bulk create users.
##### 4.1.2.2. [Excel User Creation](#excel-user-creation)
An Excel file can be used to bulk create users.

## 5. [Production Deployment](#deployment)
The application shall be deployed to the Azure cloud.

## 6. [Development Environment](#dev-environment)
### 6.1. [Local Database](#dev-db)
The development environment shall have a local database that is
used by the app during development. The development environment 
shall not connect to the production database.
### 6.1.1. [Local Data Replica](#local-data)
The development database shall have a copy of the production data
### 6.2. [Single UI Framework](#ui-framework)
The codebase shall use a single ASP.NET UI framework (MVC, Razor pages, Blazor, etc.). 