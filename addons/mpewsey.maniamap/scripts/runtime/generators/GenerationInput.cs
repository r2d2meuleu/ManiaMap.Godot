using Godot;
using System.Collections.Generic;

namespace MPewsey.ManiaMapGodot.Generators
{
    [Tool]
    [GlobalClass]
    [Icon(ManiaMapResources.Icons.GenerationInputIcon)]
    public abstract partial class GenerationInput : Node
    {
        public abstract string[] InputNames();
        public abstract void AddInputs(Dictionary<string, object> inputs);
    }
}