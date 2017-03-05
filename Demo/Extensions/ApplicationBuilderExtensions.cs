using System;
using AspNetCore.Models;
using AspNetCore.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void BuildApp(this IApplicationBuilder app)
        {
            app.UseRouter(r =>
            {
                var contactRepo = new InMemoryContactRepository();

                r.MapGet("contacts", async (request, response, routeData) =>
                {
                    var contacts = await contactRepo.GetAll();
                    await response.WriteJson(contacts);
                });

                r.MapGet("contacts/{id:int}", async (request, response, routeData) =>
                {
                    var contact = await contactRepo.Get(Convert.ToInt32(routeData.Values["id"]));
                    if (contact == null)
                    {
                        response.StatusCode = 404;
                        return;
                    }

                    await response.WriteJson(contact);
                });

                r.MapPost("contacts", async (request, response, routeData) =>
                {
                    var newContact = await request.HttpContext.ReadFromJson<Contact>();
                    if (newContact == null) return;

                    await contactRepo.Add(newContact);

                    response.StatusCode = 201;
                    await response.WriteJson(newContact);
                });

                r.MapPut("contacts/{id:int}", async (request, response, routeData) =>
                {
                    var updatedContact = await request.HttpContext.ReadFromJson<Contact>();
                    if (updatedContact == null) return;

                    updatedContact.ContactId = Convert.ToInt32(routeData.Values["id"]);
                    await contactRepo.Update(updatedContact);

                    response.StatusCode = 204;
                });

                r.MapDelete("contacts/{id:int}", async (request, response, routeData) =>
                {
                    await contactRepo.Delete(Convert.ToInt32(routeData.Values["id"]));
                    response.StatusCode = 204;
                });
            });
        }
    }
}