using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;

        public WalkersController(IWalkerRepository walkerRepository)
        {
            _walkerRepo = walkerRepository;
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
            return View(walker);
        }
    }
}
