#if UNITY_EDITOR
using UnityEditor.AssetImporters;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [ScriptedImporter(1, ConversationSaveManager.conversationExtension)]
    public class ConversationImporter : ScriptedImporter
    {
        private const string iconPath = "ConversationIcon";

        public override void OnImportAsset(AssetImportContext context)
        {
            // set icon for file
            Texture2D icon = Resources.Load<Texture2D>(iconPath);
            TextAsset description = new TextAsset(iconPath);
            context.AddObjectToAsset("icon", description, icon);
        }
    }
}
#endif