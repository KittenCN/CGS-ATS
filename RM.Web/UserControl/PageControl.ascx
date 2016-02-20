<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageControl.ascx.cs"
    Inherits="RM.Web.UserControl.PageControl" %>
<link href="/Themes/Styles/pagination.css" rel="stylesheet" type="text/css" />
<%--自定义用户分页控件--%>
<script type="text/javascript">
    $(function () {
        $(".pagination a").hover(function () {
            $(this).addClass("pageBase_Div");
        }, function () {
            $(this).removeClass("pageBase_Div");
        });
    })
    function Script(parm) {
        Loading(true);
        $("#splast").removeClass('pageBase_Div');
        $("#spnext").removeClass('pageBase_Div');
        $("#spprev").removeClass('pageBase_Div');
        $("#spfirst").removeClass('pageBase_Div');
        if (parm == 1) {
            $("#spfirst").attr('disabled', 'disabled');
            $("#spprev").attr('disabled', 'disabled');
            $("#spnext").attr('disabled', 'disabled');
            $("#splast").attr('disabled', 'disabled');
            $("#splast").addClass('pageBase_Div');
            $("#spnext").addClass('pageBase_Div');
            $("#spprev").addClass('pageBase_Div');
            $("#spfirst").addClass('pageBase_Div');
        } else if (parm == 2) {
            $("#spfirst").attr('disabled', 'disabled');
            $("#spprev").attr('disabled', 'disabled');
            $("#spnext").attr('disabled', '');
            $("#splast").attr('disabled', '');
            $("#spfirst").addClass('pageBase_Div');
            $("#spprev").addClass('pageBase_Div');
        } else if (parm == 3) {
            $("#spfirst").attr('disabled', '');
            $("#spprev").attr('disabled', '');
            $("#spnext").attr('disabled', 'disabled');
            $("#splast").attr('disabled', 'disabled');
            $("#spnext").addClass('pageBase_Div');
            $("#splast").addClass('pageBase_Div');
        } else {
            $("#spfirst").attr('disabled', '');
            $("#spprev").attr('disabled', '');
            $("#spnext").attr('disabled', '');
            $("#splast").attr('disabled', '');
        }
    }
</script>
<div class="pagination">
    <div style="float: left; padding-top: 5px;">
        &nbsp;检索到
        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
        条记录，显示第
        <asp:Label ID="default_pgStartRecord" runat="server"></asp:Label>
        条 - 第
        <asp:Label ID="default_pgEndRecord" runat="server"></asp:Label>
        条
    </div>
    <div style="float: right;">
        <table border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td>
                        <div class="pagination-btn-separator" />
                    </td>
                    <td>
                        &nbsp;
                        <asp:LinkButton ID="hlkFirst" title="首 页" runat="server" OnClick="hlkFirst_Click">
                        <span id="spfirst" class="first">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        &nbsp;
                        <asp:LinkButton ID="hlkPrev" title="上 页" runat="server" OnClick="hlkPrev_Click"> 
                            <span id="spprev" class="prev">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <div class="pagination-btn-separator" />
                    </td>
                    <td>
                        &nbsp; 第
                        <asp:Label ID="lblCurrentPageIndex" runat="server" Text="1"></asp:Label>
                        页&nbsp;/&nbsp;共
                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                        页&nbsp;&nbsp;
                    </td>
                    <td>
                        <div class="pagination-btn-separator" />
                    </td>
                    <td>
                        &nbsp;
                        <asp:LinkButton ID="hlkNext" title="下 页" runat="server" OnClick="hlkNext_Click">
                         <span id="spnext" class="next" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        &nbsp;
                        <asp:LinkButton ID="hlkLast" title="尾 页" runat="server" OnClick="hlkLast_Click">
                       <span id="splast" class="last" >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        </asp:LinkButton>
                    </td>
                    <td>
                        <div class="pagination-btn-separator" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpageList" CssClass="select" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlpageList_SelectedIndexChanged">
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem Selected="True">20</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
