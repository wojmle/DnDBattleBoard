using System;
using UnityEngine;

[Serializable]
public class Ally : IModelObject
{
    public string Name { get; set; }
    public string Race { get; set; }
    public string Model { get; set; }

    public Ally(string name, string race, string model)
    {
        Name = name;
        Race = race;
        Model = model;
    }
}
