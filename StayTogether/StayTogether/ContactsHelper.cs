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
            if (await CrossContacts.Current.RequestPermission())
            {

                var contacts = new List<ContactSynopsis>();
                CrossContacts.Current.PreferContactAggregation = false;//recommended
                                                                       //run in background
                //await Task.Run(() =>
                {
                    if (CrossContacts.Current.Contacts == null)
                        return null;

                    //for some reason we can't use linq
                    foreach (var contact in CrossContacts.Current.Contacts)
                    {
                        var cleanedName = CleanName(contact);
                        if (!string.IsNullOrWhiteSpace(cleanedName))
                        {
                            foreach (var phone in contact.Phones)
                            {
                                var cleanedPhone = CleanPhoneNumber(phone.Number);
                                if (phone.Type == PhoneType.Mobile && !string.IsNullOrWhiteSpace(cleanedPhone))
                                {
                                    //should be able to create a new class here and add  it to the list
                                    var contactSynopsis = new ContactSynopsis
                                    {
                                        Name = cleanedName,
                                        PhoneNumber = cleanedPhone
                                    };
                                    contacts.Add(contactSynopsis);
                                }
                            }
                        }
                    }


                    var sortedcontacts = contacts.OrderBy(c => c.Name).ToList();
                    contacts = sortedcontacts;
                    return contacts;
                }//);
            }
            return null;
        }

        private string CleanName(Contact contact)
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


        private string CleanPhoneNumber(string number)
        {
            var cleanNumber =  number.Where(char.IsDigit).Aggregate("", (current, character) => current + character);
            return cleanNumber.Length >= 10 ? cleanNumber.Substring(cleanNumber.Length - 10) : "";
        }
    }
}