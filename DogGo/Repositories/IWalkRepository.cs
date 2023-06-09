﻿using DogGo.Models;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAll();
        List<Walk> GetByWalkerId(int id);
        void InsertWalk(Walk walk);
    }
}