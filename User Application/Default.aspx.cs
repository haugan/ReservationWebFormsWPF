using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Xml.Linq;

public partial class _Default : Page
{
    #region Private Variables
    private string _guestName;
    private string _roomID;
    private string _checkInDate;
    private string _lengthOfStay;

    private string _hasReservation;

    private string _guestID;
    private string _reservationID;

    private Random r = new Random();

    private string _reservationsFile = @"C:\XML\Reservations.xml";
    private string _guestsFile = @"C:\XML\Guests.xml";
    private string _roomsFile = @"C:\XML\Rooms.xml";

    private XElement _reservationsXML;
    private XElement _guestsXML;
    private XElement _roomsXML;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        _guestName = null;
        _roomID = null;
        _checkInDate = null;
        _lengthOfStay = null;
        _hasReservation = null;

        _guestID = r.Next(10000).ToString();
        _reservationID = r.Next(10000).ToString();

        _reservationsXML = XElement.Load(_reservationsFile);
        _guestsXML = XElement.Load(_guestsFile);
        _roomsXML = XElement.Load(_roomsFile);

        DisplayRoomsInListBox();

        // Clear validation error messages.
        LabelMessage.Text = "";
    }

    private void DisplayRoomsInListBox()
    {
        if (!Page.IsPostBack)
        {
            IEnumerable<XElement> elements =
            (
                from r
                in _roomsXML.Elements("Room").Elements("RoomID")
                select r
            );

            foreach (var elm in elements)
            {
                RoomID.Items.Add(elm.Value);
            }
        }
    }

    protected void Register_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            _guestName = GuestName.Text;
            _hasReservation = "1";
            _roomID = RoomID.SelectedItem.Value.ToString();
            _checkInDate = CheckInDate.SelectedDate.ToShortDateString();
            _lengthOfStay = LengthOfStay.Text.ToString();

            UpdateGuestsXML(_guestID, _guestName, _hasReservation);
            UpdateReservationsXML(_reservationID, _guestID, _roomID, _checkInDate, _lengthOfStay);

            Response.Redirect("~/About");
        }
    }

    private void UpdateGuestsXML(string _guestID, string _guestName, string _hasReservation)
    {
        XElement guest =
                    new XElement("Guest",
                        new XElement("GuestID", _guestID),
                        new XElement("Name", _guestName),
                        new XElement("HasReservation", _hasReservation)
                    );

        _guestsXML.Add(guest);
        _guestsXML.Save(_guestsFile);
    }

    private void UpdateReservationsXML(string _reservationID, string _guestID, string _roomID, string _checkInDate, string _lengthOfStay)
    {
        XElement reservation =
                    new XElement("Reservation",
                        new XElement("ReservationID", r.Next(10000)),
                        new XElement("GuestID", _guestID),
                        new XElement("RoomID", _roomID),
                        new XElement("CheckInDate", _checkInDate),
                        new XElement("Nights", _lengthOfStay)
                    );

        _reservationsXML.Add(reservation);
        _reservationsXML.Save(_reservationsFile);
    }
}