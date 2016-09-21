using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Contacts;
using Plugin.Contacts.Abstractions;
using StayTogether.Classes;


namespace StayTogether
{
    public class ContactsHelper
    {

        public async Task<List<ContactSynopsis>> GetContacts()
        {
            if (!await CrossContacts.Current.RequestPermission()) return null;

            var contacts = new List<ContactSynopsis>();
            CrossContacts.Current.PreferContactAggregation = false;
            
            if (CrossContacts.Current.Contacts == null)
                return null;

            //for some reason we can't use linq
            foreach (var contact in CrossContacts.Current.Contacts)
            {
                var cleanedName = CleanName(contact);
                if (string.IsNullOrWhiteSpace(cleanedName)) continue;
                foreach (var phone in contact.Phones)
                {
                    var cleanedPhone = CleanPhoneNumber(phone.Number);
                    if (phone.Type != PhoneType.Mobile || string.IsNullOrWhiteSpace(cleanedPhone)) continue;

                    var contactSynopsis = new ContactSynopsis
                    {
                        Name = cleanedName,
                        PhoneNumber = cleanedPhone
                    };
                    contacts.Add(contactSynopsis);
                }
            }

            var sortedcontacts = contacts.OrderBy(c => c.Name).ToList();
            contacts = sortedcontacts;
            return contacts;           
        }

        private static string CleanName(Contact contact)
        {
            var cleanedName = "";
            if (!string.IsNullOrWhiteSpace(contact.LastName))
            {
                cleanedName = contact.LastName;
            }
            if (string.IsNullOrWhiteSpace(contact.FirstName)) return cleanedName;

            if (!string.IsNullOrWhiteSpace(contact.LastName))
            {
                cleanedName += ", ";
            }
            cleanedName += contact.FirstName;

            return cleanedName;
           
        }


        private static string CleanPhoneNumber(string number)
        {
            var cleanNumber =  number.Where(char.IsDigit).Aggregate("", (current, character) => current + character);
            return cleanNumber.Length >= 10 ? cleanNumber.Substring(cleanNumber.Length - 10) : "";
        }
    }
}