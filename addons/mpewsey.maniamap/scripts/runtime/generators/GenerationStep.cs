using Godot;
using MPewsey.Common.Pipelines;

namespace MPewsey.ManiaMapGodot.Generators
{
    [Tool]
    [GlobalClass]
    [Icon(ManiaMapResources.Icons.GenerationStepIcon)]
    public abstract partial class GenerationStep : Node
    {
        public abstract string[] RequiredInputNames();
        public abstract string[] OutputNames();
        public abstract IPipelineStep CreateStep();
    }
}