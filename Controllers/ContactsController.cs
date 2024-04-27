using CRUD.Data;
using CRUD.Models;
using CRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    public class ContactsController : Controller
    {
        private readonly MVCDbContext mvcDbContesxt;

        public ContactsController(MVCDbContext mvcDbContesxt)
        {
            this.mvcDbContesxt = mvcDbContesxt;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contacts =  await mvcDbContesxt.Contacts.ToListAsync();
            return View(contacts);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddContactViewModel addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Name = addContactRequest.Name,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Date = DateTime.Today,
            };

            await mvcDbContesxt.Contacts.AddAsync(contact);
            await mvcDbContesxt.SaveChangesAsync(); // to save the changes

            return RedirectToAction("Index");


        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var contact = await mvcDbContesxt.Contacts.FirstOrDefaultAsync(x => x.Id == id);

            if (contact != null)
            {

                var viewModel = new EditContactViewModelcs()
                {
                    Id = contact.Id,
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    Date = contact.Date
                };

                return await Task.Run(() =>View("view", viewModel));
            }

            return RedirectToAction("Index");

            
        }

        [HttpPost]
        public async Task<IActionResult> View(EditContactViewModelcs model)
        {
            var contact = await mvcDbContesxt.Contacts.FindAsync(model.Id);

            if (contact != null)
            {
                contact.Name = model.Name;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.Date = model.Date;
                contact.Phone = model.Phone;

                await mvcDbContesxt.SaveChangesAsync();
                    
                return RedirectToAction("View");
            }

            return RedirectToAction("View");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditContactViewModelcs model)
        {
            var contact = await mvcDbContesxt.Contacts.FindAsync(model.Id);

            if (contact != null)
            {
                mvcDbContesxt.Contacts.Remove(contact);
                await mvcDbContesxt.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



    }
}
