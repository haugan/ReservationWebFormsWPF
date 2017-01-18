<%@ Page Title="Web Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <p>
        <label for="CheckInDate">Check in date: </label>
        <asp:Calendar ID="CheckInDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="265px">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
    </p>

    <p>
        <label for="LengthOfStay">Length of stay (nights): </label>
        <asp:TextBox ID="LengthOfStay" runat="server" Width="97px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="LengthOfStayValidator" runat="server" ControlToValidate="LengthOfStay" ErrorMessage="* This field is required!" Display="Dynamic" ValidationGroup="AllValidators" ForeColor="Red" ></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="LengthOfStay" ErrorMessage="* Enter a number between 0 and 30!" Display="Dynamic" MaximumValue="31" MinimumValue="1" Type="Integer" ValidationGroup="AllValidators" ForeColor="Red"></asp:RangeValidator>
    </p>

    <p>
        <label for="RoomID">Room: </label>
        <%--<asp:TextBox ID="RoomID" runat="server" Width="97px"></asp:TextBox>--%>
        <asp:DropDownList ID="RoomID" runat="server" Width="97px"></asp:DropDownList>
    </p>

    <p>
        <label for="GuestName" runat="server">Name: </label>
        <asp:TextBox ID="GuestName" runat="server" Width="214px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="GuestName" Display="Dynamic" ErrorMessage="* This field is required!" ForeColor="Red" ValidationGroup="AllValidators"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator runat=server display=dynamic controltovalidate="GuestName" errormessage="* Enter between 2 and 40 letters" validationexpression="[a-zA-Z]{6,10}" ForeColor="Red" ValidationGroup="AllValidators" />
    </p>

    <p>
        &nbsp;</p>

    <p>
        <asp:Button ID="Register" text="Register" runat="server" ValidationGroup="AllValidators" OnClick="Register_Click" />
    </p>


    <p>
        <asp:Label ID="LabelMessage" Text="" runat="server"></asp:Label>
    </p>

</asp:Content>