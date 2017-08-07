using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace CountyLocator.Model
{
    public class CountyModel : INotifyPropertyChanged
    {
        private string _cityState;
        private string _county;
        private readonly string requestUri = "https://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address=";
        private readonly string apiKey = Properties.Resources.APIKey;

        /// <summary>
        /// Getter/Setter combo for the City and State
        /// </summary>
        public string CityState
        {
            get { return _cityState; }
            set
            {
                _cityState = value;
                OnPropertyChanged(nameof(CityState));
            }
        }

        /// <summary>
        /// Getter/Setter combo for the returned County
        /// </summary>
        public string County
        {
            get { return _county; }
            set
            {
                _county = value;
                OnPropertyChanged(nameof(County));
            }
        }

        /// <summary>
        /// Based on user input, looks up the county for a user-input city/state combination
        /// </summary>
        public void LookupCounty()
        {
            // Build the request URL
            string request = $"{requestUri}{CityState}{apiKey}";
            // Instantiate a new web request and use the response from it
            WebRequest googleRequest = WebRequest.Create(request);
            
            using (WebResponse googleResponse = googleRequest.GetResponse())
            {
                // Load the xml returned from the web request into memory
                using (Stream xmlStream = googleResponse.GetResponseStream())
                {
                    var xmlDoc = XDocument.Load(xmlStream);
                    /*
                     * Perform the following LINQ query:
                     *  from the elements in the XML under the tags GeocodeResponse -> result -> address_component
                     *  select the "long_name" element from address_component
                     *  where the element "type" in address_component is the verbose term for county
                     */
                    var result =
                        xmlDoc.Element("GeocodeResponse").Element("result").Elements("address_component")
                        .Where(node => (string)node.Element("type") == "administrative_area_level_2")
                        // Select the text between the XML tags
                        .Select(node => node.Element("long_name").Value); 

                    // Use LINQ to assign the county to the object
                    County = result.FirstOrDefault();
                }
            }
        }

        // Implementation of NotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
    }
}
