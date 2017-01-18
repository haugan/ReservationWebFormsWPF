using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace HotellAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables
        // For holding user selected values. 
        private string _selectedGuestName = null;
        private string _selectedGuestID = null;
        private string _selectedRoomID = null;
        private string _selectedCheckIn = null;
        private string _selectedLengthOfStay = null;
        private string _selectedReservationID = null;

        // Paths to XML files.
        private string _reservationsFile = @"C:\XML\Reservations.xml";
        private string _guestsFile = @"C:\XML\Guests.xml";
        private string _roomsFile = @"C:\XML\Rooms.xml";

        private XElement _reservationsXML = null;
        private XElement _guestsXML = null;
        private XElement _roomsXML = null;

        // Timestamp for debugging
        private string _timeStamp;
        #endregion

        // Constructor
        public MainWindow()
        {
            InitializeComponent();

            _reservationsXML = XElement.Load(_reservationsFile);
            _guestsXML = XElement.Load(_guestsFile);
            _roomsXML = XElement.Load(_roomsFile);

            DisplayRoomIDsInComboBox();
            DisplayGuestNamesInComboBox();
            DisplayReservationsInListBox();
            DisplayAllGuestsInListBox();
        }

        #region Display Methods
        // Display Guest Names from "Guests.xml" in "listBox_Guests"
        // Clear Items each time, so they don't double up
        // Show only guests with / without reservations if correct checkbox has value of true (i.e "is checked")
        private void DisplayAllGuestsInListBox()
        {
            if (checkBox_ReservedTrue.IsChecked == true)
            {
                IEnumerable<XElement> guests =
                (
                from n in _guestsXML.Elements("Guest")
                where n.Element("HasReservation").Value == "1"
                orderby n.Element("Name").Value //ascending
                select n
                );

                listBox_Guests.Items.Clear();
                foreach (var g in guests)
                {
                    string name = g.Element("Name").Value;
                    listBox_Guests.Items.Add(name);
                }

                Console.WriteLine("> Displaying guests with reservations..");
            }
            else if (checkBox_ReservedFalse.IsChecked == true)
            {
                IEnumerable<XElement> guests =
                (
                from n in _guestsXML.Elements("Guest")
                where n.Element("HasReservation").Value == "0"
                orderby n.Element("Name").Value //ascending
                select n
                );

                listBox_Guests.Items.Clear();
                foreach (var g in guests)
                {
                    string name = g.Element("Name").Value;
                    listBox_Guests.Items.Add(name);
                }

                Console.WriteLine("> Displaying guests without reservations..");
            }
            else
            {
                IEnumerable<XElement> guests =
                (
                from n in _guestsXML.Elements("Guest")
                orderby n.Element("Name").Value //ascending
                select n
                );

                listBox_Guests.Items.Clear();
                foreach (var g in guests)
                {
                    string name = g.Element("Name").Value;
                    listBox_Guests.Items.Add(name);
                }

                Console.WriteLine("> Displaying all guests..");
            }
        }

        private void DisplayReservationsInListBox()
        {
            IEnumerable<XElement> elements =
            (
                from r
                in _reservationsXML.Elements("Reservation")
                select r
            );

            listBox_Reservations.Items.Clear();
            foreach (var elm in elements)
            {
                listBox_Reservations.Items.Add(elm.Element("ReservationID").Value);
            }
        }

        private void DisplayRoomIDsInComboBox()
        {
            IEnumerable<XElement> elements =
            (
                from r
                in _roomsXML.Elements("Room").Elements("RoomID")
                select r
            );

            foreach (var elm in elements)
            {
                comboBox_Room.Items.Add(elm.Value);
            }
        }

        private void DisplayGuestNamesInComboBox()
        {
            IEnumerable<XElement> elements =
            (
                from n in _guestsXML.Elements("Guest").Elements("Name")
                select n
            );

            foreach (var elm in elements)
            {
                comboBox_Guest.Items.Add(elm.Value);
            }
        }

        private void DisplayReservationInfo(IEnumerable<XElement> list)
        {
            foreach (var item in list)
            {
                _selectedCheckIn = item.Element("CheckInDate").Value;
                DateTime dt = Convert.ToDateTime(_selectedCheckIn);
                //cal_CheckIn.DisplayDateStart = dt;
                cal_CheckIn.DisplayDate = dt;
                cal_CheckIn.SelectedDate = dt;

                _selectedLengthOfStay = item.Element("Nights").Value;
                textBox_Nights.Text = _selectedLengthOfStay;

                _selectedRoomID = item.Element("RoomID").Value;
                int roomIndex = comboBox_Room.Items.IndexOf(_selectedRoomID);
                comboBox_Room.SelectedItem = comboBox_Room.Items.GetItemAt(roomIndex);

                _selectedGuestID = item.Element("GuestID").Value;

                XElement guestName =
                (
                    from g in _guestsXML.Elements("Guest")
                    where g.Element("GuestID").Value == _selectedGuestID
                    select g).SingleOrDefault();

                _selectedGuestName = guestName.Element("Name").Value;

                int _guestIndex = comboBox_Guest.Items.IndexOf(_selectedGuestName);
                comboBox_Guest.SelectedItem = comboBox_Guest.Items.GetItemAt(_guestIndex);

            }
        }

        private void ClearReservationInfo()
        {
            cal_CheckIn.SelectedDates.Clear();
            cal_CheckIn.DisplayDateStart = null;
            textBox_Nights.Clear();
            comboBox_Room.SelectedItem = null;
            comboBox_Guest.SelectedItem = null;

            DisplayReservationsInListBox();
        }
        #endregion

        #region Button Click Events
        // BUTTONS
        private void btn_ClearGuest_Click(object sender, RoutedEventArgs e)
        {
            textBox_Name.Clear();
            checkBox_ReservedTrue.IsChecked = false;
            checkBox_ReservedFalse.IsChecked = false;

            DisplayAllGuestsInListBox();
        }

        private void btn_ClearReservation_Click(object sender, RoutedEventArgs e)
        {
            ClearReservationInfo();
        }
        #endregion

        #region Checkbox Checked / Un-checked Events
        // CHECKBOXES
        private void checkBox_ReservedTrue_Checked(object sender, RoutedEventArgs e)
        {
            checkBox_ReservedFalse.IsChecked = false;
            DisplayAllGuestsInListBox();

            Console.WriteLine("> ReservedTrue checked..");
        }

        private void checkBox_ReservedFalse_Checked(object sender, RoutedEventArgs e)
        {
            checkBox_ReservedTrue.IsChecked = false;
            DisplayAllGuestsInListBox();

            Console.WriteLine("> ReservedFalse checked..");
        }

        private void checkBox_ReservedTrue_Unchecked(object sender, RoutedEventArgs e)
        {
            checkBox_ReservedTrue.IsChecked = false;
            DisplayAllGuestsInListBox();

            Console.WriteLine("> ReservedTrue unchecked..");
        }

        private void checkBox_ReservedFalse_Unchecked(object sender, RoutedEventArgs e)
        {
            checkBox_ReservedFalse.IsChecked = false;
            DisplayAllGuestsInListBox();

            Console.WriteLine("> ReservedFalse unchecked..");
        }
        #endregion

        #region Combobox Drop-Down Opened Events
        // COMBO BOXES
        // Display Guest Names from "Guests.xml" in "comboBox_Guest" Drop-Down List 
        private void comboBox_Guest_DropDownOpened(object sender, EventArgs e)
        {
            comboBox_Guest.Items.Clear();
            DisplayGuestNamesInComboBox();

            Console.WriteLine("> Displaying all guests..");
        }

        // Display Room IDs / Numbers from "Rooms.xml" in "comboBox_Room" Drop-Down List
        // Clear Items each time it is opened, so they don't double up
        private void comboBox_Room_DropDownOpened(object sender, EventArgs e)
        {
            comboBox_Room.Items.Clear();
            DisplayRoomIDsInComboBox();

            Console.WriteLine("> Displaying all rooms..");
        }
        #endregion

        #region Listbox Selection Changed Events
        // LIST BOXES
        // Get Item from "listBox_Guests"
        // Display the user-selected Guest's Name in "textBox_Name" Input-Field
        private void listBox_Guests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearReservationInfo();

            if (listBox_Guests.SelectedItem != null)
            {
                textBox_Name.Text = listBox_Guests.SelectedItem.ToString();
                _selectedGuestName = textBox_Name.Text;

                _selectedGuestID = GetGuestIdFromName(_selectedGuestName);

                if (GuestIdExistsInReservationsXML(_selectedGuestID))
                {
                    // This method calls another method which is responsible for displaying all reservation info.
                    GetReservationsFromGuestID(_selectedGuestID);
                }

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Guest ID: {_selectedGuestID}");
            }
        }

        // Get user-selected Item from "listBox_Reservations"
        private void listBox_Reservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_Reservations.SelectedItem != null)
            {
                _selectedReservationID = listBox_Reservations.SelectedItem.ToString();

                IEnumerable<XElement> reservations =
                (
                    from r in _reservationsXML.Elements("Reservation")
                    where r.Element("ReservationID").Value == _selectedReservationID
                    select r
                );

                DisplayReservationInfo(reservations);

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Reservation ID: {_selectedReservationID}");
            }
        }
        #endregion

        #region Register Methods
        // Register new Guest XML Node, with Name and Random ID, and save to "Guests.xml"
        // Show Error message if User didn't enter Name
        // Show Success message is Registration was complete
        // Clear "textBox_Name", then reload and display updated list of Names in "listBox_Guests"
        private void btn_RegisterGuest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_Name.Text))
            {
                MessageBox.Show
                (
                    "Please enter a name.",
                    "This field can't be empty!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
            }
            else
            {
                Random r = new Random();

                XElement guest =
                    new XElement("Guest",
                        new XElement("GuestID", r.Next(10000)),
                        new XElement("Name", textBox_Name.Text),
                        new XElement("HasReservation", "0")
                    );

                _guestsXML.Add(guest);
                _guestsXML.Save(_guestsFile);


                textBox_Name.Text = "";
                MessageBox.Show
                (
                    "New guest was registered.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                DisplayAllGuestsInListBox();

                Console.WriteLine("> Guest registered..");
            }
        }

        // User clicks Register Reservation button.
        private void btn_RegisterReservation_Click(object sender, RoutedEventArgs e)
        {
            if (cal_CheckIn.SelectedDate == null)
            {
                MessageBox.Show
                (
                    "Please choose a check in date.",
                    "This field can't be empty!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
            }
            else if (string.IsNullOrEmpty(textBox_Nights.Text))
            {
                MessageBox.Show
                (
                    "Please enter length of stay.",
                    "This field can't be empty!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
            }
            else if (comboBox_Room.SelectedItem == null)
            {
                MessageBox.Show
                (
                    "Please choose room number.",
                    "This field can't be empty!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
            }
            else if (comboBox_Guest.SelectedItem == null)
            {
                MessageBox.Show
                (
                    "Please choose a guest.",
                    "This field can't be empty!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation
                );
            }
            else
            {
                _selectedGuestID = GetGuestIdFromName(comboBox_Guest.SelectedItem.ToString());

                Random r = new Random();

                XElement reservation =
                    new XElement("Reservation",
                        new XElement("ReservationID", r.Next(10000)),
                        new XElement("GuestID", _selectedGuestID),
                        new XElement("RoomID", comboBox_Room.SelectedItem.ToString()),
                        new XElement("CheckInDate", cal_CheckIn.SelectedDate.Value.ToShortDateString()),
                        new XElement("Nights", textBox_Nights.Text)
                    );

                _reservationsXML.Add(reservation);
                _reservationsXML.Save(_reservationsFile);

                // Set guest reservation XML element to "true / 1" (HasReservation) for selected guest
                ChangeGuestReservationStatus("1", _selectedGuestID);

                // Clear reservation info for user.
                textBox_Nights.Clear();
                comboBox_Room.SelectedItem = null;
                comboBox_Guest.SelectedItem = null;

                MessageBox.Show
                (
                    "New reservation was registered.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Refresh List Box displays of Guests and Reservations
                DisplayReservationsInListBox();
                DisplayAllGuestsInListBox();

                Console.WriteLine($"> Reservation registered..");
            }
        }

        // Update "HasReservation" element in user-selected Guest from "Guests.xml"
        // "0" = False (Guest has no reservation), "1" = True
        private void ChangeGuestReservationStatus(string status, string guestID)
        {
            XElement guest =
            (
            from g in _guestsXML.Elements("Guest")
            where g.Element("GuestID").Value == guestID
            select g).SingleOrDefault();

            guest.Element("HasReservation").Value = status;

            _guestsXML.Save(_guestsFile);
        }
        #endregion

        #region Update Methods
        // Update XML Node that matches user-selected Name, and save to "Guests.xml"
        // Show Error message if User didn't enter Name
        // Show Success message is Update was complete
        // Clear "textBox_Name", then reload and display updated list of Names in "listbox_Guests"
        private void btn_UpdateGuest_Click(object sender, RoutedEventArgs e)
        {
            XElement guest =
            (
                from g in _guestsXML.Elements("Guest")
                where g.Element("Name").Value == _selectedGuestName
                select g).SingleOrDefault();

            if (guest != null)
            {
                if (string.IsNullOrEmpty(textBox_Name.Text))
                {
                    MessageBox.Show
                    (
                        "Please enter a name.",
                        "This field can't be empty.",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
                else
                {
                    guest.Element("Name").Value = textBox_Name.Text;

                    textBox_Name.Text = "";
                    MessageBox.Show
                    (
                        "Guest's name was updated.",
                        "Success!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    _guestsXML.Save(_guestsFile);
                    DisplayAllGuestsInListBox();

                    Console.WriteLine("> Updated guest..");
                }
            }


        }

        private void btn_UpdateReservation_Click(object sender, RoutedEventArgs e)
        {
            XElement reservation =
            (
                from r in _reservationsXML.Elements("Reservation")
                where r.Element("ReservationID").Value == _selectedReservationID
                select r).SingleOrDefault();

            string oldGuestID = reservation.Element("GuestID").Value;

            if (reservation != null)
            {
                if (cal_CheckIn.SelectedDate == null)
                {
                    MessageBox.Show
                    (
                        "Please choose a check in date.",
                        "This field can't be empty!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
                else if (string.IsNullOrEmpty(textBox_Nights.Text))
                {
                    MessageBox.Show
                    (
                        "Please enter length of stay.",
                        "This field can't be empty!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
                else if (comboBox_Room.SelectedItem == null)
                {
                    MessageBox.Show
                    (
                        "Please choose room number.",
                        "This field can't be empty!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
                else if (comboBox_Guest.SelectedItem == null)
                {
                    MessageBox.Show
                    (
                        "Please choose a guest.",
                        "This field can't be empty!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
                }
                else
                {
                    string _newGuestName = comboBox_Guest.SelectedItem.ToString();
                    string newGuestID = GetGuestIdFromName(_newGuestName);

                    // Check if guest in reservation has been changed.
                    if (oldGuestID != newGuestID)
                    {
                        ChangeGuestReservationStatus("0", oldGuestID);
                        ChangeGuestReservationStatus("1", newGuestID);

                        reservation.Element("GuestID").Value = _selectedGuestID;
                        reservation.Element("RoomID").Value = comboBox_Room.SelectedItem.ToString();
                        reservation.Element("CheckInDate").Value = cal_CheckIn.SelectedDate.Value.ToShortDateString();
                        reservation.Element("Nights").Value = textBox_Nights.Text;

                        _reservationsXML.Save(_reservationsFile);

                        // Clear reservation info for user.
                        textBox_Nights.Clear();
                        comboBox_Room.SelectedItem = null;
                        comboBox_Guest.SelectedItem = null;

                        MessageBox.Show
                        (
                            "Reservation was updated.",
                            "Success!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        // Refresh List Box displays of Guests and Reservations
                        DisplayReservationsInListBox();
                        DisplayAllGuestsInListBox();

                        Console.WriteLine($"> Reservation updated..");
                    }
                    else
                    {
                        reservation.Element("GuestID").Value = _selectedGuestID;
                        reservation.Element("RoomID").Value = comboBox_Room.SelectedItem.ToString();
                        reservation.Element("CheckInDate").Value = cal_CheckIn.SelectedDate.Value.ToShortDateString();
                        reservation.Element("Nights").Value = textBox_Nights.Text;

                        _reservationsXML.Save(_reservationsFile);

                        // Clear reservation info for user.
                        textBox_Nights.Clear();
                        comboBox_Room.SelectedItem = null;
                        comboBox_Guest.SelectedItem = null;

                        MessageBox.Show
                        (
                            "Reservation was updated.",
                            "Success!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        // Refresh List Box displays of Guests and Reservations
                        DisplayReservationsInListBox();
                        DisplayAllGuestsInListBox();

                        Console.WriteLine($"> Reservation updated..");
                    }

                }
            }
        }
        #endregion

        #region Delete Methods
        // Return XML Node from "Guests.xml" that matches user-selected Name from "listBox_Guests"
        // Remove it, and save changes to "Guests.xml"
        // Show Success message if Deletion was complete
        // Display updated list of Names in "listBox_Guests"
        private void btn_DeleteGuest_Click(object sender, RoutedEventArgs e)
        {
            XElement guest =
            (
                from g in _guestsXML.Elements("Guest")
                where g.Element("Name").Value == _selectedGuestName
                select g).SingleOrDefault();

            if (guest != null)
            {
                guest.Remove();

                textBox_Name.Text = "";
                MessageBox.Show
                (
                    "Guest was deleted.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                _guestsXML.Save(_guestsFile);
                DisplayAllGuestsInListBox();

                Console.WriteLine("> Deleted guest..");
            }
        }

        private void btn_DeleteReservation_Click(object sender, RoutedEventArgs e)
        {
            XElement reservation =
            (
                from r in _reservationsXML.Elements("Reservation")
                where r.Element("ReservationID").Value == _selectedReservationID
                select r).SingleOrDefault();

            if (reservation != null)
            {
                reservation.Remove();

                textBox_Name.Text = "";
                MessageBox.Show
                (
                    "Reservation was deleted.",
                    "Success!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                _selectedGuestID = reservation.Element("GuestID").Value;
                ChangeGuestReservationStatus("0", _selectedGuestID);

                _reservationsXML.Save(_reservationsFile);
                DisplayReservationsInListBox();

                Console.WriteLine("> Deleted reservation..");
            }
        }
        #endregion

        #region Get Values
        // Return GuestID by searching "Guests.xml" for matches on Guest Name 
        private string GetGuestIdFromName(string _selectedGuestName)
        {
            XElement guestID =
            (
                from g in _guestsXML.Elements("Guest")
                where g.Element("Name").Value == _selectedGuestName
                select g
            ).SingleOrDefault();

            _selectedGuestID = guestID.Element("GuestID").Value;

            return _selectedGuestID;
        }

        private void GetReservationsFromGuestID(string _selectedGuestID)
        {
            IEnumerable<XElement> reservations =
            (
                from r in _reservationsXML.Elements("Reservation")
                where r.Element("GuestID").Value == _selectedGuestID
                select r
            );

            DisplayReservationInfo(reservations);
        }

        private bool GuestIdExistsInReservationsXML(string _selectedGuestID)
        {
            XElement reservation =
            (
                from r in _reservationsXML.Elements("Reservation")
                where r.Element("GuestID").Value == _selectedGuestID
                select r).SingleOrDefault();

            if (reservation != null)
            {
                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Guest has reservation..");

                return true;
            }
            else
            {
                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Guest ain't got no reservation..");

                return false;
            }

        }
        #endregion

        #region Set Values
        // Set user-selected Guest Name variable value from "comboBox_Guest" Drop-Down List
        private void comboBox_Guest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Guest.SelectedItem != null)
            {
                _selectedGuestName = comboBox_Guest.SelectedItem.ToString();

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Guest name: {_selectedGuestName}");
            }
        }

        // Set user-selected Room ID / Number variable value from "_comboBox_Room" Drop-Down List
        private void comboBox_Room_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Room.SelectedItem != null)
            {
                _selectedRoomID = comboBox_Room.SelectedItem.ToString();

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Room ID: {_selectedRoomID}");
            }
        }

        // Set user-selected Check In Date variable value from "cal_CheckIn" Calendar
        private void cal_CheckIn_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cal_CheckIn.SelectedDate != null)
            {
                _selectedCheckIn = cal_CheckIn.SelectedDate.Value.ToShortDateString();

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Check in date: {_selectedCheckIn}");
            }
        }

        // Set user-selected Length Of Stay variable value from "textBox_Nights"
        private void textBox_Nights_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_Nights.Text != null)
            {
                _selectedLengthOfStay = textBox_Nights.Text;

                _timeStamp = DateTime.Now.ToLongTimeString();
                Console.WriteLine($"{_timeStamp}: > Length of stay: {_selectedLengthOfStay}");
            }
        }
        #endregion

    }
}
