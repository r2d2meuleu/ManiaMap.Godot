using Godot;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap;
using MPewsey.ManiaMapGodot;
using System;

namespace MPewsey.Game
{
    [Tool]
    [GlobalClass]
    public partial class RoomTemplateResource : Resource
    {
        [Export] public int Id { get; set; }
        [Export] public string Name { get; set; }
        [Export] public string ScenePath { get; set; }
        [Export] public string SceneUidPath { get; set; }
        [Export(PropertyHint.MultilineText)] public string SerializedText { get; set; }

        private RoomTemplate _template;
        public RoomTemplate Template
        {

            get
            {
                _template ??= JsonSerialization.LoadJsonString<RoomTemplate>(SerializedText);
                return _template;
            }
            private set => _template = value;
        }

        public void Initialize(IRoomNode room)
        {
            Template = null;
            var node = (Node)room;
            var scenePath = node.SceneFilePath;
            var sceneUidPath = ResourceUid.GetIdPath(ResourceLoader.GetResourceUid(scenePath));
            var template = room.CreateTemplate(Id, Name);
            SerializedText = JsonSerialization.GetJsonString(template, new JsonWriterSettings());
            ScenePath = scenePath;
            SceneUidPath = sceneUidPath;
        }

        public PackedScene LoadScene()
        {
            if (ResourceLoader.Exists(SceneUidPath))
                return ResourceLoader.Load<PackedScene>(SceneUidPath);
            if (ResourceLoader.Exists(ScenePath))
                return ResourceLoader.Load<PackedScene>(ScenePath);

            throw new Exception($"Scene paths do not exist: (SceneUidPath = {SceneUidPath}, ScenePath = {ScenePath})");
        }
    }
}