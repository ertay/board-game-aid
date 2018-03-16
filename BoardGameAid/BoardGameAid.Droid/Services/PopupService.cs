using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoardGameAid.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using AlertDialog = Android.Support.V7.App.AlertDialog;


namespace BoardGameAid.Droid.Services
{
    /// <summary>
    /// Implementatiaon of the popup service used to show popup dialogs.
    /// </summary>
    public class PopupService : IPopupService
    {
        /// <summary>
        /// Shows an alert dialog with a single dismiss button.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttonText"></param>
        /// <param name="dismissCallback"></param>
        public void ShowPopup(string title, string message, string buttonText = "ok", Action dismissCallback = null, bool dismissable = true)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            if (top == null)
            {
                // may occur if in middle of transition
                return;
            }

            // lets build the alert popup
            AlertDialog.Builder builder = new AlertDialog.Builder(top.Activity);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNegativeButton(buttonText, (o, args) =>
            {
                // invoke this actioni only if it is not null
                dismissCallback?.Invoke();
            });
            // if cancellable is set to true, you can dismiss the popup but tapping anywhere on the page outstide it
            builder.SetCancelable(dismissable);
            // make sure the popup is created on the UI thread, otherwise it may throw an exception
            top.Activity.RunOnUiThread(() => { builder.Create().Show(); });

        }

        /// <summary>
        /// Shows an alert dialog with two buttons.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okButtonText"></param>
        /// <param name="cancelButtonText"></param>
        /// <param name="okCallback"></param>
        /// <param name="cancelCallback"></param>
        public void ShowPopup(string title, string message, string okButtonText, string cancelButtonText, Action okCallback,
            Action cancelCallback, bool dismissable = false, Action dismissCallback = null)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            if (top == null)
            {
                // may occur if in middle of transition
                return;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(top.Activity);
            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNegativeButton(cancelButtonText, (o, args) =>
            {
                cancelCallback?.Invoke();
            });
            builder.SetPositiveButton(okButtonText, (o, args) =>
            {
                okCallback?.Invoke();
            });
            builder.SetCancelable(dismissable);
            var dialog = builder.Create();
            // set the dismiss callback, note that this is not the same as tapping cancel
            dialog.CancelEvent += (sender, args) => dismissCallback?.Invoke();
            top.Activity.RunOnUiThread(() => { dialog.Show(); });
        }

    }
}
