using DogGo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        void AddDog(Dog dog);
        void DeleteDog(int dogId);
        List<Dog> GetAllDogs();
        Dog? GetDogById(int id);
        List<Dog> GetDogsByNeighborhood(int neighborhoodId);
        List<Dog> GetDogsByOwner(int ownerId);
        void UpdateDog(Dog dog);
    }
}