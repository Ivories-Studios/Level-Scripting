using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Editor
{
    public struct SearchContextElement
    {
        public object Target { get; private set; }
        public string Title { get; private set; }

        public SearchContextElement(object target, string title)
        {
            Target = target;
            Title = title;
        }
    }

    public class LevelScriptWindowSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        #region Static Variables
        public static List<SearchContextElement> Elements { get; set; }
        #endregion

        #region Properties
        public LevelScriptView Sequence { get; set; }
        public VisualElement Target { get; set; }
        #endregion

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 0));

            Elements = new List<SearchContextElement>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.CustomAttributes.ToList() != null)
                    {
                        NodeInfoAttribute attribute = type.GetCustomAttribute<NodeInfoAttribute>();
                        if (attribute != null)
                        {
                            NodeInfoAttribute att = attribute;
                            object node = Activator.CreateInstance(type);
                            if (string.IsNullOrEmpty(att.MenuItem)) { continue; }
                            Elements.Add(new SearchContextElement(node, att.MenuItem));
                        }
                    }
                }
            }

            Elements.Sort((entry1, entry2) =>
            {
                string[] splits1 = entry1.Title.Split('/');
                string[] splits2 = entry2.Title.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length) { return 1; }
                    int value = splits1[i].CompareTo(splits2[i]);
                    if (value != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            return splits1.Length.CompareTo(splits2.Length);
                        }
                        return value;
                    }
                }
                return 0;
            });

            List<string> groups = new List<string>();

            foreach (SearchContextElement element in Elements)
            {
                string[] entryTitle = element.Title.Split('/');
                string groupName = "";

                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = new SearchContextElement(element.Target, element.Title);
                tree.Add(entry);
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 windowMousePosition = Sequence.ChangeCoordinatesTo(Sequence, context.screenMousePosition - Sequence.Window.position.position);
            Vector2 graphMousePosition = Sequence.contentViewContainer.WorldToLocal(windowMousePosition);

            SearchContextElement element = (SearchContextElement)SearchTreeEntry.userData;

            ScriptingNode node = (ScriptingNode)element.Target;
            node.SetPosition(new Rect(graphMousePosition, Vector2.zero));
            Sequence.Add(node);
            return true;
        }
    }
}
