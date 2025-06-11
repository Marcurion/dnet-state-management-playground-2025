# Purpose
This workspace aims to explore the concept of a thread-safe, and centralized State Management based on MediatR that is easy to consume by the UI. Think of reacting to changes that affect a lot of different UI components.
The following issues were attempted to be addressed by this:
- Separation of UI and State into the Frontend and Application/Domain layer
- By extracting the state from the Frontend, it can be accessed and altered from other sources as well
- The Frontend consumes a representation of a state instance
- Declarative UI approach (the UI is constructed based on that state instance)
- Reduction of unnecessary render calls / trigger of manual updates for state consumers (after the initial render the components are only redrawn when the relevant state changes)
- Partial UI updates by using multiple states (separation of concerns)
- Basic CSS animation support
- Thread-safety (no unnoticed loss of changes/race conditions)
- Centralized Single Source of Truth with Subscriber functionality


# Why MediatR?
MediatR was chosen since it's PipelineBehaviours could be leveraged for several useful feats like validation, change detection, thread-synchronization, request modification and state injection. The concept of request polymorphism also seemed to align well initially with my intention to re-use pipelines and generalize functionality even though when Generics come into play this will scale not ideally.
I am aware of the immanent license changes for this package but confident similar results can be achieved with the many available alternatives, like:
https://github.com/martinothamar/Mediator

# Why did you not use [Validation, ErrorOr, Result Pattern, ...] for the StateDemo? 
Since I foremost wanted to explore the concept of using MediatR for State Management I kept the demo very pure. Even though I believe this approach would benefit from Validation and using the Result pattern instead of throwing exceptions, I did not want to make it seem like a decision for A means also you are locked into B, C & D.

# How to run and what to see
There are three XUnit test projects that should run green.
Also, you can run the Blazor Webapp "Frontend". A browser window should open (or you navigate to http://localhost:5197/)  where you are greeted by 4 widgets in the "Home" tab and a headline "App State Management Demo". The 4 widgets are 2 state producers and 2 state consumers. You can add any text via the text input of a producer, and it will be reflected in the list view of the consumer. When adding a new item to the list there should be a simple animation on all the list items. Also note that the state is persisted when changing the tab to "Counter" and back to "Home" again, as long as the app is running. Observe the output of the .net console (not browser console) where it is logged when a widget is redrawn, which in the case of list updates happens twice in a row to correctly trigger CSS animations.

# Most Relevant files
Search for the // Notable: comments.
 And to jump right into the matter you can have a look at:
 - **Src/Application/StateManagement/Pipeline** where the magic happens
 - **Src/Frontend/Components/Pages** for seeing how the state is updated and consumed:
   - **AppState1Producer.razor**
   - **AppState1Consumer.razor**
   - **AppStateDemo.razor**
 - If you want to learn more about the thread-safety aspects you can take a look at the tests in **Tests/Tests.Integration/AppState1Tests.cs**

# Synergies and future potential
The following topics could create synergies with this specific approach:
- Domain Validation: Using Validation in the MediatR pipelines for Set State operations could result in a better validated app state and domian model
- User Session Isolation & Concurrency: This approach is already developed with thread safety in mind, it could easily be extended to introduce a per user session state either in the PipelineBehaviours or in the AppState representation
- Result Pattern: Introducing the result pattern e.g. ErrorOr for the Set and Get State operations would make error/exception handling more explicit, especially as more error potentials are introduced e.g. Permissions 
- Event Handling: If we consider an event driven system MediatR is a good tool for that
- Authorization: Sitting between the presentation layer and the business side, introducing permissions checks for operations via PipelineBehaviours seems feasible