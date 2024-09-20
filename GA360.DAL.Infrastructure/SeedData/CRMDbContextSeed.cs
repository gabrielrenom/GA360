using CRM.Entities;
using CRM.Entities.Entities;
using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static GA360.DAL.Entities.Enums.StatusEnum;

namespace GA360.DAL.Infrastructure.SeedData
{
    public static class CRMDbContextSeed
    {
        public static void Initialize(CRMDbContext context)
        {

            if (!context.EthnicOrigins.Any())
            {
                var ethnicOrigins = new List<EthnicOrigin>
                {
                    new EthnicOrigin { Name = "Asian" },
                    new EthnicOrigin { Name = "Black" },
                    new EthnicOrigin { Name = "Hispanic" },
                    new EthnicOrigin { Name = "White" },
                    new EthnicOrigin { Name = "Mixed" },
                    new EthnicOrigin { Name = "Other" }
                };

                context.EthnicOrigins.AddRange(ethnicOrigins);
                context.SaveChanges();
            }


            if (!context.TrainingCentres.Any() || !context.Addresses.Any())
            {
                var manchesterAddress = new Address { City = "Manchester", Postcode = "M13 9PL", Street = "Oxford Road", Number = "176" };
                var salfordAddress = new Address { City = "Salford", Postcode = "M5 4WT", Street = "The Crescent", Number = "43" };
                var imperialCollegeAddress = new Address { City = "London", Postcode = "SW7 2AZ", Street = "Exhibition Road", Number = "1" };

                context.Addresses.AddRange(manchesterAddress, salfordAddress, imperialCollegeAddress);
                context.SaveChanges();

                context.TrainingCentres.AddRange(
                    new TrainingCentre { AddressId = manchesterAddress.Id, Name = "Imperial College" },
                    new TrainingCentre { AddressId = salfordAddress.Id, Name = "Manchester University" },
                    new TrainingCentre { AddressId = imperialCollegeAddress.Id, Name = "Imperial College" }
                );
                context.SaveChanges();
            }


            // Check if the Countries table has any data
            if (!context.Countries.Any())
            {

                context.Countries.AddRange(
  new Country { Code = "AD", Name = "Andorra", Prefix = "+376" },
  new Country
  {
      Code = "AE",
      Name = "United Arab Emirates",
      Prefix = "+971"
  },
  new Country { Code = "AF", Name = "Afghanistan", Prefix = "+93" },
  new Country
  {
      Code = "AG",
      Name = "Antigua and Barbuda",
      Prefix = "+1-268"
  },
  new Country { Code = "AI", Name = "Anguilla", Prefix = "+1-264" },
  new Country { Code = "AL", Name = "Albania", Prefix = "+355" },
  new Country { Code = "AM", Name = "Armenia", Prefix = "+374" },
  new Country { Code = "AO", Name = "Angola", Prefix = "+244" },
  new Country { Code = "AQ", Name = "Antarctica", Prefix = "+672" },
  new Country { Code = "AR", Name = "Argentina", Prefix = "+54" },
  new Country { Code = "AS", Name = "American Samoa", Prefix = "+1-684" },
  new Country { Code = "AT", Name = "Austria", Prefix = "+43" },
  new Country
  {
      Code = "AU",
      Name = "Australia",
      Prefix = "+61",
  },
  new Country { Code = "AW", Name = "Aruba", Prefix = "+297" },
  new Country { Code = "AZ", Name = "Azerbaijan", Prefix = "+994" },
  new Country
  {
      Code = "BA",
      Name = "Bosnia and Herzegovina",
      Prefix = "+387"
  },
  new Country { Code = "BB", Name = "Barbados", Prefix = "+1-246" },
  new Country { Code = "BD", Name = "Bangladesh", Prefix = "+880" },
  new Country { Code = "BE", Name = "Belgium", Prefix = "+32" },
  new Country { Code = "BF", Name = "Burkina Faso", Prefix = "+226" },
  new Country { Code = "BG", Name = "Bulgaria", Prefix = "+359" },
  new Country { Code = "BH", Name = "Bahrain", Prefix = "+973" },
  new Country { Code = "BI", Name = "Burundi", Prefix = "+257" },
  new Country { Code = "BJ", Name = "Benin", Prefix = "+229" },
  new Country { Code = "BL", Name = "Saint Barthelemy", Prefix = "+590" },
  new Country { Code = "BM", Name = "Bermuda", Prefix = "+1-441" },
  new Country { Code = "BN", Name = "Brunei Darussalam", Prefix = "+673" },
  new Country { Code = "BO", Name = "Bolivia", Prefix = "+591" },
  new Country { Code = "BR", Name = "Brazil", Prefix = "+55" },
  new Country { Code = "BS", Name = "Bahamas", Prefix = "+1-242" },
  new Country { Code = "BT", Name = "Bhutan", Prefix = "+975" },
  new Country { Code = "BW", Name = "Botswana", Prefix = "+267" },
  new Country { Code = "BY", Name = "Belarus", Prefix = "+375" },
  new Country { Code = "BZ", Name = "Belize", Prefix = "+501" },
  new Country
  {
      Code = "CD",
      Name = "Congo, Democratic Republic of the",
      Prefix = "+243"
  },
  new Country
  {
      Code = "CF",
      Name = "Central African Republic",
      Prefix = "+236"
  },
  new Country
  {
      Code = "CG",
      Name = "Congo, Republic of the",
      Prefix = "+242"
  },
  new Country { Code = "CH", Name = "Switzerland", Prefix = "+41" },
  new Country
  {
      Code = "CI",
      Name = "Cote d'Ivoire",
      Prefix = " + 225"
  },
  new Country { Code = "CK", Name = "Cook Islands", Prefix = "+682" },
      new Country { Code = "CL", Name = "Chile", Prefix = "+56" },
      new Country { Code = "CM", Name = "Cameroon", Prefix = "+237" },
      new Country { Code = "CN", Name = "China", Prefix = "+86" },
      new Country { Code = "CO", Name = "Colombia", Prefix = "+57" },
      new Country { Code = "CR", Name = "Costa Rica", Prefix = "+506" },
      new Country { Code = "CU", Name = "Cuba", Prefix = "+53" },
      new Country { Code = "CV", Name = "Cape Verde", Prefix = "+238" },
      new Country { Code = "CW", Name = "Curacao", Prefix = "+599" },
      new Country { Code = "CY", Name = "Cyprus", Prefix = "+357" },
      new Country { Code = "CZ", Name = "Czech Republic", Prefix = "+420" },
      new Country
      {
          Code = "DE",
          Name = "Germany",
          Prefix = "+49",
      },
      new Country { Code = "DJ", Name = "Djibouti", Prefix = "+253" },
      new Country { Code = "DK", Name = "Denmark", Prefix = "+45" },
      new Country { Code = "DM", Name = "Dominica", Prefix = "+1-767" },
      new Country
      {
          Code = "DO",
          Name = "Dominican Republic",
          Prefix = "+1-809"
      },
      new Country { Code = "DZ", Name = "Algeria", Prefix = "+213" },
      new Country { Code = "EC", Name = "Ecuador", Prefix = "+593" },
      new Country { Code = "EE", Name = "Estonia", Prefix = "+372" },
      new Country { Code = "EG", Name = "Egypt", Prefix = "+20" },
      new Country { Code = "ER", Name = "Eritrea", Prefix = "+291" },
      new Country { Code = "ES", Name = "Spain", Prefix = "+34" },
      new Country { Code = "ET", Name = "Ethiopia", Prefix = "+251" },
      new Country { Code = "FI", Name = "Finland", Prefix = "+358" },
      new Country { Code = "FJ", Name = "Fiji", Prefix = "+679" },
      new Country
      {
          Code = "FK",
          Name = "Falkland Islands (Malvinas)",
          Prefix = "+500"
      },
      new Country
      {
          Code = "FM",
          Name = "Micronesia, Federated States of",
          Prefix = "+691"
      },
      new Country { Code = "FO", Name = "Faroe Islands", Prefix = "+298" },
      new Country
      {
          Code = "FR",
          Name = "France",
          Prefix = "+33",
      },
      new Country { Code = "GA", Name = "Gabon", Prefix = "+241" },
      new Country { Code = "GB", Name = "United Kingdom", Prefix = "+44" },
      new Country { Code = "GD", Name = "Grenada", Prefix = "+1-473" },
      new Country { Code = "GE", Name = "Georgia", Prefix = "+995" },
      new Country { Code = "GF", Name = "French Guiana", Prefix = "+594" },
      new Country { Code = "GH", Name = "Ghana", Prefix = "+233" },
      new Country { Code = "GI", Name = "Gibraltar", Prefix = "+350" },
      new Country { Code = "GL", Name = "Greenland", Prefix = "+299" },
      new Country { Code = "GM", Name = "Gambia", Prefix = "+220" },
      new Country { Code = "GN", Name = "Guinea", Prefix = "+224" },
      new Country { Code = "GQ", Name = "Equatorial Guinea", Prefix = "+240" },
      new Country { Code = "GR", Name = "Greece", Prefix = "+30" },
      new Country { Code = "GT", Name = "Guatemala", Prefix = "+502" },
      new Country { Code = "GU", Name = "Guam", Prefix = "+1-671" },
      new Country { Code = "GW", Name = "Guinea-Bissau", Prefix = "+245" },
      new Country { Code = "GY", Name = "Guyana", Prefix = "+592" },
      new Country { Code = "HK", Name = "Hong Kong", Prefix = "+852" },
      new Country { Code = "HN", Name = "Honduras", Prefix = "+504" },
      new Country { Code = "HR", Name = "Croatia", Prefix = "+385" },
      new Country { Code = "HT", Name = "Haiti", Prefix = "+509" },
      new Country { Code = "HU", Name = "Hungary", Prefix = "3+6" },
      new Country { Code = "ID", Name = "Indonesia", Prefix = "+62" },
      new Country { Code = "IE", Name = "Ireland", Prefix = "+353" },
      new Country { Code = "IL", Name = "Israel", Prefix = "+972" },
      new Country { Code = "IN", Name = "India", Prefix = "+91" },
      new Country
      {
          Code = "IO",
          Name = "British Indian Ocean Territory",
          Prefix = "+246"
      },
      new Country { Code = "IQ", Name = "Iraq", Prefix = "+964" },
      new Country
      {
          Code = "IR",
          Name = "Iran, Islamic Republic of",
          Prefix = "+98"
      },
      new Country { Code = "IS", Name = "Iceland", Prefix = "+354" },
      new Country { Code = "IT", Name = "Italy", Prefix = "+39" },
      new Country { Code = "JM", Name = "Jamaica", Prefix = "+1-876" },
      new Country { Code = "JO", Name = "Jordan", Prefix = "+962" },
      new Country
      {
          Code = "JP",
          Name = "Japan",
          Prefix = "+81",
      },
      new Country { Code = "KE", Name = "Kenya", Prefix = "+254" },
      new Country { Code = "KG", Name = "Kyrgyzstan", Prefix = "+996" },
      new Country { Code = "KH", Name = "Cambodia", Prefix = "+855" },
      new Country { Code = "KI", Name = "Kiribati", Prefix = "+686" },
      new Country { Code = "KM", Name = "Comoros", Prefix = "+269" },
      new Country
      {
          Code = "KN",
          Name = "Saint Kitts and Nevis",
          Prefix = "+1-869"
      },
      new Country
      {
          Code = "KP",
          Name = "Korea, Democratic People's Republic of",
          Prefix = "+850"
      },
      new Country { Code = "KR", Name = "Korea, Republic of", Prefix = "+82" },
      new Country { Code = "KW", Name = "Kuwait", Prefix = "+965" },
      new Country { Code = "KY", Name = "Cayman Islands", Prefix = "+1-345" },
      new Country
      {
          Code = "LA",
          Name = "Lao People's Democratic Republic",
          Prefix = "+856"
      },
      new Country { Code = "LB", Name = "Lebanon", Prefix = "+961" },
      new Country { Code = "LC", Name = "Saint Lucia", Prefix = "+1-758" },
      new Country { Code = "LI", Name = "Liechtenstein", Prefix = "+423" },
      new Country { Code = "LK", Name = "Sri Lanka", Prefix = "+94" },
      new Country { Code = "LR", Name = "Liberia", Prefix = "+231" },
      new Country { Code = "LS", Name = "Lesotho", Prefix = "+266" },
      new Country { Code = "LT", Name = "Lithuania", Prefix = "+370" },
      new Country { Code = "LU", Name = "Luxembourg", Prefix = "+352" },
      new Country { Code = "LV", Name = "Latvia", Prefix = "+371" },
      new Country { Code = "LY", Name = "Libya", Prefix = "+218" },
      new Country { Code = "MA", Name = "Morocco", Prefix = "+212" },
      new Country { Code = "MC", Name = "Monaco", Prefix = "+377" },
      new Country
      {
          Code = "MD",
          Name = "Moldova, Republic of",
          Prefix = "+373"
      },
      new Country { Code = "ME", Name = "Montenegro", Prefix = "+382" },
      new Country { Code = "MG", Name = "Madagascar", Prefix = "+261" },
      new Country { Code = "MH", Name = "Marshall Islands", Prefix = "+692" },
      new Country
      {
          Code = "MK",
          Name = "Macedonia, the Former Yugoslav Republic of",
          Prefix = "+389"
      },
      new Country { Code = "ML", Name = "Mali", Prefix = "+223" },
      new Country { Code = "MM", Name = "Myanmar", Prefix = "+95" },
      new Country { Code = "MN", Name = "Mongolia", Prefix = "+976" },
      new Country { Code = "MO", Name = "Macao", Prefix = "+853" },
      new Country
      {
          Code = "MP",
          Name = "Northern Mariana Islands",
          Prefix = "+1-670"
      },
      new Country { Code = "MQ", Name = "Martinique", Prefix = "+596" },
      new Country { Code = "MR", Name = "Mauritania", Prefix = "+222" },
      new Country { Code = "MS", Name = "Montserrat", Prefix = "+1-664" },
      new Country { Code = "MT", Name = "Malta", Prefix = "+356" },
      new Country { Code = "MU", Name = "Mauritius", Prefix = "+230" },
      new Country { Code = "MV", Name = "Maldives", Prefix = "+960" },
      new Country { Code = "MW", Name = "Malawi", Prefix = "+265" },
      new Country { Code = "MX", Name = "Mexico", Prefix = "+52" },
      new Country { Code = "MY", Name = "Malaysia", Prefix = "+60" },
      new Country { Code = "MZ", Name = "Mozambique", Prefix = "+258" },
      new Country { Code = "NA", Name = "Namibia", Prefix = "+264" },
      new Country { Code = "NC", Name = "New Caledonia", Prefix = "+687" },
      new Country { Code = "NE", Name = "Niger", Prefix = "+227" },
      new Country { Code = "NG", Name = "Nigeria", Prefix = "+234" },
      new Country { Code = "NI", Name = "Nicaragua", Prefix = "+505" },
      new Country { Code = "NL", Name = "Netherlands", Prefix = "+31" },
      new Country { Code = "NO", Name = "Norway", Prefix = "+47" },
      new Country { Code = "NP", Name = "Nepal", Prefix = "+977" },
      new Country { Code = "NR", Name = "Nauru", Prefix = "+674" },
      new Country { Code = "NU", Name = "Niue", Prefix = "+683" },
      new Country { Code = "NZ", Name = "New Zealand", Prefix = "+64" },
      new Country { Code = "OM", Name = "Oman", Prefix = "+968" },
      new Country { Code = "PA", Name = "Panama", Prefix = "+507" },
      new Country { Code = "PE", Name = "Peru", Prefix = "+51" },
      new Country { Code = "PF", Name = "French Polynesia", Prefix = "+689" },
      new Country { Code = "PG", Name = "Papua New Guinea", Prefix = "+675" },
      new Country { Code = "PH", Name = "Philippines", Prefix = "+63" },
      new Country { Code = "PK", Name = "Pakistan", Prefix = "+92" },
      new Country { Code = "PL", Name = "Poland", Prefix = "+48" },
      new Country
      {
          Code = "PM",
          Name = "Saint Pierre and Miquelon",
          Prefix = "+508"
      },
      new Country { Code = "PN", Name = "Pitcairn", Prefix = "+870" },
      new Country
      {
          Code = "PS",
          Name = "Palestine, State of",
          Prefix = "+970"
      },
      new Country { Code = "PT", Name = "Portugal", Prefix = "+351" },
      new Country { Code = "PW", Name = "Palau", Prefix = "+680" },
      new Country { Code = "PY", Name = "Paraguay", Prefix = "+595" },
      new Country { Code = "QA", Name = "Qatar", Prefix = "+974" },
      new Country { Code = "RO", Name = "Romania", Prefix = "+40" },
      new Country { Code = "RS", Name = "Serbia", Prefix = "+381" },
      new Country { Code = "RU", Name = "Russian Federation", Prefix = "+7" },
      new Country { Code = "RW", Name = "Rwanda", Prefix = "+250" },
      new Country { Code = "SA", Name = "Saudi Arabia", Prefix = "+966" },
      new Country { Code = "SB", Name = "Solomon Islands", Prefix = "+677" },
      new Country { Code = "SC", Name = "Seychelles", Prefix = "+248" },
      new Country { Code = "SD", Name = "Sudan", Prefix = "+249" },
      new Country { Code = "SE", Name = "Sweden", Prefix = "+46" },
      new Country { Code = "SG", Name = "Singapore", Prefix = "+65" },
      new Country { Code = "SH", Name = "Saint Helena", Prefix = "+290" },
      new Country { Code = "SI", Name = "Slovenia", Prefix = "+386" },
      new Country { Code = "SK", Name = "Slovakia", Prefix = "+421" },
      new Country { Code = "SL", Name = "Sierra Leone", Prefix = "+232" },
      new Country { Code = "SM", Name = "San Marino", Prefix = "+378" },
      new Country { Code = "SN", Name = "Senegal", Prefix = "+221" },
      new Country { Code = "SO", Name = "Somalia", Prefix = "+252" },
      new Country { Code = "SR", Name = "Suriname", Prefix = "+597" },
      new Country { Code = "SS", Name = "South Sudan", Prefix = "+211" },
      new Country
      {
          Code = "ST",
          Name = "Sao Tome and Principe",
          Prefix = "+239"
      },
      new Country { Code = "SV", Name = "El Salvador", Prefix = "+503" },
      new Country
      {
          Code = "SX",
          Name = "Sint Maarten (Dutch part)",
          Prefix = "+1-721"
      },
      new Country
      {
          Code = "SY",
          Name = "Syrian Arab Republic",
          Prefix = "+963"
      },
      new Country { Code = "SZ", Name = "Swaziland", Prefix = "+268" },
      new Country
      {
          Code = "TC",
          Name = "Turks and Caicos Islands",
          Prefix = "+1-649"
      },
      new Country { Code = "TD", Name = "Chad", Prefix = "+235" },
      new Country { Code = "TG", Name = "Togo", Prefix = "+228" },
      new Country { Code = "TH", Name = "Thailand", Prefix = "+66" },
      new Country { Code = "TJ", Name = "Tajikistan", Prefix = "+992" },
      new Country { Code = "TK", Name = "Tokelau", Prefix = "+690" },
      new Country { Code = "TL", Name = "Timor-Leste", Prefix = "+670" },
      new Country { Code = "TM", Name = "Turkmenistan", Prefix = "+993" },
      new Country { Code = "TN", Name = "Tunisia", Prefix = "+216" },
      new Country { Code = "TO", Name = "Tonga", Prefix = "+676" },
      new Country { Code = "TR", Name = "Turkey", Prefix = "+90" },
      new Country
      {
          Code = "TT",
          Name = "Trinidad and Tobago",
          Prefix = "+1-868"
      },
      new Country { Code = "TV", Name = "Tuvalu", Prefix = "+688" },
      new Country
      {
          Code = "TW",
          Name = "Taiwan, Province of China",
          Prefix = "+886"
      },
      new Country
      {
          Code = "TZ",
          Name = "United Republic of Tanzania",
          Prefix = "+255"
      },
      new Country { Code = "UA", Name = "Ukraine", Prefix = "+380" },
      new Country { Code = "UG", Name = "Uganda", Prefix = "+256" },
      new Country
      {
          Code = "US",
          Name = "United States",
          Prefix = "+1",
      },
      new Country { Code = "UY", Name = "Uruguay", Prefix = "+598" },
      new Country { Code = "UZ", Name = "Uzbekistan", Prefix = "+998" },
      new Country
      {
          Code = "VA",
          Name = "Holy See (Vatican City State)",
          Prefix = "+379"
      },
      new Country
      {
          Code = "VC",
          Name = "Saint Vincent and the Grenadines",
          Prefix = "+1-784"
      },
      new Country { Code = "VE", Name = "Venezuela", Prefix = "+58" },
      new Country
      {
          Code = "VG",
          Name = "British Virgin Islands",
          Prefix = "+1-284"
      },
      new Country
      {
          Code = "VI",
          Name = "US Virgin Islands",
          Prefix = "+1-340"
      },
      new Country { Code = "VN", Name = "Vietnam", Prefix = "+84" },
      new Country { Code = "VU", Name = "Vanuatu", Prefix = "+678" },
      new Country { Code = "WF", Name = "Wallis and Futuna", Prefix = "+681" },
      new Country { Code = "WS", Name = "Samoa", Prefix = "+685" },
      new Country { Code = "XK", Name = "Kosovo", Prefix = "+383" },
      new Country { Code = "YE", Name = "Yemen", Prefix = "+967" },
      new Country { Code = "YT", Name = "Mayotte", Prefix = "+262" },
      new Country { Code = "ZA", Name = "South Africa", Prefix = "+27" },
      new Country { Code = "ZM", Name = "Zambia", Prefix = "+260" },
      new Country { Code = "ZW", Name = "Zimbabwe", Prefix = "+263" });
                context.SaveChanges();

            }
            var uk = context.Countries.FirstOrDefault(x => x.Code.ToLower() == "gb");
            var us = context.Countries.FirstOrDefault(x => x.Code.ToLower() == "us");
            var thailand = context.Countries.FirstOrDefault(x => x.Code.ToLower() == "th");

            if (!context.Skills.Any())
            {
                context.Skills.AddRange(
                new Skill
                {
                    IsDeleted = false,
                    Name = "Languages",
                    Description = "Languages",

                },
                new Skill
                {
                    IsDeleted = false,
                    Name = "Software",
                    Description = "Software",
                }
                );

                context.SaveChanges();
            }


            // Check if the Clients table has any data
            var manchesteruni = context.Addresses.FirstOrDefault();

            if (!context.Clients.Any())
            {
                context.Clients.AddRange(
                    new Client
                    {
                        Name = "Global Alliance 360",
                        Description = "Global Alliance 360",
                        CountryId = uk.Id,
                        ParentClientId = null,
                        IsDeleted = false,
                        Mail = "gabrielrenom@gmail.com",
                        Phone = "07855185075",
                        Type = Entities.Enums.StatusEnum.ClientType.Partner,
                        Website = "https://globalalliance360.com",
                        AddressId = manchesteruni.Id,

                    }
                );
                context.SaveChanges();
            }
            var ethnic = context.EthnicOrigins.FirstOrDefault();

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                        new Customer
                        {
                            FirstName = "Phoebe",
                            LastName = "Venturi",
                            Gender = "Female",
                            Contact = "(887) 744-6950",
                            Email = "ke@gmail.com",
                            Country = new Country { Name = "Thailand" },
                            Location = "1804 Ahedi Trail, Owottug, Bolivia - 47403",
                            FatherName = "Helen Stewart",
                            Role = "Logistics Manager",
                            About = "Udukaape gozune jig fu foslinan tadka kumu no guw upe or cifdasbej di ige.",
                            Status = Status.ProjectWin,
                            CountryId = thailand.Id,
                            AddressId = manchesteruni.Id,
                            NI = "AB 12 34 56 C",
                            DOB = "1/4/2000",
                            Employer = "Microsoft",
                            Disability = "No",
                            ePortfolio = "Renewal",
                            EthnicOriginId = ethnic.Id,
                            EmploymentStatus = "Employed"

                        },

                        new Customer
                        {
                            FirstName = "John",
                            LastName = "Doe",
                            Gender = "Male",
                            Contact = "(123) 456-7890",
                            Email = "john.doe@example.com",
                            Country = new Country { Name = "USA" },
                            Location = "123 Main St, Springfield, USA - 12345",
                            FatherName = "Robert Doe",
                            Role = "Software Engineer",
                            About = "Passionate about coding and technology.",
                            Status = Status.ProjectFail,
                            CountryId = us.Id,
                            AddressId = manchesteruni.Id,
                            NI = "AB 12 34 56 A",
                            DOB = "1/3/2000",
                            Employer = "Coca Cola",
                            Disability = "No",
                            ePortfolio = "Original",
                            EthnicOriginId = ethnic.Id,
                            EmploymentStatus = "Employed"
                        },
    new Customer
    {
        FirstName = "Jane",
        LastName = "Smith",
        Gender = "Female",
        Contact = "(987) 654-3210",
        Email = "jane.smith@example.com",
        Country = new Country { Name = "Canada" },
        Location = "456 Elm St, Toronto, Canada - 67890",
        FatherName = "Michael Smith",
        Role = "Product Manager",
        About = "Experienced in managing product lifecycles.",
        Status = Status.RequestCome,
        CountryId = us.Id,
        AddressId = manchesteruni.Id,
        NI = "AB 12 34 56 A",
        DOB = "1/1/2000",
        Employer = "Amazon",
        Disability = " A disability is a physical or mental condition that significantly limits a person’s abilities to perform certain activities or interact with the world around them. Disabilities can be visible or invisible, temporary or permanent, and can affect people in various ways.",
        ePortfolio = "Renewal",
        EthnicOriginId = ethnic.Id,
        EmploymentStatus = "Self Employed"

    }
    );

                context.SaveChanges();

            }


            if (!context.ClientContacts.Any())
            {
                var contacts = context.Customers.ToList();
                var client = context.Clients.FirstOrDefault();
                foreach (var contact in contacts)
                {
                    context.ClientContacts.Add(new ClientCustomer { ClientId = client.Id, CustomerId = contact.Id });
                }
                context.SaveChanges();
            }

            if (!context.CustomerSkills.Any())
            {
                var skills = context.Skills.ToList();
                var contacts = context.Customers.ToList();

                foreach (var customer in contacts)
                {
                    context.CustomerSkills.Add(new CustomerSkills
                    {
                        SkillId = skills.FirstOrDefault().Id,
                        CustomerId = customer.Id
                    });

                    context.CustomerSkills.Add(new CustomerSkills
                    {
                        SkillId = skills.LastOrDefault().Id,
                        CustomerId = customer.Id
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
