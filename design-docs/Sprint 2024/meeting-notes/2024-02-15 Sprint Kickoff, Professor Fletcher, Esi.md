# Sprint Kickoff February 15th, 2024

## Meeting with Professor Fletcher
- Showed him what we've done so far
- Get Sommee server and copy data from production to the local database
- Somee login
    - He'll send it to us
- Maybe move to Azure
    - He'll get in touch with the Lowell professor and see about
    getting Azure set up
- Professor Fletcher will connect us with the client (PhD student at Lowell)
    - Talk with her about refining the requirements
- Clean up the UI
    - Make it one UI framework

## Meeting with Esi
- She logs in with a Learn-ID
- Learn-ID
    - Connects to SAP
- SAP users just enter transactions
    - No indication of whether what they did is right or
    wrong
    - Business transactions
        - Receiving money / paying money
- SAP Problems
    - System is boring and technical
    - Project makes using SAP more enjoyable
- SAP users still use SAP directly
    - Use their interface
- Gamification app reads data from SAP to
give users a score and badges
    - Acts as a rewards / progression system
    - Points, badges, etc.
    - Makes doing your job more fun
- Case studies map to SAP business functions
- SAP being used in a school setting
    - Students learning to use SAP
    - Gamification system fosters engagement in students
    while learning SAP
- Learn-ID
    - Anonymous usernames for students
    - 1000 possible Learn-0 to Learn-999
    - No usecase for actual usernames right now
    - Maps to a Learn-ID in SAP
- Users sign up for SAP and get a Learn-ID
    - Gamification app uses the same Learn-ID
- Architecture
    - SAP API used to get information from SAP
        - REST API
    - Gamification ASP.NET server
        - Served by IIS
    - Database associated with ASP.NET project
- Admin
    - Can create users
        - Application server
            - The SAP instance
        - Client ID, the client using the SAP instance
        - Right now, she manually assigns new users
        a starter password
        - Users are not forced to change it on first 
        login
            - But they should be
        - Password should also be automatically generated?
        - Should passwords be emailed out on user creation?
- Dashboard (priority 1)
    - All game elements should be placed on the dashboard
        - Leaderboard, badges, user information, etc.
        - Leaderboard summary
            - Shows top 3
            - Then your position in context
- Esi has a design document for the Gamification Web App
    - She will share with us
    - Most of it is already accomplished
    - Our work starts on page 6, next phase
- Chat / Discussion Board (priority 1)
    - Promote collaboration amoung users
    - Uses can chat with each other
    - Users can chat with an LLM
        - Embeded in the dashboard
        - Maybe asking about the next steps
        - No data curation or training necessary
        - ChatGPT knows about SAP already
- More personalization elements (priority 2)
    - Profile information
    - Change your avatar
    - Users can change their profile information
    - Change avatar
- Clickable items (priority 3)
    - Badges and other dashboard items are clickable
    - Badges have names
    - Badges are greyed out if you don't have them
- Leagues (priority 3)
    - Leagues run for a week
    - Depending on points and progression you're ranked within a specific league
- User creation (priority 1)
    - User creation is broken right now
        - Doesn't use the chosen password
    - Can only create one user at a time
    - Wants to be able to create a bunch of users with
    the same **manually chosen** password 
    - Should email the user when a new user is created
        - Comes with login link
- Admin reports
    - Show all the Learn-IDs (show all the users)
    - Summary of user progressions
        - By case study
    - See students by case study
    - Create reports by student
    - Create aggregate user reports?
- The word "Total" instead of "Progress"
- [YouTube Video](https://www.youtube.com/watch?v=ExJ2yrwE0VY)

## Sprint Kickoff Meeting
### Agenda
- [ ] ~~Review issues closed during the last sprint~~
- [x] Review issues left open from last sprint
    - Remove from last sprint and possibly move to this sprint
- [ ] ~~Review product requirements and add new issues as necessary~~
- [x] Assign issues for this sprint

### Notes
- For PRs, instead of requiring reviews from everyone before merging,
just merge after ~1 day, whenever it feels right. Request reviews from
everyone but just as a way to let them know the code base is changing.
    - No need to slow down your progress to get reviews
- Use MVC Views for new UI we build
- Paul will take the admin panel work
    - Bulk user creation from CSV
    - Fix user password field on user creation
- Ramsey will take the leaderboard summary on dashboard
- Kyle will work on the SAP interface and development database setup 
- Kyle will update our requirements document with information from Esi's 
meeting