# 🪨📃✂️

This is a small project playing around with a simple Domain Model based around the game Rock Paper Scissors. Inspired by a blog post by [Jan Kronquist](https://blog.jayway.com/2013/03/08/aggregates-event-sourcing-distilled/) and a number of excellent talks by Greg Young, but [this one](https://vimeo.com/131636650) in particular.

## Why do this?

The aim here was to take the ideas from Greg and implement them in a simple Domain, but with CQRS and Event Sourcing. I was particuarly keen to include the following adjustments given that it's 2022 and we can use some of the nice features of C# 9:

- Immutable Aggregates (state isn't mutated inside the commands, instead events are returned)
- Pattern matching to help make the code more DRY and functional
- C# Record types
- Implementing a left fold over the Event list to return the current state of an object

## What's left to do?

I'm keen to add some additional features to this repo including:

- An implementation purely in F#
- Projections as well as a Read Model
- A process manager and with event handlers (for handling multiple games?)
- Implementation of a [Event Store DB](https://www.eventstore.com/) for persistance
- A multi paradigm implementation with the Domain Model in F# and the Application Layer in C#
- Write an article on DEV.to about this code
- Add Lizard and Spock to the move types
