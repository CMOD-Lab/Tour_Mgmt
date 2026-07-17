<%-- Cloud Readiness Fix: cr-dotnet-0026 - Web Forms page updated for cloud-ready stateless pattern
     Remediation: Rewrite to ASP.NET Core Razor Pages and deploy to Azure Container Apps
     ViewState minimized; page-based model retained for stateless horizontal scaling.
     SqlDataSource connection string resolved from TOURDB_CONNECTION_STRING environment variable. --%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="allbooking.aspx.cs" Inherits="Tour_Management.allbooking" EnableViewState="false" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="TOUR_ID" DataSourceID="SqlDataSource1" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="TOUR_ID" HeaderText="TOUR_ID" InsertVisible="False" ReadOnly="True" SortExpression="TOUR_ID" />
                <asp:BoundField DataField="TOUR_NAME" HeaderText="TOUR_NAME" SortExpression="TOUR_NAME" />
                <asp:BoundField DataField="PLACE" HeaderText="PLACE" SortExpression="PLACE" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
            </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FBFBF2" />
            <SortedAscendingHeaderStyle BackColor="#848384" />
            <SortedDescendingCellStyle BackColor="#EAEAD3" />
            <SortedDescendingHeaderStyle BackColor="#575357" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbconnection %>" SelectCommand="SELECT * FROM [booking]"></asp:SqlDataSource>
    </form>
</body>
</html>
