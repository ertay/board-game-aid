using System;

namespace BoardGameAid.Core.Services
{
    /// <summary>
    /// Interface for creating popups / alerts
    /// </summary>
    public interface IPopupService
    {
        /// <summary>
        /// Show a simple popup with a single dismiss button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttonText"></param>
        /// <param name="dismissCallback"></param>
        /// <param name="dismissable"></param>
        void ShowPopup(string title, string message, string buttonText = "ok", Action dismissCallback = null,
            bool dismissable = true);

        /// <summary>
        /// Show a popup with two buttons.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okButtonText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="okCallback"></param>
        /// <param name="cancelCallback"></param>
        /// <param name="dismissable"></param>
        /// <param name="dismissCallback"></param>
        void ShowPopup(string title, string message, string okButtonText, string cancelButtonText, Action okCallback,
            Action cancelCallback, bool dismissable = false, Action dismissCallback = null);



    }
}
