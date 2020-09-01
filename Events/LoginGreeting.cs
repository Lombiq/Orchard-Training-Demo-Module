/*
 * Orchard exposes a lot of events that you can hook into to do anything. These events are commonly in the form that
 * you implement an interface and then Orchard will call into your implementation (so, not .NET-style events). We've
 * already seen an example of this with PersonPartHandler but here's a more event handler-like event handler, about
 * login events.
 */

using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Users.Events;
using System;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Events
{
    // ILoginFormEvent exposes events of the, well, login form :). Useful to display a login greeting or anything even
    // more useful! The rest of it is pretty standard and we just use INotifier again.
    public class LoginGreeting : ILoginFormEvent
    {
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer T;


        public LoginGreeting(INotifier notifier, IHtmlLocalizer<LoginGreeting> htmlLocalizer)
        {
            _notifier = notifier;
            T = htmlLocalizer;
        }


        public Task LoggedInAsync(string userName)
        {
            _notifier.Success(T["Hi {0}!", userName]);
            return Task.CompletedTask;
        }

        public Task LoggingInAsync(string userName, Action<string, string> reportError) => Task.CompletedTask;

        public Task LoggingInFailedAsync(string userName) => Task.CompletedTask;
    }
}

// END OF TRAINING SECTION: Event handlers

// NEXT STATION: Controllers/ApiController.cs
