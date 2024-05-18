using Godot;
using MPewsey.Common.Serialization;
using MPewsey.ManiaMap;
using System;
using System.Threading.Tasks;

namespace MPewsey.ManiaMapGodot
{
    [Tool]
    [GlobalClass]
    public partial class RoomTemplateResource : Resource
    {
        [Export] public int Id { get; set; } = Rand.GetRandomId();
        [Export] public string TemplateName { get; set; } = "<None>";
        [Export] public string ScenePath { get; set; }
        [Export] public string SceneUidPath { get; set; }

        [ExportGroup("Serialized Text")]
        [Export(PropertyHint.MultilineText)] public string SerializedText { get; set; }

        private RoomTemplate _template;
        public RoomTemplate Template
        {

            get
            {
                if (string.IsNullOrWhiteSpace(SerializedText))
                    throw new Exception($"Serialized text has not been assigned: {this}");

                _template ??= JsonSerialization.LoadJsonString<RoomTemplate>(SerializedText);
                return _template;
            }
            private set => _template = value;
        }

#if TOOLS
        public override void _ValidateProperty(Godot.Collections.Dictionary property)
        {
            base._ValidateProperty(property);
            var name = property["name"].AsStringName();

            if (IsReadOnlyProperty(name))
                property["usage"] = (int)(property["usage"].As<PropertyUsageFlags>() | PropertyUsageFlags.ReadOnly);
        }

        private static bool IsReadOnlyProperty(StringName name)
        {
            return name == PropertyName.ScenePath
                || name == PropertyName.SceneUidPath
                || name == PropertyName.SerializedText;
        }
#endif

        public void Initialize(IRoomNode room)
        {
            Template = null;
            var node = (Node)room;
            var scenePath = node.SceneFilePath;
            var sceneUidPath = ResourceUid.IdToText(ResourceLoader.GetResourceUid(scenePath));
            var template = room.GetMMRoomTemplate(Id, TemplateName);
            SerializedText = JsonSerialization.GetJsonString(template, new JsonWriterSettings());
            ScenePath = scenePath;
            SceneUidPath = sceneUidPath;
        }

        public string GetScenePath()
        {
            if (ResourceLoader.Exists(SceneUidPath))
                return SceneUidPath;
            if (ResourceLoader.Exists(ScenePath))
                return ScenePath;

            throw new Exception($"Scene paths do not exist: (SceneUidPath = {SceneUidPath}, ScenePath = {ScenePath})");
        }

        public PackedScene LoadScene()
        {
            return ResourceLoader.Load<PackedScene>(GetScenePath());
        }

        public Task<PackedScene> LoadSceneAsync(bool useSubThreads = false)
        {
            return AsyncResourceLoader.LoadAsync<PackedScene>(GetScenePath(), string.Empty, useSubThreads);
        }
    }
}