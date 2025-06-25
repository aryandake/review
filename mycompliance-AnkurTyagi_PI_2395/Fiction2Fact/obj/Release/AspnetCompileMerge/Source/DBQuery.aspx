<%@ Page Language="C#" AutoEventWireup="true" Inherits="Fiction2Fact.DBQuery_Test"  EnableEventValidation="false"  Codebehind="DBQuery.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title> DB Query</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <center>
    <br />
        <div class="ContentHeader">
            DB Query
        </div>
        <br />
        <asp:Label ID="lblMsg" runat="server" />
    </center>
    <table width="100%" cellpadding="2" cellspacing="1">
        <tr>
            <td class="tabhead">
                Query :
                <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*"></asp:Label></td>
            <td class="tabbody">
                <asp:TextBox CssClass="form-control" ID="txtQuery" MaxLength="50" runat="server" Rows="4" TextMode="MultiLine"
                    Columns="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvQuery" runat="server" ControlToValidate="txtQuery"
                    Display="Dynamic" ValidationGroup="Save" ErrorMessage="Please enter Query."
                    SetFocusOnError="True">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td class="tabhead">
        If Select Query :
        </td>
        <td class="tabbody">
         <asp:RadioButtonList ID="rblSelect" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem> </asp:RadioButtonList>
        </td>
        </tr>
          <tr>
            <td colspan="2" align="center">
                <asp:Button CssClass="Button" ID="btnSave" runat="server" CausesValidation="true"
                    Text="Save" OnClick="btnSave_Click" ValidationGroup="Save" />
            </td>
        </tr>
    </table>
    <br />
      <asp:GridView ID="gvDetails" PageSize="5" runat="server" AutoGenerateColumns="True"
               AllowSorting="false" AllowPaging="false" GridLines="Both"
                CellPadding="4"  
                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Sr.No.</HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblsrno" Text='<%# Container.DataItemIndex + 1  %>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                    </asp:GridView>
    </div>
    </form>
</body>
</html>
