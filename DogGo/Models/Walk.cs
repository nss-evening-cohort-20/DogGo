﻿namespace DogGo.Models;

public class Walk
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; } //in seconds
    public int WalkerId { get; set; }
    public int DogId { get; set; }
}
