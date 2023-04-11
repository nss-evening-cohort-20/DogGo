using DogGo.Models;

namespace DogGo.Repositories;

public interface IOwnerRepository
{
    void AddOwner(Owner owner);
    void DeleteOwner(int ownerId);
    List<Owner> GetAllOwners();
    Owner? GetById(int id);
    Owner? GetOwnerByEmail(string email);
    void UpdateOwner(Owner owner);
}