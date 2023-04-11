using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;

        public OwnersController(IOwnerRepository ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }


        // GET: OwnersController
        public IActionResult Index()
        {
            var owners = _ownerRepo.GetAllOwners();
            return View(owners);
        }

        // GET: OwnersController/Details/5
        public IActionResult Details(int id)
        {
            Owner? owner = _ownerRepo.GetById(id);
            if (owner is null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Owner {id} not found" });
            }
            return View(owner);
        }

        // GET: OwnersController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            Owner? ownerToEdit = _ownerRepo.GetById(id);
            if (ownerToEdit is null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Owner {id} not found." });
            }
            return View(ownerToEdit);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                if (owner.Id != id)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"You can't do that." });
                }
                _ownerRepo.UpdateOwner(owner);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner ownerToDelete = _ownerRepo.GetById(id);
            if (ownerToDelete is null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Owner {id} not found and could not be deleted." });
            }
            return View(ownerToDelete);
        }

        // POST: OwnersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                if (owner.Id != id)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"You can't do that, dude." });
                }
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
