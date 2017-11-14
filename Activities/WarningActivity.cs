using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.UI.Notify;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;

namespace OrchardHUN.TrainingDemo.Activities
{
    // Extending Workflows: this simple sample shows how a configurable warning notification can be displayed.
    // In NotificationActivity this feature is built-in, just using notifications for the sake of simplicity.

    // This will be a Workflow activity that is a Task, i.e. that executes something. It could be an Event too, which
    // could start (trigger) a Workflow.
    public class WarningActivity : Task
    {
        public const string FormName = "WarningActivity";
        public const string MessageFieldName = "Message";

        private readonly INotifier _notifier;

        public Localizer T { get; set; }


        public WarningActivity(INotifier notifier)
        {
            _notifier = notifier;

            T = NullLocalizer.Instance;
        }


        // Configuring some metadata of the activity.
        public override string Name
        {
            get { return "Warning"; }
        }

        public override LocalizedString Category
        {
            get { return T("Notification"); }
        }

        public override LocalizedString Description
        {
            get { return T("Display a warning notification."); }
        }

        // The name of the form which can be used to configure the activity. For the form itself see below.
        public override string Form
        {
            get { return FormName; }
        }

        // The activity can have multiple outcomes, i.e. multiple branches that other activities can be linked onto.
        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Done");
        }

        // Implementing what the activity actually does.
        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            // What was previously saved via the form can be fetched from the Workflow's state here.
            var message = activityContext.GetState<string>(MessageFieldName);
            _notifier.Warning(T(message));
            yield return T("Done");
        }
    }


    // The form that can be used to configure the activity.
    public class WarningActivityForms : IFormProvider
    {
        private readonly dynamic _shapeFactory;

        public Localizer T { get; set; }


        public WarningActivityForms(IShapeFactory shapeFactory)
        {
            _shapeFactory = shapeFactory;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context)
        {
            // Note that the form should have the same name as the one returned from the Form property in WarningActivity.
            context.Form(WarningActivity.FormName,
                shape => _shapeFactory.Form(
                Id: WarningActivity.FormName,
                // Creating a textbox for configuring the message. You could also add custom ad-hoc shapes here.
                _Message: _shapeFactory.Textbox(
                    Id: WarningActivity.MessageFieldName,
                    Name: WarningActivity.MessageFieldName,
                    Title: T("Message"),
                    Description: T("The warning message to display."),
                    Classes: new[] { "text medium", "tokenized" })
                )
            );


            // NEXT STATION: we'll dive into unit testing! This module's folder contains a test suite: it's a project 
            // called OrchardHUN.TrainingDemo.Tests in the folder named the same. If you haven't already add it to the 
            // Test solution folder and open it up! You'll be surprised but it contains a StartHere.txt. Don't start 
            // there! And by that I mean do start there!
        }
    }
}