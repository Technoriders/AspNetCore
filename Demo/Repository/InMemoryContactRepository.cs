using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Models;

namespace AspNetCore.Repository
{
    public class InMemoryContactRepository
    {
        private readonly List<Contact> contacts_ = new List<Contact>
        {
            new Contact { ContactId = 1, Name = "Alishia Sergi", Address = "2742 Distribution Way", City = "New York" },
            new Contact { ContactId = 2, Name = "Kiley Caldarera", Address = "25 E 75th St #69", City = "Los Angeles" },
            new Contact { ContactId = 3, Name = "Lavera Perin", Address = "678 3rd Ave", City = "Miami" },
            new Contact { ContactId = 4, Name = "Jose Bautista", Address = "98 Connecticut Ave Nw", City = "Chagrin Falls" },
            new Contact { ContactId = 5, Name = "Mitsue Tollner", Address = "7 Eads St", City = "Chicago" }
        };

        public Task<IEnumerable<Contact>> GetAll()
        {
            return Task.FromResult(contacts_.AsEnumerable());
        }

        public Task<Contact> Get(int id)
        {
            return Task.FromResult(contacts_.FirstOrDefault(x => x.ContactId == id));
        }

        public Task<int> Add(Contact contact)
        {
            var newId = (contacts_.LastOrDefault()?.ContactId ?? 0) + 1;
            contact.ContactId = newId;
            contacts_.Add(contact);
            return Task.FromResult(newId);
        }

        public async Task Update(Contact updatedContact)
        {
            var contact = await Get(updatedContact.ContactId).ConfigureAwait(false);
            if (contact == null)
            {
                throw new InvalidOperationException(string.Format("Contact with id '{0}' does not exists", updatedContact.ContactId));
            }

            contact.Address = updatedContact.Address;
            contact.City = updatedContact.City;
            contact.Name = updatedContact.Name;
        }

        public async Task Delete(int id)
        {
            var contact = await Get(id).ConfigureAwait(false);
            if (contact == null)
            {
                throw new InvalidOperationException(string.Format("Contact with id '{0}' does not exists", id));
            }

            contacts_.Remove(contact);
        }
    }
}