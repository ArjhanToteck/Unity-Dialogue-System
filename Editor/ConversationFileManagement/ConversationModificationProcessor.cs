#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Handles the user modification of a conversation file.
    /// </summary>
    public class ConversationModificationProcessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            foreach (var window in ConversationEditorWindow.openWindows)
            {
                // check if opened dialogue file was deleted
                if (window.savePath == assetPath)
                {
                    // reset window
                    window.savePath = null;
                    window.ResetWindow();
                }
            }

            return AssetDeleteResult.DidNotDelete;
        }

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            foreach (var window in ConversationEditorWindow.openWindows)
            {
                // check if opened dialogue file was renamed
                if (window.savePath == sourcePath)
                {
                    // set new path
                    window.savePath = destinationPath;
                }
            }

            return AssetMoveResult.DidNotMove;
        }
    }
}
#endif