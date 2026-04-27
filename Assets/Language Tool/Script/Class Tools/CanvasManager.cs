/*
 * ---------------------------------------------------------------------------
 * Description: Utility class for serializing, reconstructing and validating 
 *              Unity Canvas hierarchies. Provides functionality to extract 
 *              layout metadata, apply saved structures to existing objects, 
 *              and instantiate canvases programmatically using structured 
 *              data formats. Ensures hierarchy integrity through duplicate 
 *              name detection and preserves component configurations such as 
 *              CanvasScaler and GraphicRaycaster.
 *              
 * Author: Lucas Gomes Cecchini
 * Pseudonym: AGAMENOM
 * ---------------------------------------------------------------------------
*/

using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

namespace LanguageTools
{
    public class CanvasManager
    {
        #region === Constants and Ignored Prefixes ===

        /// <summary>
        /// Name prefixes used by TextMeshPro or other UI systems to create internal layout elements.
        /// Objects whose names start with any of these prefixes are excluded from hierarchy processing.
        /// </summary>
        private static readonly List<string> ignoredPrefixes = new() { "TMP SubMeshUI" , "Dropdown List", "Blocker" };

        #endregion

        #region === Public Methods ===

        /// <summary>
        /// Extracts structural and component metadata from a Canvas GameObject and serializes it into a CanvasStructure.
        /// This includes hierarchy paths, RectTransform configurations, and core UI components.
        /// Temporarily activates inactive children to ensure full traversal and restores their original states afterward.
        /// Fails early if duplicate sibling names are detected, as they would compromise deterministic hierarchy reconstruction.
        /// </summary>
        /// <param name="canvasStructure">Reference to the structure that will be populated with extracted data.</param>
        /// <param name="canvasObject">The source Canvas GameObject to extract data from.</param>
        public static void ExtractCanvasData(ref CanvasStructure canvasStructure, GameObject canvasObject)
        {
            // Initialize an empty CanvasStructure instance.
            canvasStructure = new()
            {
                canvasLayers = new CanvasLayers[0],
                rectTransform = new RectTransformData(),
                canvas = new CanvasData(),
                canvasScaler = new CanvasScalerData(),
                graphicRaycaster = new GraphicRaycasterData()
            };

            // Validate uniqueness of sibling names to prevent hierarchy issues.
            if (ContainsDuplicateSiblings(canvasObject.GetComponent<RectTransform>()))
            {
                Debug.LogError("Extraction failed: Duplicate layer names in hierarchy.", canvasObject);
                return;
            }

            // Set canvas name.
            canvasStructure.canvasName = canvasObject.name;

            // Store original active states and temporarily activate all children for hierarchy traversal.
            Dictionary<GameObject, bool> originalStates = new();
            ActivateAllChildren(canvasObject.transform, originalStates);

            // Extract metadata from core canvas components.
            PopulateCanvasMetadata(ref canvasStructure, canvasObject);

            // Extract and build hierarchy layers.
            canvasStructure.canvasLayers = GenerateCanvasLayers(canvasObject);

            // Restore original active states.
            foreach (var kvp in originalStates) kvp.Key.SetActive(kvp.Value);
        }

