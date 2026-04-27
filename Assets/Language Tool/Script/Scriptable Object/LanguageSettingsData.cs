/*
 * ---------------------------------------------------------------------------
 * Description: ScriptableObject that stores localization settings for Unity projects.
 *              Includes folder paths, default language, font configuration, culture,
 *              available languages, and metadata for text and canvas localization.
 *              
 * Author: Lucas Gomes Cecchini
 * Pseudonym: AGAMENOM
 * ---------------------------------------------------------------------------
*/

using System.Collections.Generic;
using LanguageTools.Legacy;
using LanguageTools.TMP;
using UnityEngine;

namespace LanguageTools
{
    /// <summary>
    /// Stores localization settings and data for supported languages and culture-specific text rendering.
    /// Used by localization tools and runtime language loading systems.
    /// </summary>
    public class LanguageSettingsData : ScriptableObject
    {
        #region === Folder and Language Settings ===

        [Header("Archives Location")]
        [Tooltip("Directory containing the language files.")]
        public string folderName = "Languages";

        [Space(10)]

        [Header("Default Language")]
        [Tooltip("Default language used when no user preference is stored.")]
        public string defaultLanguage = "en";

        #endregion

        #region === Font Configuration ===

        [Space(10)]

        [Header("Font List Data")]
        [Tooltip("Stores the list of fonts used for standard Unity UI elements.")]
        public LanguageFontListData fontListData;

        [Tooltip("Stores the list of TMP fonts used for TextMeshPro UI elements.")]
        public LanguageFontListDataTMP fontListDataTMP;

        #endregion

        #region === Canvas Configuration ===

        [Space(10)]

        [Header("Canvas Log")]
        [Tooltip("UI prefab shown when language loading or configuration fails.")]
        public GameObject errorLanguageTool;

        #endregion

        #region === Extracted Data ===

        [Space(10)]

        [Header("Extracted Data")]
        [Tooltip("The current language culture selected by the player (e.g., \"en-US\").")]
        public string selectedCulture;

        [Tooltip("All languages defined and optionally available in the build.")]
        public List<LanguageAvailable> availableLanguages = new();

        [Tooltip("Text elements for dynamic localization in non-canvas components.")]
        public List<IdData> idData = new();

        [Tooltip("Additional information or attributes related to each localized ID (e.g., font size or tags).")]
        public List<IdMetaData> idMetaData = new();

        [Tooltip("Text elements specifically used within Unity Canvas UI components.")]
        public List<IdData> idCanvasData = new();

        #endregion
    }
}