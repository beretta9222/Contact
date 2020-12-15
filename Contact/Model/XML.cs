using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Contact.Model
{
    public static class XML
    {
        /// <summary>
        /// Get all saved contacts 
        /// </summary>
        /// <returns>Contacts collection</returns>
        public static ObservableCollection<Contact> GetContacts()
        {
            ObservableCollection<Contact> list = new ObservableCollection<Contact>();

            try
            {
                XDocument doc = XDocument.Load("Contacts.xml");
                foreach (var item in doc.Element("contacts").Elements("contact"))
                {
                    Contact contact = new Contact();
                    contact.ID = int.Parse(item.Element("id").Value);
                    contact.Name = item.Element("fio").Value;
                    contact.Phone = item.Element("phone").Value;
                    list.Add(contact);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return list;
        }

        public static bool SaveContact(ObservableCollection<Contact> contacts)
        {
            bool result = false;

            try
            {
                XDocument doc = new XDocument();
                var writer = doc.CreateWriter();
                writer.WriteStartElement("contacts");
                foreach (var item in contacts)
                {
                    writer.WriteStartElement("contact");
                    writer.WriteElementString("id", item.ID.ToString());
                    writer.WriteElementString("fio", item.Name.ToString());
                    writer.WriteElementString("phone", item.Phone.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
                doc.Save("Contacts.xml");
                writer.Flush();
                writer.Dispose();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }
    }
}