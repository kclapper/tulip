# Tulip 

This document describes the requirements for Project Tulip, the
SAP gamification web app. Tulip is a web app built on top of the
SAP enterprise resource planning (ERP) software platform. SAP is
used by businesses and other large organizations for a variety
of adminstrative tasks including: finance, inventory,
sales, and analytics. The main goal of Tulip is to provide an 
easier to use and more enjoyable interface to SAP.

To that end, Tulip's interface makes using SAP feel similar to 
playing a game. Performing various tasks in SAP using the Tulip 
interface lets users earn points, badges, medals, and rank up to
higher levels. Community elements such as leaderboards and chat
help keep users engaged in a similar way to many multiplayer
video games. Where possible, Tulip simplifies the process of 
performing SAP actions to help players avoid frustration and
focus on the pleasant parts of the experience.

Tulip also takes into account the experience of SAP system
administrators. While not focused on the gamification features,
Tulip's interface simplifies administrative tasks, letting admins
spend less time and frustration using the system. 

Tulip provides value to users and administrators in 3 main ways:
1. **Ease of use** - Tulip makes SAP easier to use for users and administrators   
2. **Fun** - Tulip makes the act of using SAP enjoyable 
3. **Connection** - Tulip provides users a way to connect with each other 
Any features/requirements added to Tulip should serve one or more of these
values.

The following sections detail the Tulip project requirements. Each 
requirement is numbered and linked to allow for easy reference. 

Note the use of _shall_, _should_, and _may_. _Shall_ denotes
a requirement. _Should_ denotes a recommendation. _May_ 
denotes permission.

# Tulip Product Requirements
Tulip has been in development prior to this team
joining the project. The following requirements 
describe only the features this team will work on.

## 1. [Achievements](#achievements)
### 1.1. [Points](#points)
### 1.2. [Badges](#badges)
### 1.3. [Leveling System](#levels)
### 1.4 [Medals](#medals)

## 2. [Leaderboard](#leaderboard)
### 2.1. [Grouping](#leaderboard-groups)
The leaderboard shall be able to display users
ranked within user specified groups.
### 2.1.2. [Class Group](#leaderboard-class-groups)
The leaderboard shall be able to be grouped by class.

## 3. [Chat System](#chat)
### 3.1. [User Chat](#user-chat)
### 3.2. [AI Chat](#ai-chat)

## 4. [Admin Panel](#admin-panel)
### 4.1. [Bulk User Creation](#admin-bulk-creation)
The admin panel shall allow admins to create large groups of users at once by importing from a file.
### 4.1.1. [CSV User Creation](#csv-user-creation)
A CSV file can be used to bulk create users.
### 4.1.1. [Excel User Creation](#excel-user-creation)
An Excel file can be used to bulk create users.

## 5. [Browser Extension](#extension)