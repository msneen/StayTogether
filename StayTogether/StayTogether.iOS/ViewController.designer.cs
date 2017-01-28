// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace StayTogether.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ContactsUITableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPhoneNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartGroup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UINameTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UIPhoneNumberTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContactsUITableView != null) {
                ContactsUITableView.Dispose ();
                ContactsUITableView = null;
            }

            if (lblPhoneNumber != null) {
                lblPhoneNumber.Dispose ();
                lblPhoneNumber = null;
            }

            if (StartGroup != null) {
                StartGroup.Dispose ();
                StartGroup = null;
            }

            if (UINameTextField != null) {
                UINameTextField.Dispose ();
                UINameTextField = null;
            }

            if (UIPhoneNumberTextField != null) {
                UIPhoneNumberTextField.Dispose ();
                UIPhoneNumberTextField = null;
            }
        }
    }
}