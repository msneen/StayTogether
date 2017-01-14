using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using StayTogether.Classes;

namespace StayTogether.iOS.Models
{
    public class UITableViewContactsViewSource : UITableViewSource
    {
        private static List<GroupMemberVm> Contacts;


        public UITableViewContactsViewSource(List<GroupMemberVm> contacts)
        {
            Contacts = contacts;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            UITableViewCell cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell();

            cell.TextLabel.Text = Contacts[indexPath.Row].ToString();
            cell.TextLabel.SizeToFit();

            if (Contacts[indexPath.Row].Selected)
            {
                cell.Accessory = UITableViewCellAccessory.Checkmark;
            }
            else
            {
                cell.Accessory = UITableViewCellAccessory.None;
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell();
            cell.Selected = !Contacts[indexPath.Row].Selected;
            Contacts[indexPath.Row].Selected = !Contacts[indexPath.Row].Selected;
            tableView.ReloadData();
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Contacts?.Count ?? 0;
        }

        //////public override nint GetComponentCount(UIPickerView pickerView)
        //////{
        //////    return Contacts?.Count ?? 0;
        //////}

        //////public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        //////{
        //////    return Contacts?.Count ?? 0;
        //////}

        //////public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        //////{
        //////    var intRow = Convert.ToInt32( row );
        //////    switch (component)
        //////    {
        //////        case 0:
        //////            return Contacts[intRow].ToString();
        //////        case 1:
        //////            return "";//    return row.ToString();
        //////        case 2:
        //////            return "";//    return new string((char)('A' + row), 1);
        //////        default:
        //////            throw new NotImplementedException();
        //////    }
        //////}

        //////public override void Selected(UIPickerView pickerView, nint row, nint component)
        //////{
        //////    var intRow = Convert.ToInt32(row);
        //////    Contacts[intRow].Selected = true;


        //////}



        //////public override nfloat GetComponentWidth(UIPickerView pickerView, nint component)
        //////{
        //////    return component == 0 ? 220f : 30f;
        //////}

    }
}