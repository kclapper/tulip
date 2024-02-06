# CS682 Development Process

Hey folks, I have some ideas about the process
we could use to build our CS682 project and I was
hoping to get some feedback from you both. Would you
be able to read through so we can discuss at our first
meeting? 

My ideas are broken into four major components:
1. Requirements document
2. GitHub Issues
3. Weekly releases
4. Testing plan 

## Requirements Document

In our first lecture, Professor Fletcher spoke about the 
importance of having detailed project requirements. I 
suggest that we explicitly write out our product 
requirements in fine grained detail in a dedicated
requirements document. This document would give us clarity
about the scope of the project and would help guide us
during our bi-weekly sprints. Whenever we work on something,
we should be able to tie it back to a requirement to ensure
that the work we do is actually driving the product forward.

At the start of the project we might not have a fully formed 
idea of what all the requirements are, we may only know the 
broad strokes. At the beginning of the semester I think we 
should lay out the requirements in as much detail as we can.
Then at the beginning of each sprint, we can pick a requirement
area to focus on and break it own into smaller, more 
actionable, and specific requirements. As we work on the project
we'll learn more about it and be able to refine our requirements.

I've included a [sample requirements document](REQUIREMENTS.md) in this repository.

## GitHub Issues

We should have a system for splitting up the work that needs to 
be done to satisfy our product requirements. I think we should use
GitHub issues for this. We can open one or more GitHub issues for 
each requirement we're going to work on. Issues allow us to assign 
someone to work on them and can be tied to a pull request when the work
is ready to be merged into the codebase.  

There is a template feature for GitHub issues that makes it so each issue
has a similar format. It think it would be useful for each issue we create
to say explicitly which product requirement it relates to. This 
would help us stay organized and make sure that we're staying 
focused on the work that needs to be done. 

At the beginning of each sprint, after we review and refine our product 
requirements, we should create GitHub issues for each item we'll be working
on for that sprint. These issues should be well defined and focused on
individual tasks that need to be accomplished. The issues we create should
be small enough that they can be accomplished within a given sprint. 

GitHub issues should also be created whenever we find a bug that needs
to be fixed. These bugs don't necessarily have to be fixed during the 
sprint they're found, but they should be added to the backlog so we 
can track and eventually fix them. At the beginning of each sprint, we
should review the remaining bug tickets and determine if they should
be fixed that sprint.
 
## Weekly Releases

I think it's very valuable to build a minimally working prototype as 
quickly as possible and add features to it in frequent small increments.
Having a working prototype allows us to better assess whether or not it's 
meeting the requirements and lets us learn more about how it will be 
used. It also lets us more quickly identify bugs that affect the user 
experience.

To that end, I think we should release a new version of the product at the
end of each week. This should be an internal release that's only visible to us,
but that allows us to assess how the product is coming along. This also gives 
us a timeline for integrating our individual work. Each week we would take 
what we've done and put it all together. These releases should be numbered 
with a [semantic version number](https://semver.org/).

These releases also give us something to present to the client at the end of 
each sprint. Since the sprints are two weeks long, doing weekly releases means
we would do at least one internal build before presenting to the client. This
gives us an opportunity to identify and correct bugs in the prototype before
asking the client to evaluate it.

## Testing

We should have a testing plan which goes along with our requirements document.
This plan could either be a separate written document, a special Issue template,
or an informal process we come up with. Whatever form it takes, the testing 
plan should have steps to verify each product requirement has been 
met successfully. We should test each weekly release according to this plan 
to identify any bugs or unmet requirements so new Issues can be created and
addressed.

This testing plan should be in addition to the automated unit and integration 
tests we write for our code. Ideally, it's something we perform manually to verify the product we built
meets the requirements we outlined. 

## Sprint Timeline
I think it would be useful to meet once a week (separately from our bi-weekly meetings with professor Fletcher). Assuming
the sprints start on Thursday, weekly meetings give us a 
change to prepare each sprint and touch base in the middle
to make sure we're on track. I suggest Thursdays at 2:30pm,
or whatever time allows us to meet before our 
meetings with professor Fletcher.

This is what I image our sprints might look like:

### Week One (start of sprint)
- **Day 1, Thursday**: _Weekly meeting_. Review our progress 
from the last sprint. 
Review test results from our most recent weekly release.
Review and refine product requirements. Create and assign 
GitHub Issues for the sprint. (Maybe this is where we 
touch base with the client too? To make sure we're meeting
their expectations). 
- **Days 2 - 5**: Work on GitHub Issues. Create, review,
and merge pull requests. 
- **Day 7, Wednesday**: Finish reviewing and merging PRs.
Create new weekly release. Perform test plan.  

### Week Two (mid sprint review)
- **Day 8, Thusrday**: _Weekly meeting_. Review test results 
from last weekly 
release and create any necessary bug Issues. Review the 
progress we've made and check in to make sure everyone has
what they need to move forward.
- **Days 9-13**: Work on GitHub Issues. Create, review,
and merge pull requests. 
- **Day 14, Wednesday**: Finish reviewing and merging PRs.
Create new weekly release. Perform test plan.  

## Other Ideas

I'm interested to know if you have any comments or feedback for these suggestions
I've outlined. I also want to know if you have thoughts about the following ideas
as well:
- **Code Name**: Can we please use a fun code name for our
project? Saying _"Gamification Web App"_ everytime is a bit
of a mouthfull. Could we call it something like _"Skyline"_
or _"Tulip"_? I'm open to suggestions :)
- **Git strategy**: How do we want to manage git? Do we want to do a traditional 
fork and pull strategy or just have working branches off one repository? If 
professor Fletcher gives us an existing code base then this may be somewhat decided
for us but I want to know your thoughts.
- **Pull Requests**: I think it would be valuable to do code reviews on each pull request.
When we have working code we're ready to merge back into the main branch, I think it would
be a good idea to have at least one other person review each PR for code quality.
- **Unit Tests / TDD**: I feel strongly that we should have unit tests for major components 
of the codebase or for any public apis. I think that any code we submit should have
an associated set of unit tests. What do you guys think about this? Are there any guidelines
we should follow when it comes to unit tests? 
- **CI/CD**: I'm a big fan of GitHub Actions for CI/CD and I've used it a lot. I'd be happy to 
set up a CI/CD pipeline. I think it would be useful to have automated tests/linting run for each
PR we create and for each accepted PR into main. What do you guys think?