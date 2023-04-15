using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(IWalkRepository walkRepo, IWalkerRepository walkerRepo, IDogRepository dogRepo)
        {
            _walkRepo = walkRepo;
            _walkerRepo = walkerRepo;
            _dogRepo = dogRepo;
        }

        // GET: WalksController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create(int id)
        {
            Walker? walker = _walkerRepo.GetWalkerById(id);
            if (walker is null)
            {
                return NotFound();
            }
            WalkFormViewModel vm = new WalkFormViewModel();


            List<SelectListItem> dogOptions = _dogRepo.GetDogsByNeighborhood(walker.NeighborhoodId)
                                                      .Select(dog => new SelectListItem() { Value = dog.Id.ToString(), Text = dog.Name })
                                                      .ToList();
            /*
             * {
             *      Id,
             *      Name,
             *      Breed,
             *      OwnerId,
             * }
             * 
             * {
             *      Value,
             *      DisplayText
             * {
             */
            vm.DogOptions = dogOptions;
            vm.WalkerId = id;
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkFormViewModel vm)
        {
            try
            {
                foreach (var dogId in vm.SelectedDogs)
                {
                    Walk walk = new Walk()
                    {
                        Date = vm.Date,
                        Duration = (vm.Hours * 3600) + (vm.Minutes * 60),
                        WalkerId = vm.WalkerId,
                        DogId = dogId
                    };
                    
                    _walkRepo.InsertWalk(walk);
                }

                return RedirectToAction(nameof(Details), "Walkers", new { id = vm.WalkerId });
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
