using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        public OwnersController(IOwnerRepository ownerRepo, IWalkerRepository walkerRepo, INeighborhoodRepository neighborhoodRepo)
        {
            _ownerRepo = ownerRepo;
            _walkerRepo = walkerRepo;
            _neighborhoodRepo = neighborhoodRepo;
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

            Owner owner = _ownerRepo.GetById(id);

            if (owner is null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Owner {id} not found" });
            }

            // ?? - null coalesce operator
            int neighborhoodId = owner.NeighborhoodId ?? 0;
            var walkers = _walkerRepo.GetWalkersInNeighborhood(neighborhoodId);
            
            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Walkers = walkers,
            };

            return View(vm);
        }

        // GET: OwnersController/Create
        public IActionResult Create()
        {
            OwnerFormViewModel viewModel = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                NeighborhoodOptions = _neighborhoodRepo.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            };

            return View(viewModel);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepo.AddOwner(vm.Owner);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                vm.NeighborhoodOptions = _neighborhoodRepo.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
                return View(vm);
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            var newData = _neighborhoodRepo.GetAll();


            var neighborhoods = _neighborhoodRepo.GetAll();
            OwnerFormViewModel viewModel = new OwnerFormViewModel()
            {
                Owner = _ownerRepo.GetById(id),
                NeighborhoodOptions = neighborhoods.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            };
            
            if (viewModel.Owner is null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Owner {id} not found." });
            }
            return View(viewModel);
        }

        // POST: OwnersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OwnerFormViewModel vm)
        {
            try
            {
                if (vm.Owner.Id != id)
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"You can't do that." });
                }
                _ownerRepo.UpdateOwner(vm.Owner);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                vm.NeighborhoodOptions = _neighborhoodRepo.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
                return View(vm);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        new Claim(ClaimTypes.Email, owner.Email),
        new Claim(ClaimTypes.Role, "DogOwner"),
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Details", new {id = owner.Id});
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
