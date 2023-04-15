using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;

        public WalkersController(IWalkerRepository walkerRepository, IWalkRepository walkRepo)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepo;
        }

        
        public IActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }

        public IActionResult Details(int id)
        {
            Walker? walker = _walkerRepo.GetWalkerById(id);

            if (walker == null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = $"Walker {id} not found" });
            }

            List<Walk> walks = _walkRepo.GetByWalkerId(id);
            walker.Walks = walks;

            var vm = new WalkerDetailViewModel()
            {
                Walker = walker
            };
            return View(walker);
        }
    }
}
