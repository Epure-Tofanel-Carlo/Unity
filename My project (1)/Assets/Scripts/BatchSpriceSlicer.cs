using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Put this script in the Assets/Editor folder
namespace MaruEditor.Sprites
{
    // Select a bunch of sprites, and slice them all at once
    public class BatchSpriteSlicer : EditorWindow
    {
        [MenuItem("Multi-Slicer/Slice Selected Sprites")]
        static void Init()
        {
            var window = CreateInstance<BatchSpriteSlicer>();
            window.Show();
        }

        private static Vector2Int lastSize;

        private Rect buttonRect;
        private Vector2Int sliceSize;
        private bool sliceClicked;
        private bool cancelClicked;

        void OnGUI()
        {
            if (sliceSize == Vector2Int.zero)
            {
                sliceSize = lastSize;
            }
            sliceSize = EditorGUILayout.Vector2IntField("Slice Size", sliceSize);
            lastSize = sliceSize;
            sliceClicked = GUILayout.Button("Slice");
            cancelClicked = GUILayout.Button("Cancel");
            if (sliceClicked)
            {
                if (sliceSize.x == 0 || sliceSize.y == 0)
                {
                    Debug.LogError("Must set Slice Size to non-zero values");
                    return;
                }

                SliceSprites();
                Debug.Log("Done Slicing!");
                Close();
            }

            if (cancelClicked)
            {
                Close();
            }
        }

        // adapted from https://answers.unity.com/questions/1113025/batch-operation-to-slice-sprites-in-editor.html
        void SliceSprites()
        {
            foreach (var si in Selection.objects)
            {
                var tex = si as Texture2D;
                var path = AssetDatabase.GetAssetPath(tex);
                var ti = AssetImporter.GetAtPath(path) as TextureImporter;
                if (ReferenceEquals(ti, null))
                {
                    continue;
                }
                ti.isReadable = true;
                ti.spriteImportMode = SpriteImportMode.Multiple;
                var newData = new List<SpriteMetaData>();
                for (var j = tex.height; j > 0; j -= sliceSize.y)
                {
                    for (var i = 0; i < tex.width; i += sliceSize.x)
                    {
                        // Verifică dacă rect-ul nu este complet opac
                        if (!IsRectCompletelyOpaque(tex, new Rect(i, j - sliceSize.y, sliceSize.x, sliceSize.y)))
                        {
                            var smd = new SpriteMetaData
                            {
                                pivot = new Vector2(0.5f, 0.5f),
                                alignment = 9,
                                name = tex.name + "_" + newData.Count,
                                rect = new Rect(i, j - sliceSize.y, sliceSize.x, sliceSize.y),
                            };
                            newData.Add(smd);
                        }
                    }
                }

                ti.spritesheet = newData.ToArray();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
        }

        // Verifică dacă un rect dintr-o textură este complet opac
        bool IsRectCompletelyOpaque(Texture2D texture, Rect rect)
        {
            int startX = Mathf.FloorToInt(rect.xMin);
            int endX = Mathf.CeilToInt(rect.xMax);
            int startY = Mathf.FloorToInt(rect.yMin);
            int endY = Mathf.CeilToInt(rect.yMax);

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);
                    if (pixelColor.a > 0f )
                    {
                        // Pixelul are o valoare alfa între 0 și 1, deci nu este complet opac
                        return false;
                    }
                }
            }

            // Toate pixelii din rect au valoarea alfa egală cu 1, deci rect-ul este complet opac
            return true;
        }




    }
}