        /// <summary>
        /// Applies a previously serialized CanvasStructure onto an existing Canvas GameObject,
        /// updating its hierarchy layout and RectTransform properties without creating new objects.
        /// Only existing matching hierarchy paths are modified; missing elements are reported but not created.
        /// Requires strict structural consistency between stored layer paths and transform data.
        /// </summary>
        /// <param name="canvasStructure">The structure containing layout and metadata to apply.</param>
        /// <param name="canvasObject">The target Canvas GameObject to receive the applied data.</param>
        public static void ApplyCanvasData(CanvasStructure canvasStructure, GameObject canvasObject)
        {
            // Validate data consistency before applying.
            if (canvasStructure.canvasLayers == null || canvasStructure.canvasLayers.Length == 0 || canvasStructure.canvasLayers.Any(l => l.CanvasObjectsLayers.Length != l.rectTransforms.Length))
            {
                Debug.LogError("Invalid layers: Mismatched CanvasObjectsLayers and rectTransforms.", canvasObject);
                return;
            }

            // Apply canvas component metadata.
            UpdateCanvasMetadata(canvasStructure, canvasObject);

            var root = canvasObject.GetComponent<RectTransform>();

            // Traverse the hierarchy and apply RectTransform data to each matching element.
            foreach (var layer in canvasStructure.canvasLayers)
            {
                var parent = root;

                for (int i = 0; i < layer.CanvasObjectsLayers.Length; i++)
                {
                    string name = layer.CanvasObjectsLayers[i];
                    var existing = FindChildByName(parent, name);

                    if (existing != null && existing.TryGetComponent(out RectTransform rect))
                    {
                        // Apply transform data to matching element.
                        var data = layer.rectTransforms[i];
                        rect.localPosition = data.localPosition;
                        rect.localRotation = data.localRotation;
                        rect.localScale = data.localScale;
                        rect.anchorMin = data.anchorMin;
                        rect.anchorMax = data.anchorMax;
                        rect.anchoredPosition = data.anchoredPosition;
                        rect.sizeDelta = data.sizeDelta;
                        rect.pivot = data.pivot;

                        // Proceed to next child in hierarchy.
                        parent = rect;
                    }
                    else
                    {
                        Debug.LogWarning($"Missing layer: '{name}' not found.", canvasObject);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new Canvas GameObject from a CanvasStructure definition,
        /// reconstructing its hierarchy, RectTransform layout, and core UI components.
        /// Missing hierarchy elements are instantiated, and visual debug aids (Image color and Outline)
        /// are optionally added to improve editor visibility.
        /// Requires a valid structure with consistent layer and transform data.
        /// </summary>
        /// <param name="canvasStructure">The structure describing the Canvas to be created.</param>
        /// <param name="canvasObject">Outputs the newly created Canvas GameObject, or null if creation fails.</param>
        public static void CreateCanvasFromStructure(CanvasStructure canvasStructure, out GameObject canvasObject)
        {
            canvasObject = null;

            // Ensure valid canvas name.
            if (string.IsNullOrEmpty(canvasStructure.canvasName))
            {
                Debug.LogError("Creation failed: Canvas name is not defined.", canvasObject);
                return;
            }

            // Validate structural consistency before creating the hierarchy.
            if (canvasStructure.canvasLayers == null || canvasStructure.canvasLayers.Length == 0 || canvasStructure.canvasLayers.Any(l => l.CanvasObjectsLayers.Length != l.rectTransforms.Length))
            {
                Debug.LogError("Creation failed: Mismatched CanvasObjectsLayers and rectTransforms.", canvasObject);
                return;
            }

            var colorMap = new Dictionary<int, Color>();

            // Create root canvas GameObject and attach required components.
            canvasObject = new GameObject(canvasStructure.canvasName, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            UpdateCanvasMetadata(canvasStructure, canvasObject);
            var root = canvasObject.GetComponent<RectTransform>();

            // Build canvas hierarchy layer by layer.
            foreach (var layer in canvasStructure.canvasLayers)
            {
                var parent = root;

                for (int i = 0; i < layer.CanvasObjectsLayers.Length; i++)
                {
                    string name = layer.CanvasObjectsLayers[i];
                    var existing = FindChildByName(parent, name);

                    if (existing != null)
                    {
                        parent = existing;
                        continue;
                    }

                    // Instantiate new GameObject for layer element.
                    var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Outline));

                    if (go.TryGetComponent(out RectTransform rect))
                    {
                        rect.SetParent(parent != null ? parent : root, false);

                        // Apply RectTransform data if available.
                        if (i < layer.rectTransforms.Length)
                        {
                            var data = layer.rectTransforms[i];
                            rect.localPosition = data.localPosition;
                            rect.localRotation = data.localRotation;
                            rect.localScale = data.localScale;
                            rect.anchorMin = data.anchorMin;
                            rect.anchorMax = data.anchorMax;
                            rect.anchoredPosition = data.anchoredPosition;
                            rect.sizeDelta = data.sizeDelta;
                            rect.pivot = data.pivot;
                        }

                        // Assign a random debug color to the Image component for visual clarity in the Editor.
                        if (go.TryGetComponent(out Image image))
                        {
                            if (!colorMap.ContainsKey(i)) colorMap[i] = new Color(Random.value, Random.value, Random.value);
                            image.color = colorMap[i];
                        }

                        // Add outline effect for visibility.
                        if (go.TryGetComponent(out Outline outline)) outline.effectDistance = new Vector2(3, 3);

                        parent = rect;
                    }
                }
            }
        }

        #endregion

        #region === Hierarchy Processing ===

        /// <summary>
        /// Traverses the Canvas hierarchy and converts it into a collection of CanvasLayers,
        /// where each layer represents a full path from a root child to a leaf node.
        /// Ignored objects (based on predefined name prefixes) are excluded from traversal,
        /// ensuring only meaningful UI elements are serialized.
        /// </summary>
        /// <param name="canvasObject">The root Canvas GameObject to process.</param>
        /// <returns>An array of CanvasLayers representing all valid hierarchy paths.</returns>
        private static CanvasLayers[] GenerateCanvasLayers(GameObject canvasObject)
        {
            var paths = new List<List<RectTransform>>();

            // Recursively collect all RectTransform paths from root to each leaf node.
            void Collect(RectTransform parent, List<RectTransform> path)
            {
                // Ignore objects whose names start with any ignored prefix.
                if (IsIgnoredName(parent.name)) return;

                path.Add(parent);
                bool hasValidChild = false;

                foreach (RectTransform child in parent)
                {
                    // Only include children that are not ignored.
                    if (!IsIgnoredName(child.name))
                    {
                        hasValidChild = true;
                        Collect(child, path);
                    }
                }

                if (!hasValidChild) paths.Add(new(path));

                path.RemoveAt(path.Count - 1);
            }

            // Start collecting from the root children.
            foreach (RectTransform child in canvasObject.transform)
            {
                if (!IsIgnoredName(child.name)) Collect(child, new());
            }

            // Convert collected paths into CanvasLayers.
            return paths.Select(p => new CanvasLayers
            {
                CanvasObjectsLayers = p.Select(t => t.name).ToArray(),
                rectTransforms = p.Select(ConvertToRectTransformData).ToArray()
            }).ToArray();
        }

        /// <summary>
        /// Recursively checks whether any sibling group within the hierarchy contains duplicate names.
        /// Objects with ignored prefixes are excluded from validation.
        /// This constraint is critical to ensure deterministic hierarchy reconstruction based on names.
        /// </summary>
        /// <param name="parent">The RectTransform root to begin validation from.</param>
        /// <returns>True if any duplicate sibling names are found; otherwise, false.</returns>
        private static bool ContainsDuplicateSiblings(RectTransform parent)
        {
            HashSet<string> names = new();

            foreach (RectTransform child in parent)
            {
                // Skip children whose names start with any ignored prefix.
                if (IsIgnoredName(child.name)) continue;

                // Check for duplicate names among siblings at this level.
                if (!names.Add(child.name))
                {
                #if UNITY_EDITOR
                    UnityEditor.Selection.activeObject = child;
                    Debug.LogWarning($"Duplicate: {child.name}", child.gameObject);
                #endif
                    return true;
                }

                // Recursively check child levels.
                if (ContainsDuplicateSiblings(child)) return true;
            }

            return false;
        }

        #endregion

        #region === Utility Methods ===

        /// <summary>
        /// Converts a RectTransform component into a serializable RectTransformData structure,
        /// capturing all layout-relevant properties required for reconstruction.
        /// </summary>
        /// <param name="t">The RectTransform to convert.</param>
        /// <returns>A populated RectTransformData instance.</returns>
        private static RectTransformData ConvertToRectTransformData(RectTransform t) => new()
        {
            localPosition = t.localPosition,
            localRotation = t.localRotation,
            localScale = t.localScale,
            anchorMin = t.anchorMin,
            anchorMax = t.anchorMax,
            anchoredPosition = t.anchoredPosition,
            sizeDelta = t.sizeDelta,
            pivot = t.pivot
        };

        /// <summary>
        /// Recursively activates all child GameObjects in a hierarchy while storing their original active states.
        /// This ensures that inactive elements are included during hierarchy traversal and can be restored afterward.
        /// Ignored objects are skipped and not modified.
        /// </summary>
        /// <param name="parent">The root transform to begin activation from.</param>
        /// <param name="states">Dictionary used to store original active states for later restoration.</param>
        private static void ActivateAllChildren(Transform parent, Dictionary<GameObject, bool> states)
        {
            foreach (Transform child in parent)
            {
                // Skip children whose names start with ignored prefixes.
                if (IsIgnoredName(child.name)) continue;

                var go = child.gameObject;

                // Store and activate if necessary.
                if (!states.ContainsKey(go))
                {
                    states[go] = go.activeSelf;
                    if (!go.activeSelf) go.SetActive(true);
                }

                // Recurse into deeper children.
                ActivateAllChildren(child, states);
            }
        }

        /// <summary>
        /// Searches for a direct child RectTransform under a given parent by name.
        /// The search is non-recursive and limited to immediate children.
        /// </summary>
        /// <param name="parent">The parent RectTransform to search within.</param>
        /// <param name="name">The name of the child to find.</param>
        /// <returns>The matching RectTransform if found; otherwise, null.</returns>
        private static RectTransform FindChildByName(RectTransform parent, string name)
        {
            foreach (RectTransform child in parent)
                if (child.name == name) return child;
            return null;
        }

        /// <summary>
        /// Determines whether a GameObject name should be ignored during hierarchy processing
        /// based on predefined internal-use prefixes (e.g., TextMeshPro generated elements).
        /// </summary>
        /// <param name="name">The name to evaluate.</param>
        /// <returns>True if the name matches any ignored prefix; otherwise, false.</returns>
        private static bool IsIgnoredName(string name)
        {
            // Check if the name starts with any of the ignored prefixes.
            foreach (var prefix in ignoredPrefixes)
                if (name.StartsWith(prefix)) return true;
            return false;
        }

        #endregion

        #region === Metadata Extraction and Application ===

        /// <summary>
        /// Extracts component-level metadata from a Canvas GameObject and stores it into a CanvasStructure.
        /// Includes RectTransform, Canvas, CanvasScaler, and GraphicRaycaster configurations.
        /// Only components present on the object are processed.
        /// </summary>
        /// <param name="s">Reference to the structure that will receive the extracted metadata.</param>
        /// <param name="obj">The source GameObject containing Canvas-related components.</param>
        private static void PopulateCanvasMetadata(ref CanvasStructure s, GameObject obj)
        {
            // Populate structure with RectTransform data.
            if (obj.TryGetComponent(out RectTransform rt)) s.rectTransform = ConvertToRectTransformData(rt);

            // Extract Canvas settings.
            if (obj.TryGetComponent(out Canvas canvas))
            {
                var c = s.canvas;
                c.renderMode = canvas.renderMode;
                c.planeDistance = canvas.planeDistance;
                c.pixelPerfect = canvas.pixelPerfect;
                c.overrideSorting = canvas.overrideSorting;
                c.overridePixelPerfect = canvas.overridePixelPerfect;
                c.sortingBucketNormalizedSize = canvas.normalizedSortingGridSize;
                c.vertexColorAlwaysGammaSpace = canvas.vertexColorAlwaysGammaSpace;
                c.additionalShaderChannels = canvas.additionalShaderChannels;
                c.updateRectTransformForStandalone = canvas.updateRectTransformForStandalone;
            }

            // Extract CanvasScaler settings.
            if (obj.TryGetComponent(out CanvasScaler scaler))
            {
                var sData = s.canvasScaler;
                sData.uiScaleMode = scaler.uiScaleMode;
                sData.referencePixelsPerUnit = scaler.referencePixelsPerUnit;
                sData.scaleFactor = scaler.scaleFactor;
                sData.referenceResolution = scaler.referenceResolution;
                sData.screenMatchMode = scaler.screenMatchMode;
                sData.matchWidthOrHeight = scaler.matchWidthOrHeight;
                sData.physicalUnit = scaler.physicalUnit;
                sData.fallbackScreenDPI = scaler.fallbackScreenDPI;
                sData.defaultSpriteDPI = scaler.defaultSpriteDPI;
                sData.dynamicPixelsPerUnit = scaler.dynamicPixelsPerUnit;
            }

            // Extract GraphicRaycaster settings.
            if (obj.TryGetComponent(out GraphicRaycaster raycaster))
            {
                var g = s.graphicRaycaster;
                g.ignoreReversedGraphics = raycaster.ignoreReversedGraphics;
                g.blockingObjects = raycaster.blockingObjects;
                g.blockingMask = raycaster.blockingMask;
            }
        }

        /// <summary>
        /// Applies stored metadata from a CanvasStructure onto a target Canvas GameObject,
        /// updating its RectTransform and core UI component configurations.
        /// Only existing components are modified; missing components are ignored.
        /// </summary>
        /// <param name="s">The structure containing metadata to apply.</param>
        /// <param name="obj">The target GameObject to receive the metadata.</param>
        private static void UpdateCanvasMetadata(CanvasStructure s, GameObject obj)
        {
            // Apply RectTransform data.
            if (obj.TryGetComponent(out RectTransform rt))
            {
                var data = s.rectTransform;
                rt.localPosition = data.localPosition;
                rt.localRotation = data.localRotation;
                rt.localScale = data.localScale;
                rt.anchorMin = data.anchorMin;
                rt.anchorMax = data.anchorMax;
                rt.anchoredPosition = data.anchoredPosition;
                rt.sizeDelta = data.sizeDelta;
                rt.pivot = data.pivot;
            }

            // Apply Canvas settings.
            if (obj.TryGetComponent(out Canvas canvas))
            {
                var c = s.canvas;
                canvas.renderMode = c.renderMode;
                canvas.planeDistance = c.planeDistance;
                canvas.pixelPerfect = c.pixelPerfect;
                canvas.overrideSorting = c.overrideSorting;
                canvas.overridePixelPerfect = c.overridePixelPerfect;
                canvas.normalizedSortingGridSize = c.sortingBucketNormalizedSize;
                canvas.vertexColorAlwaysGammaSpace = c.vertexColorAlwaysGammaSpace;
                canvas.additionalShaderChannels = c.additionalShaderChannels;
                canvas.updateRectTransformForStandalone = c.updateRectTransformForStandalone;
            }

            // Apply CanvasScaler settings.
            if (obj.TryGetComponent(out CanvasScaler scaler))
            {
                var sData = s.canvasScaler;
                scaler.uiScaleMode = sData.uiScaleMode;
                scaler.referencePixelsPerUnit = sData.referencePixelsPerUnit;
                scaler.scaleFactor = sData.scaleFactor;
                scaler.referenceResolution = sData.referenceResolution;
                scaler.screenMatchMode = sData.screenMatchMode;
                scaler.matchWidthOrHeight = sData.matchWidthOrHeight;
                scaler.physicalUnit = sData.physicalUnit;
                scaler.fallbackScreenDPI = sData.fallbackScreenDPI;
                scaler.defaultSpriteDPI = sData.defaultSpriteDPI;
                scaler.dynamicPixelsPerUnit = sData.dynamicPixelsPerUnit;
            }

            // Apply GraphicRaycaster settings.
            if (obj.TryGetComponent(out GraphicRaycaster raycaster))
            {
                var g = s.graphicRaycaster;
                raycaster.ignoreReversedGraphics = g.ignoreReversedGraphics;
                raycaster.blockingObjects = g.blockingObjects;
                raycaster.blockingMask = g.blockingMask;
            }
        }

        #endregion
    }
}