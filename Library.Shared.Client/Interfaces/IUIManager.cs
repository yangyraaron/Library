using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Shared.Client.Interfaces
{
    /// <summary>
    /// interface defines all ui operations that will be 
    /// used by the ui classes in charge of the ui logic
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// set content to fundemental ui
        /// </summary>
        /// <param name="content"></param>
        void SetContent(object content);
    }
}
