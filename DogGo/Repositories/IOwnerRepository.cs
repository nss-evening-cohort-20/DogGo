using DogGo.Models;

namespace DogGo.Repositories;

public interface IOwnerRepository
{
    List<Owner> GetAllOwners();
    Owner? GetById(int id);
}