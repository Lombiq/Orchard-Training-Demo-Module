using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Localization;
using Orchard.Services;
using Orchard.Tokens;

namespace OrchardHUN.TrainingDemo.Tokens
{
    // A simple example of creating a custom token provider. For the sake of simplicity this token will only display 
    // the current UTC date time. However do note that a lot of DateTime-related tokens are built into Orchard OOTB
    // in Orchard.Tokens.Providers.DateTokens.
    public class UtcNowTokens : ITokenProvider
    {
        private readonly IClock _clock;

        public Localizer T { get; set; }


        public UtcNowTokens(IClock clock)
        {
            _clock = clock;

            T = NullLocalizer.Instance;
        }


        // First tokens need to be "described". This is the definition phase, the texts here will be shown to the user
        // from the token helper.
        public void Describe(DescribeContext context)
        {
            // Adding a token for the "UtcNow" target. I.e. every token will be in the {UtcNow.Something} format.
            context.For("UtcNow", T("Current UTC Date/Time"), T("Current UTC Date/time tokens"))
                // Adding a token called "Default", i.e. it will be usable as {UtcNow.Default}. The last parameter 
                // allows the output of the token to be processed by tokens defined for the "Text" target, i.e. by tokens
                // in the {Text.Something} format (like {UtcNow.Default.Limit:5}, see TextTokens in Orchard.Tokens).
                .Token("Default", T("Default"), T("The current date/time in UTC, with the default formatting."), "Text");
        }

        // Defining how tokens are executed, so here are the actual values produced.
        public void Evaluate(EvaluateContext context)
        {
            // The second parameter provides a default value which will be used if the token is used on its own, not
            // chained onto another token, like just {UtcNow.Default}. But if the tokens would be chained onto another
            // token that a) allows chaining for UtcNow (see Describe() above) and b) returns a DateTime object then
            // they'd instead use the output of that token as the input.
            context.For("UtcNow", () => _clock.UtcNow)
                // Producing the actual value when the token is not chained.
                .Token("Default", dateTime => dateTime.ToString())
                // Producing the output if Text tokens are chained onto this token.
                .Chain("Default", "Text", dateTime => dateTime.ToString());


            // NEXT STATION: see how Workflows can be extended in WarningActivity!
        }
    }
}