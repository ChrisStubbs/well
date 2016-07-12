namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable()]
    public class Account : Entity<int>
        {

        public Account()
        {
            this.EntityAttributes = new Collection<Attribute>();
        }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("AccountTypeCode")]
        public string AccountTypeCode { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }


        [XmlElement("Address1")]
        public string Address1 { get; set; }

        [XmlElement("Address2")]
        public string Address2 { get; set; }

        [XmlElement("PostCode")]
        public string PostCode { get; set; }

        [XmlElement("ContactName")]
        public string ContactName { get; set; }

        [XmlElement("ContactNumber")]
        public string ContactNumber { get; set; }

        [XmlElement("ContactNumber2")]
        public string ContactNumber2 { get; set; }

        [XmlElement("ContactEmailAddress")]
        public string ContactEmailAddress { get; set; }

        [XmlElement("StartWindow")]
        public string StartWindow { get; set; }

        [XmlElement("EndWindow")]
        public string EndWindow { get; set; }

        [XmlElement("Latitude")]
        public double Latitude { get; set; }

        [XmlElement("Longitude")]
        public double Longitude { get; set; }

        [XmlArray("EntityAttributes")]
        [XmlArrayItem("EntityAttribute", typeof(Attribute))]
        public Collection<Attribute> EntityAttributes { get; set; }

        [XmlElement("IsDropAndDrive")]
        public string IsDropAndDrive { get; set; }

        [XmlIgnore]
        public int StopId { get; set; }
        }
    }

