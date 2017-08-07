using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountyLocator.Model;

namespace CountyLocator.ViewModel
{
    public class CountyLocatorViewModel
    {
        // Create a property for the Model that the ViewModel can access
        public CountyModel County { get; set; }

        // Constructor
        public CountyLocatorViewModel()
        {
            County = new CountyModel();
        }

        // Give the ViewModel the means to access the method within the Model
        public void LookupCounty()
        {
            County.LookupCounty();
        }
    }
}
