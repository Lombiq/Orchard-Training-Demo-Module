using System;
using System.Collections.Generic;
using System.Text;

namespace OrchardHUN.TrainingDemo.Models
{
    public class PersonWithAddresses
    {
        public PersonPart PersonPart { get; set; }

        public AddressPart WorkAddress { get; set; }

        public AddressPart HomeAddress { get; set; }

    }
}
