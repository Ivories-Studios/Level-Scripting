using System;

namespace IvoriesStudios.LevelScripting.Attributes
{
    public enum IOTypes
    {
        None,
        Single,
        Multiple
    }

    public class NodeInfoAttribute : Attribute
    {
        #region Properties
        public string Title { get; private set; }
        public string MenuItem { get; private set; }
        public IOTypes InputType { get; private set; }
        public IOTypes OutputType { get; private set; }
        public bool IsStopping { get; private set; }
        #endregion

        #region Lifecycle
        public NodeInfoAttribute(string title, string menuItem = "", IOTypes inputType = IOTypes.Multiple, IOTypes outputType = IOTypes.Single, bool isStopping = false)
        {
            Title = title;
            MenuItem = menuItem;
            InputType = inputType;
            OutputType = outputType;
            IsStopping = isStopping;
        }
        #endregion
    }
}
